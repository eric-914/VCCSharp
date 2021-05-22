using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IVcc
    {
        unsafe VccState* GetVccState();
        void CheckScreenModeChange();
        void CreatePrimaryWindow();
        void SetAppTitle(string binFileName);
        void EmuLoop();
        string GetExecPath();
        void ApplyConfigurationChanges();
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

        public void SetAppTitle(string binFileName)
        {
            string appTitle = _modules.Config.AppTitle;

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

        public string GetExecPath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        public void ApplyConfigurationChanges()
        {
            unsafe
            {
                ConfigState* configState = _modules.Config.GetConfigState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                JoystickModel* left = configState->Model->Left;
                JoystickModel* right = configState->Model->Right;

                emuState->ResetPending = (byte)ResetPendingStates.ClsSynch;

                //if ((configState->Model->RamSize != configModel->RamSize) || (configState->Model->CpuType != configModel->CpuType)) {
                emuState->ResetPending = (byte)ResetPendingStates.Hard;
                //}

                string soundCardName = Converter.ToString(configState->Model->SoundCardName);
                byte tempSoundCardIndex = _modules.Config.GetSoundCardIndex(soundCardName);
                _modules.Audio.SoundInit(emuState->WindowHandle, Lookup(tempSoundCardIndex).Guid, configState->Model->AudioRate);

                _modules.Keyboard.vccKeyboardBuildRuntimeTable(configState->Model->KeyMapIndex);

                _modules.Joystick.SetStickNumbers(left->DiDevice, right->DiDevice);
            }
        }

        private unsafe SoundCardList Lookup(int index)
        {
            ConfigState* configState = _modules.Config.GetConfigState();

            switch (index)
            {
                case 0: return configState->SoundCards._0;
                case 1: return configState->SoundCards._1;
                case 2: return configState->SoundCards._2;
                    //TODO: Fill in the rest.  Or just figure out how to turn it into an array like it should be.
            }

            return default;
        }
    }
}
