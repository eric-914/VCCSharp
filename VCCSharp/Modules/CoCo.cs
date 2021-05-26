using System;
using System.Collections.Generic;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICoCo
    {
        unsafe CoCoState* GetCoCoState();
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
    }

    public class CoCo : ICoCo
    {
        public const double CyclesPerSecond = (Define.COLORBURST / 4) * (Define.TARGETFRAMERATE / Define.FRAMESPERSECORD);
        public const double LinesPerSecond = Define.TARGETFRAMERATE * (double)Define.LINESPERFIELD;
        public const double PicosPerLine = Define.PICOSECOND / LinesPerSecond;
        public const double CyclesPerLine = CyclesPerSecond / LinesPerSecond;

        private Action _audioEvent = () => { };

        private readonly IModules _modules;

        private static byte _lastMode;

        public Action<int> UpdateTapeDialog { get; set; }

        private double SoundInterrupt;
        private double PicosToSoundSample;
        private double CycleDrift;
        private double CyclesThisLine;
        private double PicosToInterrupt;
        private double OldMaster;
        private double MasterTickCounter;
        private double UnxlatedTickCounter; //TODO: Unxlated?
        private double PicosThisLine;

        private byte SoundOutputMode;
        private byte HorzInterruptEnabled;
        private byte VertInterruptEnabled;
        private byte TopBorder;
        private byte BottomBorder;
        private byte LinesperScreen;
        private byte TimerInterruptEnabled;
        private byte BlinkPhase = 1;
        private byte Throttle;

        private ushort TimerClockRate;
        private ushort SoundRate;
        private ushort AudioIndex;

        private uint StateSwitch;

        private int ClipCycle = 1;
        private int WaitCycle = 2000;
        private int IntEnable;
        private int SndEnable = 1;

        private uint[] AudioBuffer = new uint[16384];
        private byte[] CassetteBuffer = new byte[8192];

        public CoCo(IModules modules)
        {
            _modules = modules;

            UpdateTapeDialog = offset => { }; //_modules.Config.UpdateTapeDialog((uint)offset);
        }

        public unsafe CoCoState* GetCoCoState()
        {
            return Library.CoCo.GetCoCoState();
        }

        public void CocoReset()
        {
            HorzInterruptEnabled = 0;
            VertInterruptEnabled = 0;
            TimerInterruptEnabled = 0;
            TimerClockRate = 0;
            MasterTickCounter = 0;
            UnxlatedTickCounter = 0;
            OldMaster = 0;

            SoundInterrupt = 0;
            PicosToSoundSample = 0;
            CycleDrift = 0;
            CyclesThisLine = 0;
            PicosThisLine = 0;
            IntEnable = 0;
            AudioIndex = 0;
        }

        public void SetClockSpeed(ushort cycles)
        {
            unsafe
            {
                GetCoCoState()->OverClock = cycles;
            }
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
            _modules.Graphics.SetBlinkState(BlinkPhase);

            _modules.MC6821.MC6821_irq_fs(PhaseStates.Falling);   //FS low to High transition start of display Blink needs this

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
            for (short counter = 0; counter < TopBorder - 4; counter++)
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
            for (short counter = 0; counter < LinesperScreen; counter++)
            {
                _modules.Emu.LineCounter = counter;
                CpuCycle();

                if (_modules.Emu.FrameCounter % _modules.Emu.FrameSkip == Define.FALSE)
                {
                    CoCoUpdateScreen(emuState);
                }
            }

            _modules.MC6821.MC6821_irq_fs(PhaseStates.Rising);  //End of active display FS goes High to Low

            if (VertInterruptEnabled != Define.FALSE)
            {
                _modules.TC1014.GimeAssertVertInterrupt();
            }

            //for (_modules.Emu.LineCounter = 0; _modules.Emu.LineCounter < (cocoState->BottomBorder); _modules.Emu.LineCounter++)
            //Bottom border
            for (short counter = 0; counter < BottomBorder; counter++)
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
                switch (SoundOutputMode)
                {
                    case 0:
                        fixed (uint* buffer = AudioBuffer)
                        {
                            _modules.Audio.FlushAudioBuffer(buffer, (ushort)(AudioIndex << 2));
                        }
                        break;

                    case 1:
                        fixed (byte* buffer = CassetteBuffer)
                        {
                            FlushCassetteBuffer(buffer, AudioIndex);
                        }

                        break;

                    case 2:
                        LoadCassetteBuffer();
                        break;
                }

                AudioIndex = 0;
            }
        }


        #endregion

        #region CPUCycle

        private /* _inline */ void CpuCycle()
        {
            if (HorzInterruptEnabled == Define.TRUE)
            {
                _modules.TC1014.GimeAssertHorzInterrupt();
            }

            _modules.MC6821.MC6821_irq_hs(PhaseStates.Any);
            _modules.PAKInterface.PakTimer();

            PicosThisLine += PicosPerLine;

            while (PicosThisLine > 1)
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
            if (Throttle == 0)
            {
                Throttle = vcc.Throttle;

                if (Throttle == 0)
                {
                    Throttle = 2; // 2 = No throttle.
                }
            }

            vcc.Throttle = 0;

            if (ClipCycle == 1)
            {
                key = _modules.Clipboard.PeekClipboard();

                if (key == shift)
                {
                    _modules.Keyboard.KeyboardHandleKeyDown(shift, shift);  //Press shift and...
                    _modules.Clipboard.PopClipboard();
                    key = _modules.Clipboard.PeekClipboard();
                }

                _modules.Keyboard.KeyboardHandleKeyDown((byte)key, (byte)key);

                WaitCycle = key == 0x1c ? 6000 : 2000;
            }
            else if (ClipCycle == 500)
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
                    vcc.Throttle = Throttle == 2 ? (byte)0 : (byte)1;

                    //...and reset the keymap to the original state
                    ResetKeyMap();

                    Throttle = 0;
                }
            }

            ClipCycle++;

            if (ClipCycle > WaitCycle)
            {
                ClipCycle = 1;
            }

        }

        private void CpuCyclePicos()
        {
            StateSwitch = 0;

            //Does this iteration need to Timer Interrupt
            if ((PicosToInterrupt <= PicosThisLine) && IntEnable == Define.TRUE)
            {
                StateSwitch = 1;
            }

            //Does it need to collect an Audio sample
            if ((PicosToSoundSample <= PicosThisLine) && SndEnable == Define.TRUE)
            {
                StateSwitch += 2;
            }

            var cases = new Dictionary<uint, Action>
            {
                {0, CpuCyclePicosCase0 },
                {1, CpuCyclePicosCase1 },
                {2, CpuCyclePicosCase2 },
                {3, CpuCyclePicosCase3 }
            };

            cases[StateSwitch]();
        }

        //No interrupts this line
        private void CpuCyclePicosCase0()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                CyclesThisLine = CycleDrift + (PicosThisLine * CyclesPerLine * cocoState->OverClock / PicosPerLine);

                if (CyclesThisLine >= 1)
                {   //Avoid un-needed CPU engine calls
                    CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) + (CyclesThisLine - Math.Floor(CyclesThisLine));
                }
                else
                {
                    CycleDrift = CyclesThisLine;
                }

                PicosToInterrupt -= PicosThisLine;
                PicosToSoundSample -= PicosThisLine;
                PicosThisLine = 0;
            }
        }

        //Only Interrupting
        private void CpuCyclePicosCase1()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                PicosThisLine -= PicosToInterrupt;
                CyclesThisLine = CycleDrift + (PicosToInterrupt * CyclesPerLine * cocoState->OverClock / PicosPerLine);

                if (CyclesThisLine >= 1)
                {
                    CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) + (CyclesThisLine - Math.Floor(CyclesThisLine));
                }
                else
                {
                    CycleDrift = CyclesThisLine;
                }

                _modules.TC1014.GimeAssertTimerInterrupt();

                PicosToSoundSample -= PicosToInterrupt;
                PicosToInterrupt = MasterTickCounter;
            }
        }

        //Only Sampling
        private void CpuCyclePicosCase2()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                PicosThisLine -= PicosToSoundSample;
                CyclesThisLine = CycleDrift + (PicosToSoundSample * CyclesPerLine * cocoState->OverClock / PicosPerLine);

                if (CyclesThisLine >= 1)
                {
                    CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) + (CyclesThisLine - Math.Floor(CyclesThisLine));
                }
                else
                {
                    CycleDrift = CyclesThisLine;
                }

                _audioEvent();

                PicosToInterrupt -= PicosToSoundSample;
                PicosToSoundSample = SoundInterrupt;
            }
        }

        //Interrupting and Sampling
        private void CpuCyclePicosCase3()
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                if (PicosToSoundSample < PicosToInterrupt)
                {
                    PicosThisLine -= PicosToSoundSample;
                    CyclesThisLine = CycleDrift + (PicosToSoundSample *
                        CyclesPerLine * instance->OverClock / PicosPerLine);

                    if (CyclesThisLine >= 1)
                    {
                        CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) +
                                               (CyclesThisLine - Math.Floor(CyclesThisLine));
                    }
                    else
                    {
                        CycleDrift = CyclesThisLine;
                    }

                    _audioEvent();

                    PicosToInterrupt -= PicosToSoundSample;
                    PicosToSoundSample = SoundInterrupt;
                    PicosThisLine -= PicosToInterrupt;

                    CyclesThisLine = CycleDrift + (PicosToInterrupt *
                        CyclesPerLine * instance->OverClock / PicosPerLine);

                    if (CyclesThisLine >= 1)
                    {
                        CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) +
                                               (CyclesThisLine - Math.Floor(CyclesThisLine));
                    }
                    else
                    {
                        CycleDrift = CyclesThisLine;
                    }

                    _modules.TC1014.GimeAssertTimerInterrupt();

                    PicosToSoundSample -= PicosToInterrupt;
                    PicosToInterrupt = MasterTickCounter;

                    return;
                }

                if (PicosToSoundSample > PicosToInterrupt)
                {
                    PicosThisLine -= PicosToInterrupt;
                    CyclesThisLine = CycleDrift + (PicosToInterrupt *
                        CyclesPerLine * instance->OverClock / PicosPerLine);

                    if (CyclesThisLine >= 1)
                    {
                        CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) +
                                               (CyclesThisLine - Math.Floor(CyclesThisLine));
                    }
                    else
                    {
                        CycleDrift = CyclesThisLine;
                    }

                    _modules.TC1014.GimeAssertTimerInterrupt();

                    PicosToSoundSample -= PicosToInterrupt;
                    PicosToInterrupt = MasterTickCounter;
                    PicosThisLine -= PicosToSoundSample;
                    CyclesThisLine = CycleDrift + (PicosToSoundSample *
                        CyclesPerLine * instance->OverClock / PicosPerLine);

                    if (CyclesThisLine >= 1)
                    {
                        CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) +
                                               (CyclesThisLine - Math.Floor(CyclesThisLine));
                    }
                    else
                    {
                        CycleDrift = CyclesThisLine;
                    }

                    _audioEvent();

                    PicosToInterrupt -= PicosToSoundSample;
                    PicosToSoundSample = SoundInterrupt;

                    return;
                }

                //They are the same (rare)
                PicosThisLine -= PicosToInterrupt;
                CyclesThisLine = CycleDrift + (PicosToSoundSample *
                    CyclesPerLine * instance->OverClock / PicosPerLine);

                if (CyclesThisLine > 1)
                {
                    CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(CyclesThisLine)) +
                                           (CyclesThisLine - Math.Floor(CyclesThisLine));
                }
                else
                {
                    CycleDrift = CyclesThisLine;
                }

                _modules.TC1014.GimeAssertTimerInterrupt();

                _audioEvent();

                PicosToInterrupt = MasterTickCounter;
                PicosToSoundSample = SoundInterrupt;
            }
        }

        #endregion

        private void ResetKeyMap()
        {
            int currentKeyMap = _modules.Clipboard.CurrentKeyMap;

            _modules.Keyboard.KeyboardBuildRuntimeTable((byte)currentKeyMap);
        }

        //0 = Speaker 1= Cassette Out 2=Cassette In
        public byte SetSndOutMode(byte mode)
        {
            unsafe
            {
                //--TODO: Is there a purpose to this variable?
                ushort primarySoundRate = SoundRate;

                switch (mode)
                {
                    case 0:
                        if (_lastMode == 1)
                        {
                            //Send the last bits to be encoded
                            fixed (byte* buffer = CassetteBuffer)
                            {
                                FlushCassetteBuffer(buffer, AudioIndex); /* Cassette.cpp */
                            }
                        }

                        SetAudioEventAudioOut();
                        SetAudioRate(primarySoundRate);

                        break;

                    case 1:
                        SetAudioEventCassetteOut();

                        primarySoundRate = SoundRate;

                        SetAudioRate(Define.TAPEAUDIORATE);

                        break;

                    case 2:
                        SetAudioEventCassetteIn();

                        primarySoundRate = SoundRate;

                        SetAudioRate(Define.TAPEAUDIORATE);

                        break;

                    default: //QUERY
                        return SoundOutputMode;
                }

                if (mode != _lastMode)
                {
                    AudioIndex = 0;	//Reset Buffer on true mode switch
                    _lastMode = mode;
                }

                SoundOutputMode = mode;

                return SoundOutputMode;
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
            UnxlatedTickCounter = (timer & 0xFFF);

            SetMasterTickCounter();
        }

        public void SetTimerClockRate(byte clockRate)
        {
            //1= 279.265nS (1/ColorBurst)
            //0= 63.695uS  (1/60*262)  1 scan line time

            TimerClockRate = (ushort)(clockRate == 0 ? 0 : 1);

            SetMasterTickCounter();
        }

        public void SetVerticalInterruptState(byte state)
        {
            VertInterruptEnabled = (byte)(state == 0 ? 1 : 0);
        }

        public void SetHorizontalInterruptState(byte state)
        {
            HorzInterruptEnabled = (byte)(state == 0 ? 1 : 0);
        }

        public void SetTimerInterruptState(byte state)
        {
            TimerInterruptEnabled = state;
        }

        public void SetMasterTickCounter()
        {
            double[] rate = { Define.PICOSECOND / (Define.TARGETFRAMERATE * Define.LINESPERFIELD), Define.PICOSECOND / Define.COLORBURST };

            if (UnxlatedTickCounter == 0)
            {
                MasterTickCounter = 0;
            }
            else
            {
                MasterTickCounter = (UnxlatedTickCounter + 2) * rate[TimerClockRate];
            }

            if (MasterTickCounter != OldMaster)
            {
                OldMaster = MasterTickCounter;
                PicosToInterrupt = MasterTickCounter;
            }

            IntEnable = MasterTickCounter == 0 ? 0 : 1;
        }

        private unsafe void FlushCassetteBuffer(byte* buffer, uint length)
        {
            uint offset = _modules.Cassette.FlushCassetteBuffer(buffer, length);

            UpdateTapeDialog((int)offset);
        }

        private unsafe void LoadCassetteBuffer()
        {
            fixed (byte* buffer = CassetteBuffer)
            {
                _modules.Cassette.LoadCassetteBuffer(buffer);
            }
        }

        public ushort SetAudioRate(ushort rate)
        {
            CycleDrift = 0;
            AudioIndex = 0;

            if (rate != 0)
            {	//Force Mute or 44100Hz
                rate = 44100;
            }

            if (rate == 0)
            {
                SndEnable = 0;
                SoundInterrupt = 0;
            }
            else
            {
                SndEnable = 1;
                SoundInterrupt = Define.PICOSECOND / rate;
                PicosToSoundSample = SoundInterrupt;
            }

            SoundRate = rate;

            return 0;
        }

        public void SetLinesPerScreen(byte lines)
        {
            lines &= 3;

            unsafe
            {
                GraphicsState* graphicsState = _modules.Graphics.GetGraphicsState();

                LinesperScreen = graphicsState->Lpf[lines];
                TopBorder = graphicsState->VcenterTable[lines];

                //4 lines of top border are un-rendered 244-4=240 rendered scan-lines
                BottomBorder = (byte)(243 - (TopBorder + LinesperScreen));
            }
        }

        public void SetAudioEventAudioOut()
        {
            void AudioOut()
            {
                AudioBuffer[AudioIndex++] = _modules.MC6821.MC6821_GetDACSample();
            }

            _audioEvent = AudioOut;
        }

        public void SetAudioEventCassetteOut()
        {
            void CassetteOut()
            {
                CassetteBuffer[AudioIndex++] = _modules.MC6821.MC6821_GetCasSample();
            }

            _audioEvent = CassetteOut;
        }

        public void SetAudioEventCassetteIn()
        {
            void CassetteIn()
            {
                AudioBuffer[AudioIndex] = _modules.MC6821.MC6821_GetDACSample();

                _modules.MC6821.MC6821_SetCassetteSample(CassetteBuffer[AudioIndex++]);
            }

            _audioEvent = CassetteIn;
        }
    }
}
