using System.Threading.Tasks;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp
{
    public interface IVccApp
    {
        void Startup(HINSTANCE hInstance, CmdLineArguments cmdLineArgs);
        void Threading();
        void Run();
        int Shutdown();
    }

    public class VccApp : IVccApp
    {
        private readonly IModules _modules;
        private readonly IKernel _kernel;
        private readonly IUser32 _user32;

        private HINSTANCE _hResources;

        public MSG MSG;

        public VccApp(IModules modules, IKernel kernel, IUser32 user32)
        {
            _modules = modules;
            _kernel = kernel;
            _user32 = user32;
        }

        public void Startup(HINSTANCE hInstance, CmdLineArguments cmdLineArgs)
        {
            //AudioState* audioState = _modules.Audio.GetAudioState();
            _modules.CoCo.SetAudioEventAudioOut();

            _hResources = _kernel.LoadLibrary("resources.dll");
                
            _modules.Emu.Resources = _hResources;

            _modules.DirectDraw.InitDirectDraw(hInstance, _hResources);
            _modules.Keyboard.SetKeyTranslations();

            _modules.CoCo.OverClock = 1;  //Default clock speed .89 MHZ	

            _modules.Vcc.CreatePrimaryWindow();

            if (!string.IsNullOrEmpty(cmdLineArgs.QLoadFile))
            {
                if (_modules.QuickLoad.QuickStart(cmdLineArgs.QLoadFile) == (int)QuickStartStatuses.Ok)
                {
                    _modules.Vcc.SetAppTitle(cmdLineArgs.QLoadFile); //TODO: No app title if no quick load
                }

                _modules.Emu.EmulationRunning = true;
            }

            //NOTE: Sound is lost if this isn't done after CreatePrimaryWindow();
            //Loads the default config file Vcc.ini from the exec directory
            _modules.Config.InitConfig(ref cmdLineArgs);

            _modules.DirectDraw.ClearScreen();

            _modules.Emu.ResetPending = (byte)ResetPendingStates.Cls;

            _modules.MenuCallbacks.RefreshCartridgeMenu();

            _modules.Emu.ResetPending = (byte)ResetPendingStates.Hard;

            _modules.Emu.EmulationRunning = _modules.Vcc.AutoStart;

            _modules.Vcc.BinaryRunning = true;

            _modules.Throttle.CalibrateThrottle();
        }

        public void Threading()
        {
            Task.Run(_modules.Vcc.EmuLoop);
        }

        public void Run()
        {
            unsafe
            {
                while (_modules.Vcc.BinaryRunning)
                {
                    _modules.Vcc.CheckScreenModeChange();

                    fixed (MSG* msg = &(MSG))
                    {
                        _user32.GetMessageA(msg, Zero, 0, 0);   //Seems if the main loop stops polling for Messages the child threads stall

                        _user32.TranslateMessage(msg);

                        _user32.DispatchMessageA(msg);
                    }
                }
            }
        }

        public int Shutdown()
        {
            _modules.PAKInterface.UnloadDll(_modules.Emu.EmulationRunning);
            _modules.Audio.SoundDeInit();

            _modules.Config.WriteIniFile(); //Save any changes to ini File

            int code = (int)MSG.wParam;

            _kernel.FreeLibrary(_hResources);

            return code;
        }
    }
}
