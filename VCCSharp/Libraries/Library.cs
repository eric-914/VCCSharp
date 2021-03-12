using System.Runtime.InteropServices;
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
            public static extern byte SetCPUMultiplier(byte multiplier);
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
            public static extern unsafe short GetSoundCardList(SoundCardList* list);

            [DllImport(LIBRARY)]
            public static extern unsafe int SoundInit(HWND hWnd, _GUID* guid, ushort rate);
        }

        public static class Callbacks
        {
            [DllImport(LIBRARY)]
            public static extern void SetDialogAudioBars(HWND hDlg, ushort left, ushort right);
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

            [DllImport(LIBRARY)]
            public static extern void CopyText();

            [DllImport(LIBRARY)]
            public static extern void PasteText();

            [DllImport(LIBRARY)]
            public static extern void PasteBASIC();

            [DllImport(LIBRARY)]
            public static extern void PasteBASICWithNew();
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
            public static extern unsafe void WriteIniFile(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern int GetPaletteType();

            [DllImport(LIBRARY)]
            public static extern unsafe byte* ExternalBasicImage();

            [DllImport(LIBRARY)]
            public static extern unsafe void GetIniFilePath(byte* iniFilePath);

            [DllImport(LIBRARY)]
            public static extern unsafe void ReadIniFile(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern void SetCpuType(byte cpuType);

            [DllImport(LIBRARY)]
            public static extern byte GetSoundCardIndex(string soundCardName);

            [DllImport(LIBRARY)]
            public static extern unsafe void DecreaseOverclockSpeed(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern unsafe void IncreaseOverclockSpeed(EmuState* emuState);
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
            public static extern unsafe float Static(EmuState* emuState);

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
        }

        public static class Events
        {
            [DllImport(LIBRARY)]
            public static extern void LoadIniFile();

            [DllImport(LIBRARY)]
            public static extern void SaveConfig();

            [DllImport(LIBRARY)]
            public static extern void ShowConfiguration();

            [DllImport(LIBRARY)]
            public static extern void ToggleMonitorType();
        }

        public static class Graphics
        {
            [DllImport(LIBRARY)]
            public static extern unsafe GraphicsState* GetGraphicsState();

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

            [DllImport(LIBRARY)]
            public static extern void SetPaletteType();

            [DllImport(LIBRARY)]
            public static extern unsafe byte SetScanLines(EmuState* emuState, byte lines);

            [DllImport(LIBRARY)]
            public static extern byte SetMonitorType(byte type);

            [DllImport(LIBRARY)]
            public static extern void InvalidateBorder();
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

    //void* __cdecl memcpy(
    //_Out_writes_bytes_all_(_Size) void* _Dst,
    //_In_reads_bytes_(_Size)       void const* _Src,
    //_In_                          size_t      _Size
    //);

    //[System.CLSCompliant(false)]
    //public static void MemoryCopy (void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy);
}
