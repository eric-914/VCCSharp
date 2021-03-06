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
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState* GetVccState();

            [DllImport(LIBRARY)]
            public static extern unsafe void EmuLoop(EmuState* emuState);
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
        }

        public static class Config
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);

            [DllImport(LIBRARY)]
            public static extern unsafe void WriteIniFile(EmuState* emuState);
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
        }

        public static class MenuCallbacks
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, int menuId, int type);
        }

        public static class PAKInterface
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void UnloadDll(EmuState* emuState);
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

        public static class Throttle
        {
            [DllImport(LIBRARY)]
            public static extern void CalibrateThrottle();
        }
    }
}
