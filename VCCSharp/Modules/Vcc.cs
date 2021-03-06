using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;

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
        private readonly IDirectDraw _directDraw;

        public Vcc(IModules modules)
        {
            _directDraw = modules.DirectDraw;
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
            return Library.Vcc.CreateEventHandle();
        }

        public HANDLE CreateThreadHandle(HANDLE hEvent)
        {
            return Library.Vcc.CreateThreadHandle(hEvent);
        }

        public void CreatePrimaryWindow()
        {
            Library.Vcc.CreatePrimaryWindow();
        }

        public void SetAppTitle(HINSTANCE hResources, string binFileName)
        {
            Library.Vcc.SetAppTitle(hResources, binFileName);
        }
    }
}
