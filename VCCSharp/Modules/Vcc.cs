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
        private readonly IEmu _emu;
        private readonly IDirectDraw _directDraw;
        private readonly IResource _resource;
        private readonly IThrottle _throttle;
        private readonly IPAKInterface _pakInterface;
        private readonly ICoCo _coco;
        private readonly IConfig _config;

        private readonly IKernel _kernel;
        
        public Vcc(IModules modules, IKernel kernel)
        {
            _emu = modules.Emu;
            _directDraw = modules.DirectDraw;
            _resource = modules.Resource;
            _throttle = modules.Throttle;
            _pakInterface = modules.PAKInterface;
            _coco = modules.CoCo;
            _config = modules.Config;

            _kernel = kernel;
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
                    _directDraw.FullScreenToggle();

                    vccState->RunState = (byte)EmuRunStates.Running;
                }
            }
        }

        public void CreatePrimaryWindow()
        {
            unsafe
            {
                EmuState* emuState = _emu.GetEmuState();

                if (!_directDraw.CreateDirectDrawWindow(emuState))
                {
                    MessageBox.Show("Can't create primary window", "Error");

                    Environment.Exit(0);
                }
            }
        }

        public void SetAppTitle(HINSTANCE hResources, string binFileName)
        {
            string appTitle = _resource.ResourceAppTitle(hResources);

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
                EmuState* emuState = _emu.GetEmuState();

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

                    _pakInterface.GetModuleStatus(emuState);

                    int frameSkip = emuState->FrameSkip;
                    string cpuName = Converter.ToString(vccState->CpuName);
                    double mhz = emuState->CPUCurrentSpeed;
                    string status = Converter.ToString(emuState->StatusLine);

                    string statusBarText = $"Skip:{frameSkip} | FPS:{fps:F} | {cpuName} @ {mhz:0.000}Mhz| {status}";

                    _directDraw.SetStatusBarText(statusBarText, emuState);

                    if (vccState->Throttle == Define.TRUE)
                    {
                        //Do nothing until the frame is over returning unused time to OS
                        _throttle.FrameWait();
                    }
                }
            }
        }

        public unsafe float Render(EmuState* emuState)
        {
            _throttle.StartRender();

            float fps = 0;

            var resetActions = new Dictionary<byte, Action>
            {
                {(byte) ResetPendingStates.None, () => { }},

                {(byte) ResetPendingStates.Soft, () => { _emu.SoftReset(); }},

                {(byte) ResetPendingStates.Hard, () =>
                {
                    _config.SynchSystemWithConfig(emuState);
                    _directDraw.DoCls(emuState);
                    _emu.HardReset(emuState);

                }},

                {(byte) ResetPendingStates.Cls, () => { _directDraw.DoCls(emuState);}},

                {(byte) ResetPendingStates.ClsSynch, () =>
                {
                    _config.SynchSystemWithConfig(emuState);
                    _directDraw.DoCls(emuState);
                }}
            };

            for (int frames = 1; frames <= emuState->FrameSkip; frames++)
            {
                resetActions[emuState->ResetPending]();

                emuState->ResetPending = (byte)ResetPendingStates.None;

                if (emuState->EmulationRunning == Define.TRUE)
                {
                    fps += _coco.RenderFrame(emuState);
                }
                else
                {
                    fps += _directDraw.Static(emuState);
                }
            }

            _throttle.EndRender(emuState->FrameSkip);

            return fps;
        }
    }
}
