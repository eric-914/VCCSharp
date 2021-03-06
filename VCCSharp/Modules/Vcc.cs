using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;
using static System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IVcc
    {
        unsafe VccState* GetVccState();
        void CheckScreenModeChange();
        HANDLE CreateEventHandle();
        HANDLE CreateThreadHandle(HANDLE hEvent);
        void CreatePrimaryWindow();
        void SetAppTitle(HINSTANCE hResources, string binFileName);
    }

    public class Vcc : IVcc
    {
        private readonly IEmu _emu;
        private readonly IDirectDraw _directDraw;
        private readonly IResource _resource;

        private readonly IKernel _kernel;


        public Vcc(IModules modules, IKernel kernel)
        {
            _emu = modules.Emu;
            _directDraw = modules.DirectDraw;
            _resource = modules.Resource;

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

        public HANDLE CreateEventHandle()
        {
            HANDLE hEvent = _kernel.CreateEventA(Define.FALSE, Define.FALSE, null);

            if (hEvent == Zero)
            {
                MessageBox.Show("Can't create event thread!!", "Error");

                System.Environment.Exit(0);
            }

            return hEvent;
        }

        public HANDLE CreateThreadHandle(HANDLE hEvent)
        {
            //Task.Run(Library.Vcc.EmuLoop);
            
            HANDLE hThread = Library.Vcc.CreateThreadHandle(hEvent);

            if (hThread == Zero)
            {
                MessageBox.Show("Can't Start main Emulation Thread!", "Ok");

                System.Environment.Exit(0);
            }

            return hThread;
        }

        public void CreatePrimaryWindow()
        {
            unsafe
            {
                EmuState* emuState = _emu.GetEmuState();

                if (!_directDraw.CreateDirectDrawWindow(emuState))
                {
                    MessageBox.Show("Can't create primary window", "Error");

                    System.Environment.Exit(0);
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
    }
}
