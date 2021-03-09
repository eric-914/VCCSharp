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

            [DllImport(LIBRARY)]
            public static extern unsafe void FlushAudioBuffer(uint* aBuffer, ushort length);
        }

        public static class Cassette
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void FlushCassetteBuffer(byte* buffer, uint length);

            [DllImport(LIBRARY)]
            public static extern unsafe void LoadCassetteBuffer(byte* cassBuffer);
        }

        public static class Clipboard
        {
            [DllImport(LIBRARY)]
            public static extern int ClipboardEmpty();

            [DllImport(LIBRARY)]
            public static extern char PeekClipboard();

            [DllImport(LIBRARY)]
            public static extern void PopClipboard();
        }

        public static class CoCo
        {
            [DllImport(LIBRARY)]
            public static extern unsafe CoCoState* GetCoCoState();

            [DllImport(LIBRARY)]
            public static extern unsafe void CoCoDrawTopBorder(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void CoCoUpdateScreen(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void CoCoDrawBottomBorder(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern void ResetKeyMap();

            [DllImport(LIBRARY)]
            public static extern void ExecuteAudioEvent();
        }   

        public static class Config
        {
            [DllImport(LIBRARY)]
            public static extern unsafe ConfigState* GetConfigState();

            [DllImport(LIBRARY)]
            public static extern unsafe ConfigModel* GetConfigModel();

            [DllImport(LIBRARY)]
            public static extern unsafe JoystickModel* GetLeftJoystick();

            [DllImport(LIBRARY)]
            public static extern unsafe JoystickModel* GetRightJoystick();

            [DllImport(LIBRARY)]
            public static extern unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);

            [DllImport(LIBRARY)]
            public static extern unsafe void WriteIniFile(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void SynchSystemWithConfig(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern int GetPaletteType();

            [DllImport(LIBRARY)]
            public static extern unsafe byte* ExternalBasicImage();
        }

        public static class CPU
        {
            [DllImport(LIBRARY)]
            public static extern void CPUReset();

            [DllImport(LIBRARY)]
            public static extern void CPUInit();

            [DllImport(LIBRARY)]
            public static extern void CPUForcePC(ushort xferAddress);

            [DllImport(LIBRARY)]
            public static extern int CPUExec(int cycle);
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

            [DllImport(LIBRARY)]
            public static extern unsafe byte LockScreen(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void UnlockScreen(EmuState* emuState);
        }

        public static class Graphics
        {
            [DllImport(LIBRARY)]
            public static extern void ResetGraphicsState();

            [DllImport(LIBRARY)]
            public static extern void MakeRGBPalette();

            [DllImport(LIBRARY)]
            public static extern void MakeCMPPalette(int paletteType);

            [DllImport(LIBRARY)]
            public static extern void SetBlinkState(byte state);

            [DllImport(LIBRARY)]
            public static extern void SetBorderChange(byte data);

            [DllImport(LIBRARY)]
            public static extern void SetVidMask(uint mask);
        }

        public static class Keyboard
        {
            [DllImport(LIBRARY)]
            public static extern void vccKeyboardHandleKeyDown(char key, char scanCode);

            [DllImport(LIBRARY)]
            public static extern void vccKeyboardHandleKeyUp(char key, char scanCode);

            [DllImport(LIBRARY)]
            public static extern void SetPaste(int flag);
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

            [DllImport(LIBRARY)]
            public static extern void MC6821_irq_fs(int phase);

            [DllImport(LIBRARY)]
            public static extern void MC6821_irq_hs(int phase);
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

            [DllImport(LIBRARY)]
            public static extern void PakTimer();
        }

        public static class Resource
        {
            [DllImport(LIBRARY)]
            public static extern void ResourceAppTitle(HINSTANCE hResources, byte[] buffer);
        }

        public static class TC1014
        {
            [DllImport(LIBRARY)]
            public static extern unsafe TC1014MmuState* GetTC1014MmuState();

            [DllImport(LIBRARY)]
            public static extern void MC6883Reset();

            [DllImport(LIBRARY)]
            public static extern void MmuReset();

            [DllImport(LIBRARY)]
            public static extern void MemWrite8(byte data, ushort address);

            [DllImport(LIBRARY)]
            public static extern void GimeAssertVertInterrupt();

            [DllImport(LIBRARY)]
            public static extern void GimeAssertHorzInterrupt();

            [DllImport(LIBRARY)]
            public static extern void GimeAssertTimerInterrupt();

            [DllImport(LIBRARY)]
            public static extern unsafe void FreeMemory(byte* target);

            [DllImport(LIBRARY)]
            public static extern unsafe byte* AllocateMemory(uint size);
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

            [DllImport(LIBRARY)]
            public static extern float CalculateFPS();
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState* GetVccState();
        }
    }
}
