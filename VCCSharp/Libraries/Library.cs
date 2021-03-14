using System.Runtime.InteropServices;
using VCCSharp.Enums;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HANDLE = System.IntPtr;
using HWND = System.IntPtr;

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

            [DllImport(LIBRARY)]
            public static extern void SetCPUMultiplierFlag(byte double_speed);
        }

        public static class Audio
        {
            [DllImport(LIBRARY)]
            public static extern unsafe AudioState* GetAudioState();

            [DllImport(LIBRARY)]
            public static extern int GetFreeBlockCount();

            [DllImport(LIBRARY)]
            public static extern unsafe void HandleSlowAudio(byte* buffer, ushort length);

            [DllImport(LIBRARY)]
            public static extern unsafe int SoundInit(HWND hWnd, _GUID* guid, ushort rate);

            [DllImport(LIBRARY)]
            public static extern void PurgeAuxBuffer();
        }

        public static class Callbacks
        {
            [DllImport(LIBRARY)]
            public static extern void SetDialogAudioBars(HWND hDlg, ushort left, ushort right);

            [DllImport(LIBRARY)]
            public static extern void SetDialogCpuMultiplier(HWND hDlg, byte cpuMultiplier);
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
            public static extern unsafe ClipboardState* GetClipboardState();
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
            public static extern void ExecuteAudioEvent();

            [DllImport(LIBRARY)]
            public static extern ushort SetAudioRate(ushort rate);
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
            public static extern unsafe byte* ExternalBasicImage();
    
            [DllImport(LIBRARY)]
            public static extern byte GetSoundCardIndex(string soundCardName);
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

            [DllImport(LIBRARY)]
            public static extern void CPUAssertInterrupt(byte irq, byte flag);

            [DllImport(LIBRARY)]
            public static extern void CPUDeAssertInterrupt(byte irq);
        }

        public static class DirectDraw
        {
            [DllImport(LIBRARY)]
            public static extern unsafe DirectDrawState* GetDirectDrawState();

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
            public static extern unsafe void Static(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void DoCls(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe byte LockScreen(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void UnlockScreen(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern byte SetAspect(byte forceAspect);
        }

        public static class DirectSound
        {
            [DllImport(LIBRARY)]
            public static extern void DirectSoundStopAndRelease();

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetCurrentPosition(ulong position);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundLock(ulong buffOffset, ushort length,
                void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);

            [DllImport(LIBRARY)]
            public static extern void EnumerateSoundCards();
        }

        public static class Events
        {
            [DllImport(LIBRARY)]
            public static extern HWND CreateConfigurationDialog(HINSTANCE resources, HWND windowHandle);
        }

        public static class Graphics
        {
            [DllImport(LIBRARY)]
            public static extern unsafe GraphicsState* GetGraphicsState();

            [DllImport(LIBRARY)]
            public static extern void MakeRGBPalette(byte index);

            [DllImport(LIBRARY)]
            public static extern void InvalidateBorder();

            [DllImport(LIBRARY)]
            public static extern void SetGimeBorderColor(byte data);

            [DllImport(LIBRARY)]
            public static extern void SetPaletteLookup(byte index, byte r, byte g, byte b);

            [DllImport(LIBRARY)]
            public static extern void SetMonitorTypePalettes(byte monType, byte palIndex);
        }

        public static class Joystick
        {
            [DllImport(LIBRARY)]
            public static extern unsafe JoystickState* GetJoystickState();

            [DllImport(LIBRARY)]
            public static extern short EnumerateJoysticks();

            [DllImport(LIBRARY)]
            public static extern int InitJoyStick(byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern void SetStickNumbers(byte leftStickNumber, byte rightStickNumber);
        }

        public static class Keyboard
        {
            [DllImport(LIBRARY)]
            public static extern unsafe KeyboardState* GetKeyBoardState();
            //--Spelled funny because there's a GetKeyboardState() in User32.dll

            [DllImport(LIBRARY)]
            public static extern void vccKeyboardHandleKey(char key, char scanCode, KeyStates keyState);

            [DllImport(LIBRARY)]
            public static extern void vccKeyboardBuildRuntimeTable(byte keyMapIndex);
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

            [DllImport(LIBRARY)]
            public static extern byte MC6821_SetCartAutoStart(byte autostart);
        }

        public static class PAKInterface
        {
            [DllImport(LIBRARY)]
            public static extern unsafe PakInterfaceState* GetPakInterfaceState();

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
            public static extern int InsertModuleCase0();

            [DllImport(LIBRARY)]
            public static extern unsafe int InsertModuleCase1(EmuState* emuState, byte* modulePath);

            [DllImport(LIBRARY)]
            public static extern unsafe int InsertModuleCase2(EmuState* emuState, byte* modulePath);

            [DllImport(LIBRARY)]
            public static extern int HasHeartBeat();

            [DllImport(LIBRARY)]
            public static extern void InvokeHeartBeat();

            [DllImport(LIBRARY)]
            public static extern void GetCurrentModule(string defaultModule);
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
            public static extern unsafe TC1014RegistersState* GetTC1014RegistersState();

            [DllImport(LIBRARY)]
            public static extern void MC6883Reset();

            [DllImport(LIBRARY)]
            public static extern void MmuReset();

            [DllImport(LIBRARY)]
            public static extern void MemWrite8(byte data, ushort address);

            [DllImport(LIBRARY)]
            public static extern void GimeAssertHorzInterrupt();

            [DllImport(LIBRARY)]
            public static extern void GimeAssertTimerInterrupt();

            [DllImport(LIBRARY)]
            public static extern unsafe void FreeMemory(byte* target);

            [DllImport(LIBRARY)]
            public static extern unsafe byte* AllocateMemory(uint size);

            [DllImport(LIBRARY)]
            public static extern byte MemRead8(ushort address);

            [DllImport(LIBRARY)]
            public static extern ushort GetMem(int address);
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState* GetVccState();
        }
    }

    //void* __cdecl memcpy(
    //_Out_writes_bytes_all_(_Size) void* _Dst,
    //_In_reads_bytes_(_Size)       void const* _Src,
    //_In_                          size_t      _Size
    //);

    //[System.CLSCompliant(false)]
    //public static void MemoryCopy (void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy);
}
