using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IVcc
    {
        void CheckScreenModeChange();
        void CreatePrimaryWindow();
        void SetAppTitle(string binFileName);
        void EmuLoop();
        string GetExecPath();
        void ApplyConfigurationChanges();

        bool AutoStart { get; set; }
        bool BinaryRunning { get; set; }
        byte RunState { get; set; }
        byte Throttle { get; set; } 
        string CpuName { get; set; }
    }

    public class Vcc : IVcc
    {
        private readonly IModules _modules;

        public bool AutoStart { get; set; } = true;
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
            if (!_modules.DirectDraw.CreateDirectDrawWindow())
            {
                MessageBox.Show("Can't create primary window", "Error");

                Environment.Exit(0);
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

                float fps = Render();

                _modules.PAKInterface.GetModuleStatus();

                int frameSkip = _modules.Emu.FrameSkip;
                string cpuName = CpuName;
                double mhz = _modules.Emu.CpuCurrentSpeed;
                string status = _modules.Emu.StatusLine;

                string statusBarText = $"Skip:{frameSkip} | FPS:{fps:F} | {cpuName} @ {mhz:0.000}Mhz| {status}";

                _modules.DirectDraw.SetStatusBarText(statusBarText);

                if (Throttle == Define.TRUE)
                {
                    //Do nothing until the frame is over returning unused time to OS
                    _modules.Throttle.FrameWait();
                }
            }
        }

        private float Render()
        {
            _modules.Throttle.StartRender();

            float fps = 0;

            var resetActions = new Dictionary<byte, Action>
            {
                {(byte) ResetPendingStates.None, () => { }},

                {(byte) ResetPendingStates.Soft, () => { _modules.Emu.SoftReset(); }},

                {(byte) ResetPendingStates.Hard, () =>
                {
                    _modules.Config.SynchSystemWithConfig();
                    _modules.DirectDraw.DoCls();
                    _modules.Emu.HardReset();

                }},

                {(byte) ResetPendingStates.Cls, () => { _modules.DirectDraw.DoCls();}},

                {(byte) ResetPendingStates.ClsSynch, () =>
                {
                    _modules.Config.SynchSystemWithConfig();
                    _modules.DirectDraw.DoCls();
                }}
            };

            for (int frames = 1; frames <= _modules.Emu.FrameSkip; frames++)
            {
                resetActions[_modules.Emu.ResetPending]();

                _modules.Emu.ResetPending = (byte)ResetPendingStates.None;

                if (_modules.Emu.EmulationRunning)
                {
                    fps += _modules.CoCo.RenderFrame();
                }
                else
                {
                    fps += _modules.DirectDraw.Static();
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
                ConfigModel configModel = _modules.Config.ConfigModel;

                JoystickModel left = _modules.Config.GetLeftJoystick();
                JoystickModel right = _modules.Config.GetRightJoystick();

                _modules.Emu.ResetPending = (byte)ResetPendingStates.ClsSynch;

                //if ((configState->Model->RamSize != configModel->RamSize) || (configState->Model->CpuType != configModel->CpuType)) {
                //emuState->ResetPending = (byte)ResetPendingStates.Hard;
                //}

                string soundCardName = configModel.SoundCardName;
                byte tempSoundCardIndex = _modules.Config.GetSoundCardIndex(soundCardName);
                SoundCardList card = _modules.Config.SoundCards[tempSoundCardIndex];

                _modules.Audio.SoundInit(_modules.Emu.WindowHandle, card.Guid, configModel.AudioRate);

                _modules.Keyboard.KeyboardBuildRuntimeTable(configModel.KeyMapIndex);

                _modules.Joystick.SetStickNumbers(left.DiDevice, right.DiDevice);
            }
        }
    }
}
