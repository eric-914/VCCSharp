using System;
using System.Collections.Generic;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICoCo
    {
        unsafe float RenderFrame(EmuState* emuState);
        void CocoReset();
        void SetClockSpeed(ushort cycles);
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

    public class CoCo : ICoCo
    {
        public const double CyclesPerSecond = (Define.COLORBURST / 4) * (Define.TARGETFRAMERATE / Define.FRAMESPERSECORD);
        public const double LinesPerSecond = Define.TARGETFRAMERATE * (double)Define.LINESPERFIELD;
        public const double PicosPerLine = Define.PICOSECOND / LinesPerSecond;
        public const double CyclesPerLine = CyclesPerSecond / LinesPerSecond;
        private const byte BLINK_PHASE = 1;

        private Action _audioEvent = () => { };

        private readonly IModules _modules;

        private static byte _lastMode;

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
        private byte _horizontalInterruptEnabled;
        private byte _verticalInterruptEnabled;
        private byte _topBorder;
        private byte _bottomBorder;
        private byte _linesPerScreen;
        private byte _throttle;

        private ushort _timerClockRate;
        private ushort _soundRate;
        private ushort _audioIndex;

        private uint _stateSwitch;

        private int _clipCycle = 1;
        private int _waitCycle = 2000;
        private int _intEnable;
        private int _sndEnable = 1;

        private readonly uint[] _audioBuffer = new uint[16384];
        private readonly byte[] _cassetteBuffer = new byte[8192];

        public int OverClock { get; set; }

        public CoCo(IModules modules)
        {
            _modules = modules;

            UpdateTapeDialog = offset => { }; //_modules.Config.UpdateTapeDialog((uint)offset);
        }

        public void CocoReset()
        {
            _horizontalInterruptEnabled = 0;
            _verticalInterruptEnabled = 0;
            _timerClockRate = 0;
            _masterTickCounter = 0;
            _partialTickCounter = 0;
            _oldMaster = 0;

            _soundInterrupt = 0;
            _picosToSoundSample = 0;
            _cycleDrift = 0;
            _cyclesThisLine = 0;
            _picosThisLine = 0;
            _intEnable = 0;
            _audioIndex = 0;
        }

        public void SetClockSpeed(ushort cycles)
        {
            OverClock = cycles;
        }

        #region RenderFrame

        public unsafe float RenderFrame(EmuState* emuState)
        {
            if (RenderVideoFrame(emuState) == 1)
            {
                return 0;
            }

            RenderAudioFrame();

            return _modules.Throttle.CalculateFPS();
        }

        private unsafe byte RenderVideoFrame(EmuState* emuState)
        {
            _modules.Graphics.SetBlinkState(BLINK_PHASE);

            _modules.MC6821.irq_fs(PhaseStates.Falling);   //FS low to High transition start of display Blink needs this

            //TODO: I don't think updating _modules.Emu.LineCounter = counter is needed.

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < 13; _modules.Emu.LineCounter++)
            //Vertical Blanking 13 H lines
            for (short counter = 0; counter < 13; counter++)
            {
                _modules.Emu.LineCounter = counter;
                CpuCycle();
            }

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < 4; _modules.Emu.LineCounter++)
            //4 non-Rendered top Border lines
            for (short counter = 0; counter < 4; counter++)
            {
                _modules.Emu.LineCounter = counter;
                CpuCycle();
            }

            if (_modules.Emu.FrameCounter % _modules.Emu.FrameSkip == Define.FALSE)
            {
                if (_modules.DirectDraw.LockScreen(emuState) == Define.TRUE)
                {
                    return 1;
                }
            }

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < cocoState->TopBorder - 4; _modules.Emu.LineCounter++)
            for (short counter = 0; counter < _topBorder - 4; counter++)
            {
                _modules.Emu.LineCounter = counter;
                if (_modules.Emu.FrameCounter % _modules.Emu.FrameSkip == Define.FALSE)
                {
                    CoCoDrawTopBorder(emuState);
                }

                CpuCycle();
            }

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < cocoState->LinesPerScreen; _modules.Emu.LineCounter++)
            //Active Display area
            for (short counter = 0; counter < _linesPerScreen; counter++)
            {
                _modules.Emu.LineCounter = counter;
                CpuCycle();

                if (_modules.Emu.FrameCounter % _modules.Emu.FrameSkip == Define.FALSE)
                {
                    CoCoUpdateScreen(emuState);
                }
            }

            _modules.MC6821.irq_fs(PhaseStates.Rising);  //End of active display FS goes High to Low

            if (_verticalInterruptEnabled != Define.FALSE)
            {
                _modules.TC1014.GimeAssertVertInterrupt();
            }

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < (cocoState->BottomBorder); _modules.Emu.LineCounter++)
            //Bottom border
            for (short counter = 0; counter < _bottomBorder; counter++)
            {
                _modules.Emu.LineCounter = counter;
                CpuCycle();

                if (_modules.Emu.FrameCounter % _modules.Emu.FrameSkip == Define.FALSE)
                {
                    CoCoDrawBottomBorder(emuState);
                }
            }

            if (_modules.Emu.FrameCounter % _modules.Emu.FrameSkip == Define.FALSE)
            {
                _modules.DirectDraw.UnlockScreen(emuState);
                _modules.Graphics.SetBorderChange();
            }

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < 6; _modules.Emu.LineCounter++)
            //Vertical Retrace 6 H lines
            for (short counter = 0; counter < 4; counter++)
            {
                _modules.Emu.LineCounter = counter;
                CpuCycle();
            }

            return 0;
        }

        private void RenderAudioFrame()
        {
            unsafe
            {
                switch (_soundOutputMode)
                {
                    case 0:
                        fixed (uint* buffer = _audioBuffer)
                        {
                            _modules.Audio.FlushAudioBuffer(buffer, (ushort)(_audioIndex << 2));
                        }
                        break;

                    case 1:
                        fixed (byte* buffer = _cassetteBuffer)
                        {
                            FlushCassetteBuffer(buffer, _audioIndex);
                        }

                        break;

                    case 2:
                        LoadCassetteBuffer();
                        break;
                }

                _audioIndex = 0;
            }
        }


        #endregion

        #region CPUCycle

        private /* _inline */ void CpuCycle()
        {
            if (_horizontalInterruptEnabled == Define.TRUE)
            {
                _modules.TC1014.GimeAssertHorzInterrupt();
            }

            _modules.MC6821.irq_hs(PhaseStates.Any);
            _modules.PAKInterface.PakTimer();

            _picosThisLine += PicosPerLine;

            while (_picosThisLine > 1)
            {
                CpuCyclePicos();
            }

            if (!_modules.Clipboard.ClipboardEmpty())
            {
                CpuCycleClipboard(_modules.Vcc);
            }
        }

        private void CpuCycleClipboard(IVcc vcc)
        {
            const byte shift = 0x36;
            char key;

            //Remember the original throttle setting.
            //Set it to off. We need speed for this!
            if (_throttle == 0)
            {
                _throttle = vcc.Throttle;

                if (_throttle == 0)
                {
                    _throttle = 2; // 2 = No throttle.
                }
            }

            vcc.Throttle = 0;

            if (_clipCycle == 1)
            {
                key = _modules.Clipboard.PeekClipboard();

                if (key == shift)
                {
                    _modules.Keyboard.KeyboardHandleKeyDown(shift, shift);  //Press shift and...
                    _modules.Clipboard.PopClipboard();
                    key = _modules.Clipboard.PeekClipboard();
                }

                _modules.Keyboard.KeyboardHandleKeyDown((byte)key, (byte)key);

                _waitCycle = key == 0x1c ? 6000 : 2000;
            }
            else if (_clipCycle == 500)
            {
                key = _modules.Clipboard.PeekClipboard();

                _modules.Keyboard.KeyboardHandleKeyUp(shift, shift);
                _modules.Keyboard.KeyboardHandleKeyUp(0x42, (byte)key); //TODO: What is 0x42?
                _modules.Clipboard.PopClipboard();

                //Finished?
                if (_modules.Clipboard.ClipboardEmpty())
                {
                    _modules.Keyboard.SetPaste(false);

                    //Done pasting. Reset throttle to original state
                    vcc.Throttle = _throttle == 2 ? (byte)0 : (byte)1;

                    //...and reset the keymap to the original state
                    ResetKeyMap();

                    _throttle = 0;
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
            if ((_picosToInterrupt <= _picosThisLine) && _intEnable == Define.TRUE)
            {
                _stateSwitch = 1;
            }

            //Does it need to collect an Audio sample
            if ((_picosToSoundSample <= _picosThisLine) && _sndEnable == Define.TRUE)
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

            if (_cyclesThisLine >= 1)
            {   //Avoid un-needed CPU engine calls
                _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) + (_cyclesThisLine - Math.Floor(_cyclesThisLine));
            }
            else
            {
                _cycleDrift = _cyclesThisLine;
            }

            _picosToInterrupt -= _picosThisLine;
            _picosToSoundSample -= _picosThisLine;
            _picosThisLine = 0;
        }

        //Only Interrupting
        private void CpuCyclePicosCase1()
        {
            _picosThisLine -= _picosToInterrupt;
            _cyclesThisLine = _cycleDrift + (_picosToInterrupt * CyclesPerLine * OverClock / PicosPerLine);

            if (_cyclesThisLine >= 1)
            {
                _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) + (_cyclesThisLine - Math.Floor(_cyclesThisLine));
            }
            else
            {
                _cycleDrift = _cyclesThisLine;
            }

            _modules.TC1014.GimeAssertTimerInterrupt();

            _picosToSoundSample -= _picosToInterrupt;
            _picosToInterrupt = _masterTickCounter;
        }

        //Only Sampling
        private void CpuCyclePicosCase2()
        {
            _picosThisLine -= _picosToSoundSample;
            _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

            if (_cyclesThisLine >= 1)
            {
                _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) + (_cyclesThisLine - Math.Floor(_cyclesThisLine));
            }
            else
            {
                _cycleDrift = _cyclesThisLine;
            }

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

                if (_cyclesThisLine >= 1)
                {
                    _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) +
                                 (_cyclesThisLine - Math.Floor(_cyclesThisLine));
                }
                else
                {
                    _cycleDrift = _cyclesThisLine;
                }

                _audioEvent();

                _picosToInterrupt -= _picosToSoundSample;
                _picosToSoundSample = _soundInterrupt;
                _picosThisLine -= _picosToInterrupt;

                _cyclesThisLine = _cycleDrift + (_picosToInterrupt * CyclesPerLine * OverClock / PicosPerLine);

                if (_cyclesThisLine >= 1)
                {
                    _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) +
                                 (_cyclesThisLine - Math.Floor(_cyclesThisLine));
                }
                else
                {
                    _cycleDrift = _cyclesThisLine;
                }

                _modules.TC1014.GimeAssertTimerInterrupt();

                _picosToSoundSample -= _picosToInterrupt;
                _picosToInterrupt = _masterTickCounter;

                return;
            }

            if (_picosToSoundSample > _picosToInterrupt)
            {
                _picosThisLine -= _picosToInterrupt;
                _cyclesThisLine = _cycleDrift + (_picosToInterrupt * CyclesPerLine * OverClock / PicosPerLine);

                if (_cyclesThisLine >= 1)
                {
                    _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) +
                                 (_cyclesThisLine - Math.Floor(_cyclesThisLine));
                }
                else
                {
                    _cycleDrift = _cyclesThisLine;
                }

                _modules.TC1014.GimeAssertTimerInterrupt();

                _picosToSoundSample -= _picosToInterrupt;
                _picosToInterrupt = _masterTickCounter;
                _picosThisLine -= _picosToSoundSample;
                _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

                if (_cyclesThisLine >= 1)
                {
                    _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) +
                                 (_cyclesThisLine - Math.Floor(_cyclesThisLine));
                }
                else
                {
                    _cycleDrift = _cyclesThisLine;
                }

                _audioEvent();

                _picosToInterrupt -= _picosToSoundSample;
                _picosToSoundSample = _soundInterrupt;

                return;
            }

            //They are the same (rare)
            _picosThisLine -= _picosToInterrupt;
            _cyclesThisLine = _cycleDrift + (_picosToSoundSample * CyclesPerLine * OverClock / PicosPerLine);

            if (_cyclesThisLine > 1)
            {
                _cycleDrift = _modules.CPU.CPUExec((int)Math.Floor(_cyclesThisLine)) +
                             (_cyclesThisLine - Math.Floor(_cyclesThisLine));
            }
            else
            {
                _cycleDrift = _cyclesThisLine;
            }

            _modules.TC1014.GimeAssertTimerInterrupt();

            _audioEvent();

            _picosToInterrupt = _masterTickCounter;
            _picosToSoundSample = _soundInterrupt;
        }

        #endregion

        private void ResetKeyMap()
        {
            int currentKeyMap = _modules.Clipboard.CurrentKeyMap;

            _modules.Keyboard.KeyboardBuildRuntimeTable((byte)currentKeyMap);
        }

        //--TODO: what is the purpose to this variable?
        private ushort _primarySoundRate;

        //0 = Speaker 1= Cassette Out 2=Cassette In
        public byte SetSndOutMode(byte mode)
        {
            unsafe
            {
                _primarySoundRate = _soundRate;

                switch (mode)
                {
                    case 0:
                        if (_lastMode == 1)
                        {
                            //Send the last bits to be encoded
                            fixed (byte* buffer = _cassetteBuffer)
                            {
                                FlushCassetteBuffer(buffer, _audioIndex); /* Cassette.cpp */
                            }
                        }

                        SetAudioEventAudioOut();
                        //SetAudioRate(primarySoundRate);
                        SetAudioRate(_primarySoundRate);

                        break;

                    case 1:
                        SetAudioEventCassetteOut();

                        _primarySoundRate = _soundRate;

                        SetAudioRate(Define.TAPEAUDIORATE);

                        break;

                    case 2:
                        SetAudioEventCassetteIn();

                        _primarySoundRate = _soundRate;

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
        }

        private unsafe void CoCoDrawTopBorder(EmuState* emuState)
        {
            var mapping = new Dictionary<BitDepthStates, Action>
            {
                {BitDepthStates.BIT_8, () => _modules.TC1014.DrawTopBorder8(emuState)},
                {BitDepthStates.BIT_16, () => _modules.TC1014.DrawTopBorder16(emuState)},
                {BitDepthStates.BIT_24, () => _modules.TC1014.DrawTopBorder24(emuState)},
                {BitDepthStates.BIT_32, () => _modules.TC1014.DrawTopBorder32(emuState)}
            };

            mapping[(BitDepthStates)_modules.Emu.BitDepth]();
        }

        private unsafe void CoCoUpdateScreen(EmuState* emuState)
        {
            var mapping = new Dictionary<BitDepthStates, Action>
            {
                {BitDepthStates.BIT_8, () => _modules.TC1014.UpdateScreen8(emuState)},
                {BitDepthStates.BIT_16, () => _modules.TC1014.UpdateScreen16(emuState)},
                {BitDepthStates.BIT_24, () => _modules.TC1014.UpdateScreen24(emuState)},
                {BitDepthStates.BIT_32, () => _modules.TC1014.UpdateScreen32(emuState)}
            };

            mapping[(BitDepthStates)_modules.Emu.BitDepth]();
        }

        private unsafe void CoCoDrawBottomBorder(EmuState* emuState)
        {
            var mapping = new Dictionary<BitDepthStates, Action>
            {
                {BitDepthStates.BIT_8, () => _modules.TC1014.DrawBottomBorder8(emuState)},
                {BitDepthStates.BIT_16, () => _modules.TC1014.DrawBottomBorder16(emuState)},
                {BitDepthStates.BIT_24, () => _modules.TC1014.DrawBottomBorder24(emuState)},
                {BitDepthStates.BIT_32, () => _modules.TC1014.DrawBottomBorder32(emuState)}
            };

            mapping[(BitDepthStates)_modules.Emu.BitDepth]();
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
            _verticalInterruptEnabled = (byte)(state == 0 ? 1 : 0);
        }

        public void SetHorizontalInterruptState(byte state)
        {
            _horizontalInterruptEnabled = (byte)(state == 0 ? 1 : 0);
        }

        public void SetTimerInterruptState(byte state)
        {
        }

        public void SetMasterTickCounter()
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

            _intEnable = _masterTickCounter == 0 ? 0 : 1;
        }

        private unsafe void FlushCassetteBuffer(byte* buffer, uint length)
        {
            uint offset = _modules.Cassette.FlushCassetteBuffer(buffer, length);

            UpdateTapeDialog((int)offset);
        }

        private unsafe void LoadCassetteBuffer()
        {
            fixed (byte* buffer = _cassetteBuffer)
            {
                _modules.Cassette.LoadCassetteBuffer(buffer);
            }
        }

        public ushort SetAudioRate(ushort rate)
        {
            _cycleDrift = 0;
            _audioIndex = 0;

            if (rate != 0)
            {	//Force Mute or 44100Hz
                rate = 44100;
            }

            if (rate == 0)
            {
                _sndEnable = 0;
                _soundInterrupt = 0;
            }
            else
            {
                _sndEnable = 1;
                _soundInterrupt = Define.PICOSECOND / rate;
                _picosToSoundSample = _soundInterrupt;
            }

            _soundRate = rate;

            return 0;
        }

        public void SetLinesPerScreen(byte lines)
        {
            lines &= 3;

            unsafe
            {
                GraphicsState* graphicsState = _modules.Graphics.GetGraphicsState();

                _linesPerScreen = graphicsState->Lpf[lines];
                _topBorder = graphicsState->VcenterTable[lines];

                //4 lines of top border are un-rendered 244-4=240 rendered scan-lines
                _bottomBorder = (byte)(243 - (_topBorder + _linesPerScreen));
            }
        }

        public void SetAudioEventAudioOut()
        {
            void AudioOut()
            {
                _audioBuffer[_audioIndex++] = _modules.MC6821.GetDACSample();
            }

            _audioEvent = AudioOut;
        }

        public void SetAudioEventCassetteOut()
        {
            void CassetteOut()
            {
                _cassetteBuffer[_audioIndex++] = _modules.MC6821.GetCassetteSample();
            }

            _audioEvent = CassetteOut;
        }

        public void SetAudioEventCassetteIn()
        {
            void CassetteIn()
            {
                _audioBuffer[_audioIndex] = _modules.MC6821.GetDACSample();

                _modules.MC6821.SetCassetteSample(_cassetteBuffer[_audioIndex++]);
            }

            _audioEvent = CassetteIn;
        }
    }
}
