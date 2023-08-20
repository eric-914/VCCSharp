using System.Diagnostics;
using VCCSharp.Configuration;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Models.Audio;
using VCCSharp.Models.Keyboard.Mappings;

namespace VCCSharp.Modules;

public interface ICoCo : IModule
{
    float RenderFrame();
    void CocoReset();
    ushort SetAudioRate(ushort rate);
    byte SetSndOutMode(byte mode);
    void SetInterruptTimer(ushort timer);
    void SetTimerClockRate(byte clockRate);
    void SetVerticalInterruptState(byte state);
    void SetHorizontalInterruptState(byte state);
    void SetTimerInterruptState(byte state);
    void SetLinesPerScreen(byte lines);
    void SetAudioEventAudioOut();

    Action<int> UpdateTapeDialog { get; set; }
    int OverClock { get; set; }
}

// ReSharper disable once InconsistentNaming
public class CoCo : ICoCo
{
    private enum ThrottleStates
    {
        Idle,
        Active
    }

    private const double CyclesPerSecond = (Define.COLORBURST / 4) * (Define.TARGETFRAMERATE / Define.FRAMESPERSECORD);
    private const double LinesPerSecond = Define.TARGETFRAMERATE * (double)Define.LINESPERFIELD;
    private const double PicosPerLine = Define.PICOSECOND / LinesPerSecond;
    private const double CyclesPerLine = CyclesPerSecond / LinesPerSecond;

    private const byte BlinkPhase = 1;

    private Action _audioEvent = () => { };

    private readonly IModules _modules;

    private static byte _lastMode;

    private IConfiguration Configuration => _modules.Configuration;

    public Action<int> UpdateTapeDialog { get; set; }

    private double _soundInterrupt;
    private double _picosToSoundSample;
    private double _cycleDrift;
    private double _cyclesThisLine;
    private double _picosToInterrupt;
    private double _oldMaster;
    private double _masterTickCounter;
    private double _partialTickCounter; //TODO: Originally named "Un-xl-at-ed"?
    private double _picosThisLine;

    private byte _soundOutputMode;
    private bool _horizontalInterruptEnabled;
    private bool _verticalInterruptEnabled;
    private byte _topBorder;
    private byte _bottomBorder;
    private byte _linesPerScreen;
    private ThrottleStates _throttleState = ThrottleStates.Idle;
    private bool _throttle;
    private bool _shiftActive;

    private ushort _timerClockRate;
    private ushort _soundRate;
    private ushort _audioIndex;

    private uint _stateSwitch;

    private int _clipCycle = 1;
    private int _waitCycle = 2000;
    private bool _intEnable;
    private bool _sndEnable = true;

    private readonly int[] _audioBuffer = new int[16384];
    private readonly byte[] _cassetteBuffer = new byte[8192];

    private readonly DACSample _dacSample = new();

    public int OverClock { get; set; }

    private IKey ShiftKey { get; }
    private IKey ReturnKey { get; }

    public CoCo(IModules modules)
    {
        _modules = modules;

        UpdateTapeDialog = _ => { }; //_modules.ConfigurationManager.UpdateTapeDialog((uint)offset);

        KeyDefinitions keyDefinitions = new();

        ShiftKey = keyDefinitions.Shift;
        ReturnKey = keyDefinitions.Return;
    }

    public void CocoReset()
    {
        _horizontalInterruptEnabled = false;
        _verticalInterruptEnabled = false;
        _timerClockRate = 0;
        _masterTickCounter = 0;
        _partialTickCounter = 0;
        _oldMaster = 0;

        _soundInterrupt = 0;
        _picosToSoundSample = 0;
        _cycleDrift = 0;
        _cyclesThisLine = 0;
        _picosThisLine = 0;
        _intEnable = false;
        _audioIndex = 0;

        OverClock = 1;
    }

    #region RenderFrame

    public float RenderFrame()
    {
        RenderAudioFrame();

        if (!RenderVideoFrame())
        {
            return 0;
        }

        return _modules.Throttle.CalculateFps();
    }

