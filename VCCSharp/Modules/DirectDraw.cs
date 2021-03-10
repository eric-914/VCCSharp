using VCCSharp.Libraries;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IDirectDraw
    {
        bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);
        void ClearScreen();
        void FullScreenToggle();
        unsafe bool CreateDirectDrawWindow(EmuState* emuState);
        unsafe void SetStatusBarText(string textBuffer, EmuState* emuState);
        unsafe float Static(EmuState* emuState);
        unsafe void DoCls(EmuState* emuState);
        unsafe byte LockScreen(EmuState* emuState);
        unsafe void UnlockScreen(EmuState* emuState);
        byte SetAspect(byte forceAspect);
    }

    public class DirectDraw : IDirectDraw
    {
        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources)
        {
            return Library.DirectDraw.InitDirectDraw(hInstance, resources);
        }

        public void ClearScreen()
        {
            Library.DirectDraw.ClearScreen();
        }

        public void FullScreenToggle()
        {
            Library.DirectDraw.FullScreenToggle();
        }

        public unsafe bool CreateDirectDrawWindow(EmuState* emuState)
        {
            return Library.DirectDraw.CreateDirectDrawWindow(emuState) == Define.TRUE;
        }

        public unsafe void SetStatusBarText(string text, EmuState* emuState)
        {
            Library.DirectDraw.SetStatusBarText(text, emuState);
        }

        public unsafe float Static(EmuState* emuState)
        {
            return Library.DirectDraw.Static(emuState);
        }

        public unsafe void DoCls(EmuState* emuState)
        {
            Library.DirectDraw.DoCls(emuState);
        }

        public unsafe byte LockScreen(EmuState* emuState)
        {
            return Library.DirectDraw.LockScreen(emuState);
        }

        public unsafe void UnlockScreen(EmuState* emuState)
        {
            Library.DirectDraw.UnlockScreen(emuState);
        }

        public byte SetAspect(byte forceAspect)
        {
            return Library.DirectDraw.SetAspect(forceAspect);
        }
    }
}
