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

        byte AutoStart { get; set; }
        bool BinaryRunning { get; set; }
        byte RunState { get; set; }
        byte Throttle { get; set; } 
        string CpuName { get; set; }
    }

    public class Vcc : IVcc
    {
        private readonly IModules _modules;

        public byte AutoStart { get; set; } = Define.TRUE;
        public bool BinaryRunning { get; set; }

        //An IRQ of sorts telling the emulator to pause during Full Screen toggle
        public byte RunState { get; set; } = Define.EMU_RUNSTATE_RUNNING;
        public byte Throttle { get; set; } = 0;

        public string CpuName { get; set; } = "(cpu)";
        public string AppName;

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
            //Need to stop the EMU thread for screen mode change
            //As it holds the Secondary screen buffer open while running
            if (RunState == (byte)EmuRunStates.Waiting)
            {
                _modules.DirectDraw.FullScreenToggle();

                RunState = (byte)EmuRunStates.Running;
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

            AppName = appTitle;
        }

        public void EmuLoop()
        {
            unsafe
            {
                EmuState* emuState = _modules.Emu.GetEmuState();

                while (BinaryRunning)
                {
                    if (RunState == (byte)EmuRunStates.ReqWait)
                    {
                        RunState = (byte)EmuRunStates.Waiting; //Signal Main thread we are waiting

                        while (RunState == (byte)EmuRunStates.Waiting)
                        {
                            Thread.Sleep(1);
                        }
                    }

                    float fps = Render(emuState);

                    _modules.PAKInterface.GetModuleStatus(emuState);

                    int frameSkip = _modules.Emu.FrameSkip;
                    string cpuName = CpuName;
                    double mhz = _modules.Emu.CpuCurrentSpeed;
                    string status = _modules.Emu.StatusLine;

                    string statusBarText = $"Skip:{frameSkip} | FPS:{fps:F} | {cpuName} @ {mhz:0.000}Mhz| {status}";

                    _modules.DirectDraw.SetStatusBarText(statusBarText, emuState);

                    if (Throttle == Define.TRUE)
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

            for (int frames = 1; frames <= _modules.Emu.FrameSkip; frames++)
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

            _modules.Throttle.EndRender(_modules.Emu.FrameSkip);

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
                ConfigModel* configModel = _modules.Config.GetConfigModel();
                EmuState* emuState = _modules.Emu.GetEmuState();

                JoystickModel* left = _modules.Config.GetLeftJoystick();
                JoystickModel* right = _modules.Config.GetRightJoystick();

                emuState->ResetPending = (byte)ResetPendingStates.ClsSynch;

                //if ((configState->Model->RamSize != configModel->RamSize) || (configState->Model->CpuType != configModel->CpuType)) {
                //emuState->ResetPending = (byte)ResetPendingStates.Hard;
                //}

                string soundCardName = Converter.ToString(configModel->SoundCardName);
                byte tempSoundCardIndex = _modules.Config.GetSoundCardIndex(soundCardName);
                _modules.Audio.SoundInit(emuState->WindowHandle, Lookup(tempSoundCardIndex).Guid, configModel->AudioRate);

                _modules.Keyboard.KeyboardBuildRuntimeTable(configModel->KeyMapIndex);

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
