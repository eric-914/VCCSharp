using System.Runtime.InteropServices;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HANDLE = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string LIBRARY = "library.dll";

        public static class Emu
        {
            [DllImport(LIBRARY)]
            public static extern unsafe EmuState* GetEmuState();

            [DllImport(LIBRARY)]
            public static extern unsafe void SetEmuState(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void HardReset(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern void GimeReset();
        }

        public static class Audio
        {
            [DllImport(LIBRARY)]
            public static extern short SoundDeInit();
        }

        public static class CoCo
        {
            [DllImport(LIBRARY)]
            public static extern void SetClockSpeed(ushort cycles);

            [DllImport(LIBRARY)]
            public static extern unsafe float RenderFrame(EmuState* emuState);
        }

        public static class Config
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);

            [DllImport(LIBRARY)]
            public static extern unsafe void WriteIniFile(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void SynchSystemWithConfig(EmuState* emuState);
        }

        public static class CPU
        {
            [DllImport(LIBRARY)]
            public static extern void CPUReset();
        }

        public static class DirectDraw
        {
            [DllImport(LIBRARY)]
            public static extern bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);

            [DllImport(LIBRARY)]
            public static extern void ClearScreen();

            [DllImport(LIBRARY)]
            public static extern void FullScreenToggle();

            [DllImport(LIBRARY)]
            public static extern unsafe int CreateDirectDrawWindow(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void SetStatusBarText(string textBuffer, EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe float Static(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void DoCls(EmuState* emuState);
        }

        public static class MenuCallbacks
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, int menuId, int type);
        }

        public static class MC6821
        {
            [DllImport(LIBRARY)]
            public static extern void MC6821_PiaReset();
        }

        public static class PAKInterface
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void UnloadDll(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void GetModuleStatus(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern void ResetBus();
        }

        public static class QuickLoad
        {
            [DllImport(LIBRARY)]
            public static extern unsafe int QuickStart(EmuState* emuState, string binFileName);
        }

        public static class Resource
        {
            [DllImport(LIBRARY)]
            public static extern void ResourceAppTitle(HINSTANCE hResources, byte[] buffer);
        }

        public static class TC1014
        {
            [DllImport(LIBRARY)]
            public static extern void MC6883Reset();

            [DllImport(LIBRARY)]
            public static extern void CopyRom();

            [DllImport(LIBRARY)]
            public static extern void MmuReset();
        }

        public static class Throttle
        {
            [DllImport(LIBRARY)]
            public static extern void CalibrateThrottle();

            [DllImport(LIBRARY)]
            public static extern void FrameWait();

            [DllImport(LIBRARY)]
            public static extern void StartRender();

            [DllImport(LIBRARY)]
            public static extern void EndRender(byte skip);
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState* GetVccState();
        }
    }
}