    private bool RenderVideoFrame()
    {
        bool skipRender = _modules.Emu.FrameCounter % Configuration.CPU.FrameSkip != 0;

        _modules.Graphics.SetBlinkState(BlinkPhase);

        _modules.MC6821.IrqFs(PhaseStates.Falling);   //FS low to High transition start of display Blink needs this

        void Cycle(int count)
        {
            for (short counter = 0; counter < count; counter++)
            {
                CpuCycle();
            }
        }

        //Vertical Blanking 13 H lines
        Cycle(13);

        //4 non-Rendered top Border lines
        Cycle(4);

        if (!skipRender)
        {
            if (!_modules.Draw.LockScreen())
            {
                return false; //--Failed to lock screen for rendering
            }
        }

        for (short counter = 0; counter < _topBorder - 4; counter++)
        {
            if (!skipRender)
            {
                _modules.Screen.DrawTopBorder(counter);
            }

            CpuCycle();
        }

        //Active Display area
        for (short counter = 0; counter < _linesPerScreen; counter++)
        {
            CpuCycle();

            if (!skipRender)
            {
                var mode = _modules.TCC1014.GetMode();
                _modules.Screen.DrawScreen(counter, mode);
            }
        }

        _modules.MC6821.IrqFs(PhaseStates.Rising);  //End of active display FS goes High to Low

        if (_verticalInterruptEnabled)
        {
            _modules.TCC1014.GimeAssertVerticalInterrupt();
        }

        //Bottom border
        for (short counter = 0; counter < _bottomBorder; counter++)
        {
            CpuCycle();

            if (!skipRender)
            {
                _modules.Screen.DrawBottomBorder(counter);
            }
        }

        if (!skipRender)
        {
            _modules.Draw.UnlockScreen();
            _modules.Graphics.SetBorderChange();
        }

        //Vertical Retrace 6 H lines
        Cycle(6);

        return true; //--Frame was rendered
    }

    private void RenderAudioFrame()
    {
        switch (_soundOutputMode)
        {
            case 0:
                _modules.Audio.FlushAudioBuffer(_audioBuffer, (ushort)(_audioIndex << 2));
                break;

            case 1:
                FlushCassetteBuffer(_cassetteBuffer, _audioIndex);
                break;

            case 2:
                LoadCassetteBuffer();
                break;
        }

        _audioIndex = 0;
    }


    #endregion

    #region CPUCycle

    private /* _inline */ void CpuCycle()
    {
        if (_horizontalInterruptEnabled)
        {
            _modules.TCC1014.GimeAssertHorizontalInterrupt();
        }

        _modules.MC6821.IrqHs(PhaseStates.Any);
        _modules.PAKInterface.PakTimer();

        _picosThisLine += PicosPerLine;

        while (_picosThisLine > 1)
        {
            CpuCyclePicos();
        }

        if (!_modules.Clipboard.ClipboardEmpty())
        {
            CpuCycleClipboard();
        }
    }

    private void CpuCycleClipboard()
    {
        IKey key;

        var configuration = _modules.Configuration;

        //Remember the original throttle setting.
        //Set it to off. We need speed for this!
        if (_throttleState == ThrottleStates.Idle)
        {
            _throttle = configuration.CPU.ThrottleSpeed;
            _throttleState = ThrottleStates.Active;
        }

        configuration.CPU.ThrottleSpeed = false;

        if (_clipCycle == 1)
        {
            key = _modules.Clipboard.PeekClipboard();

            if (key.Shift)
            {
                _shiftActive = true;

                _modules.Keyboard.KeyboardHandleKey(ShiftKey.DIK, KeyStates.Down);  //Press shift and...
            }

            _modules.Keyboard.KeyboardHandleKey(key.DIK, KeyStates.Down);

            _waitCycle = key.ScanCode == ReturnKey.ScanCode ? 6000 : 2000;
        }
        else if (_clipCycle == 500)
        {
            key = _modules.Clipboard.PeekClipboard();

            if (_shiftActive)
            {
                _modules.Keyboard.KeyboardHandleKey(ShiftKey.DIK, KeyStates.Up);
                _shiftActive = false;
            }

            _modules.Keyboard.KeyboardHandleKey(key.DIK, KeyStates.Up);

            _modules.Clipboard.PopClipboard();

            if (_modules.Clipboard.Abort)
            {
                _modules.Clipboard.ClearClipboard();
            }

            //Finished?
            if (_modules.Clipboard.ClipboardEmpty())
            {
                //Done pasting. Reset throttle to original state
                configuration.CPU.ThrottleSpeed = _throttle;

                //...and reset the keymap to the original state
                _modules.Keyboard.ResetKeyboardLayout();

                _throttleState = ThrottleStates.Idle;
            }
        }

        _clipCycle++;

        if (_clipCycle > _waitCycle)
        {
            _clipCycle = 1;
        }
    }

