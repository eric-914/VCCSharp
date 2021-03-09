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
        void SetClockSpeed(ushort cycles);
        unsafe float RenderFrame(EmuState* emuState);
        void CocoReset();
        ushort SetAudioRate(ushort rate);
    }

    public class CoCo : ICoCo
    {
        private readonly IModules _modules;

        public CoCo(IModules modules)
        {
            _modules = modules;
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
                CPUCycle();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < 4; emuState->LineCounter++)
            //4 non-Rendered top Border lines
            for (short counter = 0; counter < 4; counter++)
            {
                emuState->LineCounter = counter;
                CPUCycle();
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

                CPUCycle();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < cocoState->LinesperScreen; emuState->LineCounter++)
            //Active Display area
            for (short counter = 0; counter < cocoState->LinesperScreen; counter++)
            {
                emuState->LineCounter = counter;
                CPUCycle();

                if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
                {
                    CoCoUpdateScreen(emuState);
                }
            }

            _modules.MC6821.MC6821_irq_fs(PhaseStates.Rising);  //End of active display FS goes High to Low

            if (cocoState->VertInterruptEnabled == Define.TRUE)
            {
                _modules.TC1014.GimeAssertVertInterrupt();
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < (cocoState->BottomBorder); emuState->LineCounter++)
            //Bottom border
            for (short counter = 0; counter < cocoState->BottomBorder; counter++)
            {
                emuState->LineCounter = counter;
                CPUCycle();

                if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
                {
                    CoCoDrawBottomBorder(emuState);
                }
            }

            if (emuState->FrameCounter % emuState->FrameSkip == Define.FALSE)
            {
                _modules.DirectDraw.UnlockScreen(emuState);
                _modules.Graphics.SetBorderChange(0);
            }

            //for (emuState->LineCounter = 0; emuState->LineCounter < 6; emuState->LineCounter++)
            //Vertical Retrace 6 H lines
            for (short counter = 0; counter < 4; counter++)
            {
                emuState->LineCounter = counter;
                CPUCycle();
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
                        _modules.Cassette.FlushCassetteBuffer(cocoState->CassBuffer, cocoState->AudioIndex);
                        break;

                    case 2:
                        _modules.Cassette.LoadCassetteBuffer(cocoState->CassBuffer);
                        break;
                }

                cocoState->AudioIndex = 0;
            }
        }

        #endregion

        #region CPUCycle

        private /* _inline */ void CPUCycle()
        {
            unsafe
            {
                CoCoState* cocoState = GetCoCoState();
                VccState* vccState = _modules.Vcc.GetVccState();

                if (cocoState->HorzInterruptEnabled == Define.TRUE)
                {
                    _modules.TC1014.GimeAssertHorzInterrupt();
                }

                _modules.MC6821.MC6821_irq_hs(PhaseStates.Any);
                _modules.PAKInterface.PakTimer();

                cocoState->PicosThisLine += cocoState->PicosPerLine;

                while (cocoState->PicosThisLine > 1)
                {
                    CPUCyclePicos(vccState);
                }

                if (_modules.Clipboard.ClipboardEmpty() == Define.FALSE)
                {
                    CPUCycleClipboard(vccState);
                }
            }
        }

        private unsafe void CPUCycleClipboard(VccState* vccState)
        {
            const char SHIFT = (char)0x36;
            char key;

            CoCoState* cocoState = GetCoCoState();

            //Remember the original throttle setting.
            //Set it to off. We need speed for this!
            if (cocoState->Throttle == 0)
            {
                cocoState->Throttle = vccState->Throttle;

                if (cocoState->Throttle == 0)
                {
                    cocoState->Throttle = 2; // 2 = No throttle.
                }
            }

            vccState->Throttle = 0;

            if (cocoState->ClipCycle == 1)
            {
                key = _modules.Clipboard.PeekClipboard();

                if (key == SHIFT)
                {
                    _modules.Keyboard.vccKeyboardHandleKeyDown(SHIFT, SHIFT);  //Press shift and...
                    _modules.Clipboard.PopClipboard();
                    key = _modules.Clipboard.PeekClipboard();
                }

                _modules.Keyboard.vccKeyboardHandleKeyDown(key, key);

                cocoState->WaitCycle = key == 0x1c ? 6000 : 2000;
            }
            else if (cocoState->ClipCycle == 500)
            {
                key = _modules.Clipboard.PeekClipboard();

                _modules.Keyboard.vccKeyboardHandleKeyUp(SHIFT, SHIFT);
                _modules.Keyboard.vccKeyboardHandleKeyUp((char)0x42, key); //TODO: What is 0x42?
                _modules.Clipboard.PopClipboard();

                //Finished?
                if (_modules.Clipboard.ClipboardEmpty() == Define.TRUE)
                {
                    _modules.Keyboard.SetPaste(Define.FALSE);

                    //Done pasting. Reset throttle to original state
                    if (cocoState->Throttle == 2)
                    {
                        vccState->Throttle = 0;
                    }
                    else
                    {
                        vccState->Throttle = 1;
                    }

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

        private unsafe void CPUCyclePicos(VccState* vccState)
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
                {0, CPUCyclePicosCase0 },
                {1, CPUCyclePicosCase1 },
                {2, CPUCyclePicosCase2 },
                {3, CPUCyclePicosCase3 }
            };

            cases[cocoState->StateSwitch]();
        }

        //No interrupts this line
        private void CPUCyclePicosCase0()
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
        private void CPUCyclePicosCase1()
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
        private void CPUCyclePicosCase2()
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

                ExecuteAudioEvent();

                cocoState->PicosToInterrupt -= cocoState->PicosToSoundSample;
                cocoState->PicosToSoundSample = cocoState->SoundInterrupt;
            }
        }

        //Interrupting and Sampling
        private void CPUCyclePicosCase3()
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

                    ExecuteAudioEvent();

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

                    ExecuteAudioEvent();

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

                ExecuteAudioEvent();

                instance->PicosToInterrupt = instance->MasterTickCounter;
                instance->PicosToSoundSample = instance->SoundInterrupt;
            }
        }

        #endregion

        private void ExecuteAudioEvent()
        {
            Library.CoCo.ExecuteAudioEvent();
        }

        private unsafe void CoCoDrawTopBorder(EmuState* emuState)
        {
            Library.CoCo.CoCoDrawTopBorder(emuState);
        }

        private unsafe void CoCoUpdateScreen(EmuState* emuState)
        {
            Library.CoCo.CoCoUpdateScreen(emuState);
        }

        private unsafe void CoCoDrawBottomBorder(EmuState* emuState)
        {
            Library.CoCo.CoCoDrawBottomBorder(emuState);
        }

        private void ResetKeyMap()
        {
            Library.CoCo.ResetKeyMap();
        }

        public ushort SetAudioRate(ushort rate)
        {
            return Library.CoCo.SetAudioRate(rate);
        }
    }
}
