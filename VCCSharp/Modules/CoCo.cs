using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
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
    }

    public class CoCo : ICoCo
    {
        private Action _audioEvent = () => { };

        private readonly IModules _modules;

        private static byte _lastMode;

        public Action<int> UpdateTapeDialog { get; set; }

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
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                cocoState->HorzInterruptEnabled = 0;
                cocoState->VertInterruptEnabled = 0;
                cocoState->TimerInterruptEnabled = 0;
                cocoState->MasterTimer = 0;
                cocoState->TimerClockRate = 0;
                cocoState->MasterTickCounter = 0;
                cocoState->UnxlatedTickCounter = 0;
                cocoState->OldMaster = 0;

                cocoState->SoundInterrupt = 0;
                cocoState->PicosToSoundSample = 0;
                cocoState->CycleDrift = 0;
                cocoState->CyclesThisLine = 0;
                cocoState->PicosThisLine = 0;
                cocoState->IntEnable = 0;
                cocoState->AudioIndex = 0;
            }
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
            CoCoState* cocoState = GetCoCoState();

            _modules.Graphics.SetBlinkState(cocoState->BlinkPhase);

            _modules.MC6821.MC6821_irq_fs(PhaseStates.Falling);   //FS low to High transition start of display Blink needs this

            //TODO: I don't think updating emuState->LineCounter = counter is needed.

            //for (emuState->LineCounter = 0; emuState->LineCounter < 13; emuState->LineCounter++)
            //Vertical Blanking 13 H lines
            for (short counter = 0; counter < 13; counter++)
            {
                emuState->LineCounter = counter;
                CpuCycle();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < 4; emuState->LineCounter++)
            //4 non-Rendered top Border lines
            for (short counter = 0; counter < 4; counter++)
            {
                emuState->LineCounter = counter;
                CpuCycle();
            }

            if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
            {
                if (_modules.DirectDraw.LockScreen(emuState) == Define.TRUE)
                {
                    return 1;
                }
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < cocoState->TopBorder - 4; emuState->LineCounter++)
            for (short counter = 0; counter < cocoState->TopBorder - 4; counter++)
            {
                emuState->LineCounter = counter;
                if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
                {
                    CoCoDrawTopBorder(emuState);
                }

                CpuCycle();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < cocoState->LinesPerScreen; emuState->LineCounter++)
            //Active Display area
            for (short counter = 0; counter < cocoState->LinesperScreen; counter++)
            {
                emuState->LineCounter = counter;
                CpuCycle();

                if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
                {
                    CoCoUpdateScreen(emuState);
                }
            }

            _modules.MC6821.MC6821_irq_fs(PhaseStates.Rising);  //End of active display FS goes High to Low

            if (cocoState->VertInterruptEnabled != Define.FALSE)
            {
                _modules.TC1014.GimeAssertVertInterrupt();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < (cocoState->BottomBorder); emuState->LineCounter++)
            //Bottom border
            for (short counter = 0; counter < cocoState->BottomBorder; counter++)
            {
                emuState->LineCounter = counter;
                CpuCycle();

                if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
                {
                    CoCoDrawBottomBorder(emuState);
                }
            }

            if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
            {
                _modules.DirectDraw.UnlockScreen(emuState);
                _modules.Graphics.SetBorderChange();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < 6; emuState->LineCounter++)
            //Vertical Retrace 6 H lines
            for (short counter = 0; counter < 4; counter++)
            {
                emuState->LineCounter = counter;
                CpuCycle();
            }

            return 0;
        }

        private void RenderAudioFrame()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                switch (cocoState->SoundOutputMode)
                {
                    case 0:
                        _modules.Audio.FlushAudioBuffer(cocoState->AudioBuffer, (ushort)(cocoState->AudioIndex << 2));
                        break;

                    case 1:
                        FlushCassetteBuffer(cocoState->CassBuffer, cocoState->AudioIndex);
                        break;

                    case 2:
                        LoadCassetteBuffer(cocoState);
                        break;
                }

                cocoState->AudioIndex = 0;
            }
        }


        #endregion

        #region CPUCycle

        private /* _inline */ void CpuCycle()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                if (cocoState->HorzInterruptEnabled == Define.TRUE)
                {
                    _modules.TC1014.GimeAssertHorzInterrupt();
                }

                _modules.MC6821.MC6821_irq_hs(PhaseStates.Any);
                _modules.PAKInterface.PakTimer();

                cocoState->PicosThisLine += cocoState->PicosPerLine;

                while (cocoState->PicosThisLine > 1)
                {
                    CpuCyclePicos();
                }

                if (!_modules.Clipboard.ClipboardEmpty())
                {
                    CpuCycleClipboard(_modules.Vcc);
                }
            }
        }

        private unsafe void CpuCycleClipboard(IVcc vcc)
        {
            const byte shift = 0x36;
            char key;

            CoCoState* cocoState = GetCoCoState();

            //Remember the original throttle setting.
            //Set it to off. We need speed for this!
            if (cocoState->Throttle == 0)
            {
                cocoState->Throttle = vcc.Throttle;

                if (cocoState->Throttle == 0)
                {
                    cocoState->Throttle = 2; // 2 = No throttle.
                }
            }

            vcc.Throttle = 0;

            if (cocoState->ClipCycle == 1)
            {
                key = _modules.Clipboard.PeekClipboard();

                if (key == shift)
                {
                    _modules.Keyboard.KeyboardHandleKeyDown(shift, shift);  //Press shift and...
                    _modules.Clipboard.PopClipboard();
                    key = _modules.Clipboard.PeekClipboard();
                }

                _modules.Keyboard.KeyboardHandleKeyDown((byte)key, (byte)key);

                cocoState->WaitCycle = key == 0x1c ? 6000 : 2000;
            }
            else if (cocoState->ClipCycle == 500)
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
                    vcc.Throttle = cocoState->Throttle == 2 ? (byte)0 : (byte)1;

                    //...and reset the keymap to the original state
                    ResetKeyMap();

                    cocoState->Throttle = 0;
                }
            }

            cocoState->ClipCycle++;

            if (cocoState->ClipCycle > cocoState->WaitCycle)
            {
                cocoState->ClipCycle = 1;
            }

        }

        private unsafe void CpuCyclePicos()
        {
            CoCoState* cocoState = GetCoCoState();
            cocoState->StateSwitch = 0;

            //Does this iteration need to Timer Interrupt
            if ((cocoState->PicosToInterrupt <= cocoState->PicosThisLine) && cocoState->IntEnable == Define.TRUE)
            {
                cocoState->StateSwitch = 1;
            }

            //Does it need to collect an Audio sample
            if ((cocoState->PicosToSoundSample <= cocoState->PicosThisLine) && cocoState->SndEnable == Define.TRUE)
            {
                cocoState->StateSwitch += 2;
            }

            var cases = new Dictionary<uint, Action>
            {
                {0, CpuCyclePicosCase0 },
                {1, CpuCyclePicosCase1 },
                {2, CpuCyclePicosCase2 },
                {3, CpuCyclePicosCase3 }
            };

            cases[cocoState->StateSwitch]();
        }

        //No interrupts this line
        private void CpuCyclePicosCase0()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                cocoState->CyclesThisLine = cocoState->CycleDrift + (cocoState->PicosThisLine * cocoState->CyclesPerLine * cocoState->OverClock / cocoState->PicosPerLine);

                if (cocoState->CyclesThisLine >= 1)
                {   //Avoid un-needed CPU engine calls
                    cocoState->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(cocoState->CyclesThisLine)) + (cocoState->CyclesThisLine - Math.Floor(cocoState->CyclesThisLine));
                }
                else
                {
                    cocoState->CycleDrift = cocoState->CyclesThisLine;
                }

                cocoState->PicosToInterrupt -= cocoState->PicosThisLine;
                cocoState->PicosToSoundSample -= cocoState->PicosThisLine;
                cocoState->PicosThisLine = 0;
            }
        }

        //Only Interrupting
        private void CpuCyclePicosCase1()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                cocoState->PicosThisLine -= cocoState->PicosToInterrupt;
                cocoState->CyclesThisLine = cocoState->CycleDrift + (cocoState->PicosToInterrupt * cocoState->CyclesPerLine * cocoState->OverClock / cocoState->PicosPerLine);

                if (cocoState->CyclesThisLine >= 1)
                {
                    cocoState->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(cocoState->CyclesThisLine)) + (cocoState->CyclesThisLine - Math.Floor(cocoState->CyclesThisLine));
                }
                else
                {
                    cocoState->CycleDrift = cocoState->CyclesThisLine;
                }

                _modules.TC1014.GimeAssertTimerInterrupt();

                cocoState->PicosToSoundSample -= cocoState->PicosToInterrupt;
                cocoState->PicosToInterrupt = cocoState->MasterTickCounter;
            }
        }

        //Only Sampling
        private void CpuCyclePicosCase2()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();

                cocoState->PicosThisLine -= cocoState->PicosToSoundSample;
                cocoState->CyclesThisLine = cocoState->CycleDrift + (cocoState->PicosToSoundSample * cocoState->CyclesPerLine * cocoState->OverClock / cocoState->PicosPerLine);

                if (cocoState->CyclesThisLine >= 1)
                {
                    cocoState->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(cocoState->CyclesThisLine)) + (cocoState->CyclesThisLine - Math.Floor(cocoState->CyclesThisLine));
                }
                else
                {
                    cocoState->CycleDrift = cocoState->CyclesThisLine;
                }

                _audioEvent();

                cocoState->PicosToInterrupt -= cocoState->PicosToSoundSample;
                cocoState->PicosToSoundSample = cocoState->SoundInterrupt;
            }
        }

        //Interrupting and Sampling
        private void CpuCyclePicosCase3()
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                if (instance->PicosToSoundSample < instance->PicosToInterrupt)
                {
                    instance->PicosThisLine -= instance->PicosToSoundSample;
                    instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample *
                        instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

                    if (instance->CyclesThisLine >= 1)
                    {
                        instance->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(instance->CyclesThisLine)) +
                                               (instance->CyclesThisLine - Math.Floor(instance->CyclesThisLine));
                    }
                    else
                    {
                        instance->CycleDrift = instance->CyclesThisLine;
                    }

                    _audioEvent();

                    instance->PicosToInterrupt -= instance->PicosToSoundSample;
                    instance->PicosToSoundSample = instance->SoundInterrupt;
                    instance->PicosThisLine -= instance->PicosToInterrupt;

                    instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToInterrupt *
                        instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

                    if (instance->CyclesThisLine >= 1)
                    {
                        instance->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(instance->CyclesThisLine)) +
                                               (instance->CyclesThisLine - Math.Floor(instance->CyclesThisLine));
                    }
                    else
                    {
                        instance->CycleDrift = instance->CyclesThisLine;
                    }

                    _modules.TC1014.GimeAssertTimerInterrupt();

                    instance->PicosToSoundSample -= instance->PicosToInterrupt;
                    instance->PicosToInterrupt = instance->MasterTickCounter;

                    return;
                }

                if (instance->PicosToSoundSample > instance->PicosToInterrupt)
                {
                    instance->PicosThisLine -= instance->PicosToInterrupt;
                    instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToInterrupt *
                        instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

                    if (instance->CyclesThisLine >= 1)
                    {
                        instance->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(instance->CyclesThisLine)) +
                                               (instance->CyclesThisLine - Math.Floor(instance->CyclesThisLine));
                    }
                    else
                    {
                        instance->CycleDrift = instance->CyclesThisLine;
                    }

                    _modules.TC1014.GimeAssertTimerInterrupt();

                    instance->PicosToSoundSample -= instance->PicosToInterrupt;
                    instance->PicosToInterrupt = instance->MasterTickCounter;
                    instance->PicosThisLine -= instance->PicosToSoundSample;
                    instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample *
                        instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

                    if (instance->CyclesThisLine >= 1)
                    {
                        instance->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(instance->CyclesThisLine)) +
                                               (instance->CyclesThisLine - Math.Floor(instance->CyclesThisLine));
                    }
                    else
                    {
                        instance->CycleDrift = instance->CyclesThisLine;
                    }

                    _audioEvent();

                    instance->PicosToInterrupt -= instance->PicosToSoundSample;
                    instance->PicosToSoundSample = instance->SoundInterrupt;

                    return;
                }

                //They are the same (rare)
                instance->PicosThisLine -= instance->PicosToInterrupt;
                instance->CyclesThisLine = instance->CycleDrift + (instance->PicosToSoundSample *
                    instance->CyclesPerLine * instance->OverClock / instance->PicosPerLine);

                if (instance->CyclesThisLine > 1)
                {
                    instance->CycleDrift = _modules.CPU.CPUExec((int)Math.Floor(instance->CyclesThisLine)) +
                                           (instance->CyclesThisLine - Math.Floor(instance->CyclesThisLine));
                }
                else
                {
                    instance->CycleDrift = instance->CyclesThisLine;
                }

                _modules.TC1014.GimeAssertTimerInterrupt();

                _audioEvent();

                instance->PicosToInterrupt = instance->MasterTickCounter;
                instance->PicosToSoundSample = instance->SoundInterrupt;
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
                CoCoState* instance = GetCoCoState();

                //--TODO: Is there a purpose to this variable?
                ushort primarySoundRate = instance->SoundRate;

                switch (mode)
                {
                    case 0:
                        if (_lastMode == 1)
                        {
                            //Send the last bits to be encoded
                            FlushCassetteBuffer(instance->CassBuffer, instance->AudioIndex); /* Cassette.cpp */
                        }

                        SetAudioEventAudioOut();
                        SetAudioRate(primarySoundRate);

                        break;

                    case 1:
                        SetAudioEventCassetteOut();

                        primarySoundRate = instance->SoundRate;

                        SetAudioRate(Define.TAPEAUDIORATE);

                        break;

                    case 2:
                        SetAudioEventCassetteIn();

                        primarySoundRate = instance->SoundRate;

                        SetAudioRate(Define.TAPEAUDIORATE);

                        break;

                    default: //QUERY
                        return instance->SoundOutputMode;
                }

                if (mode != _lastMode)
                {
                    instance->AudioIndex = 0;	//Reset Buffer on true mode switch
                    _lastMode = mode;
                }

                instance->SoundOutputMode = mode;

                return instance->SoundOutputMode;
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

            mapping[(BitDepthStates)emuState->BitDepth]();
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

            mapping[(BitDepthStates)emuState->BitDepth]();
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

            mapping[(BitDepthStates)emuState->BitDepth]();
        }

        public void SetInterruptTimer(ushort timer)
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                instance->UnxlatedTickCounter = (timer & 0xFFF);
            }

            SetMasterTickCounter();
        }

        public void SetTimerClockRate(byte clockRate)
        {
            unsafe
            {
                //1= 279.265nS (1/ColorBurst)
                //0= 63.695uS  (1/60*262)  1 scan line time

                CoCoState* instance = GetCoCoState();

                instance->TimerClockRate = (ushort)(clockRate == 0 ? 0 : 1);
            }

            SetMasterTickCounter();
        }

        public void SetVerticalInterruptState(byte state)
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                instance->VertInterruptEnabled = (byte)(state == 0 ? 1 : 0);
            }
        }

        public void SetHorizontalInterruptState(byte state)
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                instance->HorzInterruptEnabled = (byte)(state == 0 ? 1 : 0);
            }
        }

        public void SetTimerInterruptState(byte state)
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                instance->TimerInterruptEnabled = state;
            }
        }

        public void SetMasterTickCounter()
        {
            double[] rate = { Define.PICOSECOND / (Define.TARGETFRAMERATE * Define.LINESPERFIELD), Define.PICOSECOND / Define.COLORBURST };

            unsafe
            {
                CoCoState* instance = GetCoCoState();

                if (instance->UnxlatedTickCounter == 0)
                {
                    instance->MasterTickCounter = 0;
                }
                else
                {
                    instance->MasterTickCounter = (instance->UnxlatedTickCounter + 2) * rate[instance->TimerClockRate];
                }

                if (instance->MasterTickCounter != instance->OldMaster)
                {
                    instance->OldMaster = instance->MasterTickCounter;
                    instance->PicosToInterrupt = instance->MasterTickCounter;
                }

                instance->IntEnable = instance->MasterTickCounter == 0 ? 0 : 1;
            }
        }

        private unsafe void FlushCassetteBuffer(byte* buffer, uint length)
        {
            uint offset = _modules.Cassette.FlushCassetteBuffer(buffer, length);

            UpdateTapeDialog((int)offset);
        }

        private unsafe void LoadCassetteBuffer(CoCoState* cocoState)
        {
            _modules.Cassette.LoadCassetteBuffer(cocoState->CassBuffer);
        }

        public ushort SetAudioRate(ushort rate)
        {
            unsafe
            {
                CoCoState* instance = GetCoCoState();

                instance->CycleDrift = 0;
                instance->AudioIndex = 0;

                if (rate != 0)
                {	//Force Mute or 44100Hz
                    rate = 44100;
                }

                if (rate == 0)
                {
                    instance->SndEnable = 0;
                    instance->SoundInterrupt = 0;
                }
                else
                {
                    instance->SndEnable = 1;
                    instance->SoundInterrupt = Define.PICOSECOND / rate;
                    instance->PicosToSoundSample = instance->SoundInterrupt;
                }

                instance->SoundRate = rate;

                return 0;
            }
        }

        public void SetLinesPerScreen(byte lines)
        {
            lines &= 3;

            unsafe
            {
                CoCoState* instance = GetCoCoState();
                GraphicsState* graphicsState = _modules.Graphics.GetGraphicsState();

                instance->LinesperScreen = graphicsState->Lpf[lines];
                instance->TopBorder = graphicsState->VcenterTable[lines];

                //4 lines of top border are un-rendered 244-4=240 rendered scan-lines
                instance->BottomBorder = (byte)(243 - (instance->TopBorder + instance->LinesperScreen));
            }
        }

        public void SetAudioEventAudioOut()
        {
            void AudioOut()
            {
                unsafe
                {
                    CoCoState* instance = GetCoCoState();

                    instance->AudioBuffer[instance->AudioIndex++] = _modules.MC6821.MC6821_GetDACSample();
                }
            }

            _audioEvent = AudioOut;
        }

        public void SetAudioEventCassetteOut()
        {
            void CassOut()
            {
                unsafe
                {
                    CoCoState* instance = GetCoCoState();

                    instance->CassBuffer[instance->AudioIndex++] = _modules.MC6821.MC6821_GetCasSample();
                }
            }

            _audioEvent = CassOut;
        }

        public void SetAudioEventCassetteIn()
        {
            void CassIn()
            {
                unsafe
                {
                    CoCoState* instance = GetCoCoState();

                    instance->AudioBuffer[instance->AudioIndex] = _modules.MC6821.MC6821_GetDACSample();

                    _modules.MC6821.MC6821_SetCassetteSample(instance->CassBuffer[instance->AudioIndex++]);
                }

            }

            _audioEvent = CassIn;
        }
    }
}
