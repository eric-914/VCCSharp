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

        public void SetClockSpeed(ushort cycles)
        {
            unsafe
            {
                GetCoCoState()->OverClock = cycles;
            }
        }

        public unsafe float RenderFrame(EmuState* emuState)
        {
            if (RenderVideoFrame(emuState) == 1)
            {
                return 0;
            }

            RenderAudioFrame();

            return _modules.Throttle.CalculateFPS();
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

        public unsafe byte RenderVideoFrame(EmuState* emuState)
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

        public void RenderAudioFrame()
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

        public /* _inline */ int CPUCycle()
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

            return 0;
        }

        public unsafe void CoCoDrawTopBorder(EmuState* emuState)
        {
            Library.CoCo.CoCoDrawTopBorder(emuState);
        }

        public unsafe void CoCoUpdateScreen(EmuState* emuState)
        {
            Library.CoCo.CoCoUpdateScreen(emuState);
        }

        public unsafe void CoCoDrawBottomBorder(EmuState* emuState)
        {
            Library.CoCo.CoCoDrawBottomBorder(emuState);
        }

        public unsafe void CPUCyclePicos(VccState* vccState)
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

        public unsafe void CPUCycleClipboard(VccState* vccState)
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

        public void ResetKeyMap()
        {
            Library.CoCo.ResetKeyMap();
        }

        public void CPUCyclePicosCase0()
        {
            Library.CoCo.CPUCyclePicosCase0();
        }

        public void CPUCyclePicosCase1()
        {
            Library.CoCo.CPUCyclePicosCase1();
        }

        public void CPUCyclePicosCase2()
        {
            Library.CoCo.CPUCyclePicosCase2();
        }

        public void CPUCyclePicosCase3()
        {
            Library.CoCo.CPUCyclePicosCase3();
        }
    }
}
