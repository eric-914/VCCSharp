using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Main;
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
        bool Throttle { get; set; } 
        string CpuName { get; set; }
    }

    public class Vcc : IVcc
    {
        private readonly IModules _modules;
        private readonly IStatus _status;

        public bool AutoStart { get; set; } = true;
        public bool BinaryRunning { get; set; }

        //An IRQ of sorts telling the emulator to pause during Full Screen toggle
        public byte RunState { get; set; } = Define.EMU_RUNSTATE_RUNNING;
        public bool Throttle { get; set; } = false;

        public string CpuName { get; set; } = "(cpu)";
        public string AppName;

        public Vcc(IModules modules, IStatus status)
        {
            _modules = modules;
            _status = status;
        }

        public void CheckScreenModeChange()
        {
            //Need to stop the EMU thread for screen mode change
            //As it holds the Secondary screen buffer open while running
            if (RunState == (byte)EmuRunStates.Waiting)
            {
                _modules.Draw.FullScreenToggle();

                RunState = (byte)EmuRunStates.Running;
            }
        }

        public void CreatePrimaryWindow()
        {
            if (!_modules.Draw.CreateDirectDrawWindow())
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

                _status.Fps = fps;
                _status.FrameSkip = _modules.Emu.FrameSkip;
                _status.CpuName = CpuName;
                _status.Mhz = _modules.Emu.CpuCurrentSpeed;
                _status.Status = _modules.Emu.StatusLine;

                if (Throttle)
                {
                    //Do nothing until the frame is over returning unused time to OS
                    _modules.Throttle.FrameWait();
                }
            }

            _modules.Events.Shutdown();
        }

        private float Render()
        {
            _modules.Throttle.StartRender();

            float fps = 0;

            var resetActions = new Dictionary<ResetPendingStates, Action>
            {
                {ResetPendingStates.None, () => { }},

                {ResetPendingStates.Soft, () => { _modules.Emu.SoftReset(); }},

                {ResetPendingStates.Hard, () =>
                {
                    _modules.Config.SynchSystemWithConfig();
                    _modules.Draw.DoCls();
                    _modules.Emu.HardReset();
                }},

                {ResetPendingStates.Cls, () => { _modules.Draw.DoCls();}},

                {ResetPendingStates.ClsSynch, () =>
                {
                    _modules.Config.SynchSystemWithConfig();
                    _modules.Draw.DoCls();
                }}
            };

            for (int frames = 1; frames <= _modules.Emu.FrameSkip; frames++)
            {
                resetActions[_modules.Emu.ResetPending]();

                _modules.Emu.ResetPending = (byte)ResetPendingStates.None;

                fps += _modules.Emu.EmulationRunning 
                    ? _modules.CoCo.RenderFrame() 
                    : _modules.Draw.Static();
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
            var configuration = _modules.Configuration;

            _modules.Emu.ResetPending = ResetPendingStates.ClsSynch;

            //if ((configState->Model->RamSize != configModel->RamSize) || (configState->Model->CpuType != configModel->CpuType)) {
            //emuState->ResetPending = (byte)ResetPendingStates.Hard;
            //}

            string audioDevice = configuration.Audio.Device;
            int audioDeviceIndex = _modules.Config.SoundDevices.IndexOf(audioDevice);

            string leftDevice = configuration.Joysticks.Left.Device;
            int leftDeviceIndex = _modules.Config.JoystickDevices.IndexOf(leftDevice);

            string rightDevice = configuration.Joysticks.Right.Device;
            int rightDeviceIndex = _modules.Config.JoystickDevices.IndexOf(rightDevice);

            _modules.Audio.SoundInit(_modules.Emu.WindowHandle, audioDeviceIndex, configuration.Audio.Rate.Value);

            _modules.Keyboard.KeyboardBuildRuntimeTable(configuration.Keyboard.Layout.Value);

            _modules.Joystick.SetStickNumbers(leftDeviceIndex, rightDeviceIndex);
        }
    }
}