    private void CpuCyclePicos()
    {
        _stateSwitch = 0;

        //Does this iteration need to Timer Interrupt
        if (_picosToInterrupt <= _picosThisLine && _intEnable)
        {
            _stateSwitch = 1;
        }

        //Does it need to collect an Audio sample
        if (_picosToSoundSample <= _picosThisLine && _sndEnable)
        {
            _stateSwitch += 2;
        }

        var cases = new Dictionary<uint, Action>
        {
            {0, CpuCyclePicosCase0 },
            {1, CpuCyclePicosCase1 },
            {2, CpuCyclePicosCase2 },
            {3, CpuCyclePicosCase3 }
        };

        cases[_stateSwitch]();
    }

    //No interrupts this line
    private void CpuCyclePicosCase0()
    {
        _cyclesThisLine = _cycleDrift + (_picosThisLine * CyclesPerLine * OverClock / PicosPerLine);

        _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

        _picosToInterrupt -= _picosThisLine;
        _picosToSoundSample -= _picosThisLine;
        _picosThisLine = 0;
    }

    //Only Interrupting
    private void CpuCyclePicosCase1()
    {
        _picosThisLine -= _picosToInterrupt;
        _cyclesThisLine = _cycleDrift + (_picosToInterrupt * CyclesPerLine * OverClock / PicosPerLine);

        _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

        _modules.TCC1014.GimeAssertTimerInterrupt();

        _picosToSoundSample -= _picosToInterrupt;
        _picosToInterrupt = _masterTickCounter;
    }

    //Only Sampling
    private void CpuCyclePicosCase2()
    {
        _picosThisLine -= _picosToSoundSample;
        _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

        _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

        _audioEvent();

        _picosToInterrupt -= _picosToSoundSample;
        _picosToSoundSample = _soundInterrupt;
    }

    //Interrupting and Sampling
    private void CpuCyclePicosCase3()
    {
        if (_picosToSoundSample < _picosToInterrupt)
        {
            _picosThisLine -= _picosToSoundSample;
            _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

            _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

            _audioEvent();

            _picosToInterrupt -= _picosToSoundSample;
            _picosToSoundSample = _soundInterrupt;
            _picosThisLine -= _picosToInterrupt;

            _cyclesThisLine = _cycleDrift + (_picosToInterrupt * CyclesPerLine * OverClock / PicosPerLine);

            _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

            _modules.TCC1014.GimeAssertTimerInterrupt();

            _picosToSoundSample -= _picosToInterrupt;
            _picosToInterrupt = _masterTickCounter;

            return;
        }

        if (_picosToSoundSample > _picosToInterrupt)
        {
            _picosThisLine -= _picosToInterrupt;
            _cyclesThisLine = _cycleDrift + (_picosToInterrupt * CyclesPerLine * OverClock / PicosPerLine);

            _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

            _modules.TCC1014.GimeAssertTimerInterrupt();

            _picosToSoundSample -= _picosToInterrupt;
            _picosToInterrupt = _masterTickCounter;
            _picosThisLine -= _picosToSoundSample;
            _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

            _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

            _audioEvent();

            _picosToInterrupt -= _picosToSoundSample;
            _picosToSoundSample = _soundInterrupt;

            return;
        }

        //They are the same (rare)
        _picosThisLine -= _picosToInterrupt;
        _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

        _cycleDrift = _cyclesThisLine >= 1 ? _modules.CPU.Exec(_cyclesThisLine) : _cyclesThisLine;

        _modules.TCC1014.GimeAssertTimerInterrupt();

        _audioEvent();

        _picosToInterrupt = _masterTickCounter;
        _picosToSoundSample = _soundInterrupt;
    }

    #endregion

    //0=Speaker
    //1=Cassette Out
    //2=Cassette In
    public byte SetSndOutMode(byte mode)
    {
        switch (mode)
        {
            case 0:
                if (_lastMode == 1)
                {
                    //Send the last bits to be encoded
                    FlushCassetteBuffer(_cassetteBuffer, _audioIndex); /* Cassette.cpp */
                }

                SetAudioEventAudioOut();
                //SetAudioRate(primarySoundRate);
                SetAudioRate(_soundRate);

                break;

            case 1:
                SetAudioEventCassetteOut();

                SetAudioRate(Define.TAPEAUDIORATE);

                break;

            case 2:
                SetAudioEventCassetteIn();

                SetAudioRate(Define.TAPEAUDIORATE);

                break;

            default: //QUERY
                return _soundOutputMode;
        }

        if (mode != _lastMode)
        {
            _audioIndex = 0;	//Reset Buffer on true mode switch
            _lastMode = mode;
        }

        _soundOutputMode = mode;

        return _soundOutputMode;
    }

