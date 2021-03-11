using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IVcc
    {
        unsafe VccState* GetVccState();
        void CheckScreenModeChange();
        void CreatePrimaryWindow();
        void SetAppTitle(HINSTANCE hResources, string binFileName);
        void EmuLoop();
    }

    public class Vcc : IVcc
    {
        private readonly IModules _modules;
        
        public Vcc(IModules modules)
        {
            _modules = modules;
        }

        public unsafe VccState* GetVccState()
        {
            return Library.Vcc.GetVccState();
        }

        public void CheckScreenModeChange()
        {
            unsafe
            {
                VccState* vccState = GetVccState();

                //Need to stop the EMU thread for screen mode change
                //As it holds the Secondary screen buffer open while running
                if (vccState->RunState == (byte)EmuRunStates.Waiting)
                {
                    _modules.DirectDraw.FullScreenToggle();

                    vccState->RunState = (byte)EmuRunStates.Running;
                }
            }
        }

        public void CreatePrimaryWindow()
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                if (!_modules.DirectDraw.CreateDirectDrawWindow(emuState))
                {
                    MessageBox.Show("Can't create primary window", "Error");

                    Environment.Exit(0);
                }
            }
        }

        public void SetAppTitle(HINSTANCE hResources, string binFileName)
        {
            string appTitle = _modules.Resource.ResourceAppTitle(hResources);

            if (!string.IsNullOrEmpty(binFileName))
            {
                appTitle = $"{binFileName} Running on {appTitle}";
            }

            unsafe
            {
                VccState* vccState = GetVccState();

                Converter.ToByteArray(appTitle, vccState->AppName);
            }
        }

        public void EmuLoop()
        {
            unsafe
            {
                VccState* vccState = GetVccState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                while (vccState->BinaryRunning == Define.TRUE)
                {
                    if (vccState->RunState == (byte)EmuRunStates.ReqWait)
                    {
                        vccState->RunState = (byte)EmuRunStates.Waiting; //Signal Main thread we are waiting

                        while (vccState->RunState == (byte)EmuRunStates.Waiting)
                        {
                            Thread.Sleep(1);
                        }
                    }

                    float fps = Render(emuState);

                    _modules.PAKInterface.GetModuleStatus(emuState);

                    int frameSkip = emuState->FrameSkip;
                    string cpuName = Converter.ToString(vccState->CpuName);
                    double mhz = emuState->CPUCurrentSpeed;
                    string status = Converter.ToString(emuState->StatusLine);

                    string statusBarText = $"Skip:{frameSkip} | FPS:{fps:F} | {cpuName} @ {mhz:0.000}Mhz| {status}";

                    _modules.DirectDraw.SetStatusBarText(statusBarText, emuState);

                    if (vccState->Throttle == Define.TRUE)
                    {
                        //Do nothing until the frame is over returning unused time to OS
                        _modules.Throttle.FrameWait();
                    }
                }
            }
        }

        private unsafe float Render(EmuState* emuState)
        {
            _modules.Throttle.StartRender();

            float fps = 0;

            var resetActions = new Dictionary<byte, Action>
            {
                {(byte) ResetPendingStates.None, () => { }},

                {(byte) ResetPendingStates.Soft, () => { _modules.Emu.SoftReset(); }},

                {(byte) ResetPendingStates.Hard, () =>
                {
                    _modules.Config.SynchSystemWithConfig(emuState);
                    _modules.DirectDraw.DoCls(emuState);
                    _modules.Emu.HardReset(emuState);

                }},

                {(byte) ResetPendingStates.Cls, () => { _modules.DirectDraw.DoCls(emuState);}},

                {(byte) ResetPendingStates.ClsSynch, () =>
                {
                    _modules.Config.SynchSystemWithConfig(emuState);
                    _modules.DirectDraw.DoCls(emuState);
                }}
            };

            for (int frames = 1; frames <= emuState->FrameSkip; frames++)
            {
                resetActions[emuState->ResetPending]();

                emuState->ResetPending = (byte)ResetPendingStates.None;

                if (emuState->EmulationRunning == Define.TRUE)
                {
                    fps += _modules.CoCo.RenderFrame(emuState);
                }
                else
                {
                    fps += _modules.DirectDraw.Static(emuState);
                }
            }

            _modules.Throttle.EndRender(emuState->FrameSkip);

            return fps;
        }
    }
}
