﻿using System.Runtime.InteropServices;
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
            public static extern void SetCPUToHD6309();

            [DllImport(LIBRARY)]
            public static extern void SetCPUToMC6809();
        }

        public static class Audio
        {
            [DllImport(LIBRARY)]
            public static extern short SoundDeInit();

            [DllImport(LIBRARY)]
            public static extern void ResetAudio();
        }

        public static class CoCo
        {
            [DllImport(LIBRARY)]
            public static extern unsafe CoCoState* GetCoCoState();

            [DllImport(LIBRARY)]
            public static extern void SetClockSpeed(ushort cycles);

            [DllImport(LIBRARY)]
            public static extern unsafe float RenderFrame(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern void CocoReset();
        }

        public static class Config
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);

            [DllImport(LIBRARY)]
            public static extern unsafe void WriteIniFile(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void SynchSystemWithConfig(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern int GetPaletteType();
        }

        public static class CPU
        {
            [DllImport(LIBRARY)]
            public static extern void CPUReset();

            [DllImport(LIBRARY)]
            public static extern void CPUInit();

            [DllImport(LIBRARY)]
            public static extern void CPUForcePC(ushort xferAddress);
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

        public static class Graphics
        {
            [DllImport(LIBRARY)]
            public static extern void ResetGraphicsState();

            [DllImport(LIBRARY)]
            public static extern void MakeRGBPalette();

            [DllImport(LIBRARY)]
            public static extern void MakeCMPPalette(int paletteType);
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

            [DllImport(LIBRARY)]
            public static extern void UpdateBusPointer();

            [DllImport(LIBRARY)]
            public static extern unsafe int InsertModule(EmuState* emuState, string modulePath);
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

            [DllImport(LIBRARY)]
            public static extern byte MmuInit(byte ramSizeOption);

            [DllImport(LIBRARY)]
            public static extern void MemWrite8(byte data, ushort address);
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
