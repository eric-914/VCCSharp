﻿using System.Runtime.InteropServices;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HMODULE = System.IntPtr;
using INT = System.Int32;
using HANDLE = System.IntPtr;

namespace VCCSharp
{
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string LIBRARY = "library.dll";
        public const string KERNEL = "kernel32.dll";

        public static class Emu
        {
            [DllImport(LIBRARY)]
            public static extern unsafe EmuState *GetEmuState();

            [DllImport(LIBRARY)]
            public static extern unsafe void SetEmuState(EmuState *emuState);
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState *GetVccState();

            [DllImport(LIBRARY)]
            public static extern void VccRun();

            [DllImport(LIBRARY)]
            public static extern unsafe INT VccShutdown(EmuState *emuState);

            [DllImport(LIBRARY)]
            public static extern void SetAppTitle(HINSTANCE hResources, string binFileName);

            [DllImport(LIBRARY)]
            public static extern void CreatePrimaryWindow();

            [DllImport(LIBRARY)]
            public static extern void CheckScreenModeChange();

            [DllImport(LIBRARY)]
            public static extern HANDLE CreateEventHandle();

            [DllImport(LIBRARY)]
            public static extern HANDLE CreateThreadHandle(HANDLE hEvent);
        }

        public static class CoCo
        {
            [DllImport(LIBRARY)]
            public static extern void SetClockSpeed(ushort cycles);
        }

        public static class Config
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void InitConfig(EmuState *emu, ref CmdLineArguments cmdLineArgs);
        }

        public static class DirectDraw
        {
            [DllImport(LIBRARY)]
            public static extern bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources);

            [DllImport(LIBRARY)]
            public static extern void ClearScreen();
        }

        public static class MenuCallbacks
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void DynamicMenuCallback(EmuState *emu, string menuName, int menuId, int type);
        }

        public static class QuickLoad
        {
            [DllImport(LIBRARY)]
            public static extern unsafe int QuickStart(EmuState *emu, string binFileName);
        }

        [DllImport(KERNEL)]
        public static extern HMODULE LoadLibrary(string dllToLoad);

        [DllImport(KERNEL)]
        public static extern bool FreeLibrary(HMODULE hModule);

        [DllImport(KERNEL)]
        public static extern uint WaitForSingleObject(HANDLE handle, uint dwMilliseconds);

        [DllImport(KERNEL)]
        public static extern short SetThreadPriority(HANDLE handle, short nPriority);
    }
}