    public void SetInterruptTimer(ushort timer)
    {
        _partialTickCounter = (timer & 0xFFF);

        SetMasterTickCounter();
    }

    public void SetTimerClockRate(byte clockRate)
    {
        //1= 279.265nS (1/ColorBurst)
        //0= 63.695uS  (1/60*262)  1 scan line time

        _timerClockRate = (ushort)(clockRate == 0 ? 0 : 1);

        SetMasterTickCounter();
    }

    public void SetVerticalInterruptState(byte state)
    {
        _verticalInterruptEnabled = state == 0;
    }

    public void SetHorizontalInterruptState(byte state)
    {
        _horizontalInterruptEnabled = state == 0;
    }

    public void SetTimerInterruptState(byte state)
    {
    }

    private void SetMasterTickCounter()
    {
        double[] rate = { Define.PICOSECOND / (Define.TARGETFRAMERATE * Define.LINESPERFIELD), Define.PICOSECOND / Define.COLORBURST };

        if (_partialTickCounter == 0)
        {
            _masterTickCounter = 0;
        }
        else
        {
            _masterTickCounter = (_partialTickCounter + 2) * rate[_timerClockRate];
        }

        if (Math.Abs(_masterTickCounter - _oldMaster) > 0.0001)
        {
            _oldMaster = _masterTickCounter;
            _picosToInterrupt = _masterTickCounter;
        }

        _intEnable = _masterTickCounter != 0;
    }

    private void FlushCassetteBuffer(byte[] buffer, uint length)
    {
        uint offset = _modules.Cassette.FlushCassetteBuffer(buffer, length);

        UpdateTapeDialog((int)offset);
    }

    private void LoadCassetteBuffer()
    {
        _modules.Cassette.LoadCassetteBuffer(_cassetteBuffer);
    }

    public ushort SetAudioRate(ushort rate)
    {
        _cycleDrift = 0;
        _audioIndex = 0;

        if (rate != 0)
        {	//Force Disabled or 44100Hz
            rate = 44100;
        }

        if (rate == 0)
        {
            _sndEnable = false;
            _soundInterrupt = 0;
        }
        else
        {
            _sndEnable = true;
            _soundInterrupt = Define.PICOSECOND / rate;
            _picosToSoundSample = _soundInterrupt;
        }

        _soundRate = rate;

        return 0;
    }

    public void SetLinesPerScreen(byte lines)
    {
        _linesPerScreen = _modules.Graphics.Lpf[lines];
        _topBorder = _modules.Graphics.VerticalCenterTable[lines];

        //4 lines of top border are un-rendered 244-4=240 rendered scan-lines
        _bottomBorder = (byte)(243 - (_topBorder + _linesPerScreen));
    }

    public void SetAudioEventAudioOut()
    {
        void AudioOut()
        {
            _modules.MC6821.GetDACSample(_dacSample);

            _audioBuffer[_audioIndex++] = _dacSample.Sample;
        }

        _audioEvent = AudioOut;
    }

    private void SetAudioEventCassetteOut()
    {
        void CassetteOut()
        {
            _cassetteBuffer[_audioIndex++] = _modules.MC6821.GetCassetteSample();
        }

        _audioEvent = CassetteOut;
    }

    private void SetAudioEventCassetteIn()
    {
        void CassetteIn()
        {
            _modules.MC6821.GetDACSample(_dacSample);

            _audioBuffer[_audioIndex] = _dacSample.Sample;

            _modules.MC6821.SetCassetteSample(_cassetteBuffer[_audioIndex++]);
        }

        _audioEvent = CassetteIn;
    }

    public void ModuleReset()
    {
        Debug.WriteLine("CoCo.ModuleReset()");

        CocoReset();

        //private Action _audioEvent = () => { };
        //public Action<int> UpdateTapeDialog { get; set; }

        //private byte _topBorder;
        //private byte _bottomBorder;
        //private byte _linesPerScreen;

        //private ushort _soundRate;

        //public int OverClock { get; set; }

        _lastMode = 0;

        _picosToInterrupt = 0;
        _soundOutputMode = 0;

        _throttleState = ThrottleStates.Idle;
        _throttle = Configuration.CPU.ThrottleSpeed;
        _shiftActive = false;

        _stateSwitch = 0;

        _clipCycle = 1;
        _waitCycle = 2000;
        _sndEnable = true;

        _audioBuffer.Initialize();
        _cassetteBuffer.Initialize();
    }
}
