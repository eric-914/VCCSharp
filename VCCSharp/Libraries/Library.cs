using System;
using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Enums;
using VCCSharp.Models;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Libraries
{
    [SuppressUnmanagedCodeSecurity]
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
            public static extern void SetCPUMultiplierFlag(byte double_speed);

            [DllImport(LIBRARY)]
            public static extern void SetTurboMode(byte data);
        }

        public static class Audio
        {
            [DllImport(LIBRARY)]
            public static extern unsafe AudioState* GetAudioState();
        }

        public static class Callbacks
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void RefreshDynamicMenu(EmuState* emuState);
        }

        public static class Cassette
        {
            [DllImport(LIBRARY)]
            public static extern unsafe CassetteState* GetCassetteState();

            [DllImport(LIBRARY)]
            public static extern unsafe void FlushCassetteBuffer(byte* buffer, uint length);

            [DllImport(LIBRARY)]
            public static extern void CloseTapeFile();

            [DllImport(LIBRARY)]
            public static extern void SyncFileBuffer();

            [DllImport(LIBRARY)]
            public static extern unsafe int MountTape(byte* filename);
        }

        public static class CoCo
        {
            [DllImport(LIBRARY)]
            public static extern unsafe CoCoState* GetCoCoState();

            [DllImport(LIBRARY)]
            public static extern void ExecuteAudioEvent();

            [DllImport(LIBRARY)]
            public static extern ushort SetAudioRate(ushort rate);

            [DllImport(LIBRARY)]
            public static extern void SetAudioEventAudioOut();

            [DllImport(LIBRARY)]
            public static extern void SetAudioEventCassOut();

            [DllImport(LIBRARY)]
            public static extern void SetAudioEventCassIn();

            [DllImport(LIBRARY)]
            public static extern void SetLinesperScreen(byte lines);
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
            public static extern unsafe byte* GetSoundCardNameAtIndex(byte index);
        }

        public static class CPU
        {
            [DllImport(LIBRARY)]
            public static extern void CPUAssertInterrupt(byte irq, byte flag);
        }

        public static class DirectDraw
        {
            [DllImport(LIBRARY)]
            public static extern int UnlockDDBackSurface();

            [DllImport(LIBRARY)]
            public static extern unsafe void GetDDBackSurfaceDC(void** pHdc);

            [DllImport(LIBRARY)]
            public static extern unsafe void ReleaseDDBackSurfaceDC(void* hdc);

            [DllImport(LIBRARY)]
            public static extern int DDSurfaceFlip();

            [DllImport(LIBRARY)]
            public static extern unsafe int DDSurfaceBlt(RECT* rcDest, RECT* rcSrc);

            [DllImport(LIBRARY)]
            public static extern int HasDDBackSurface();

            [DllImport(LIBRARY)]
            public static extern unsafe DDSURFACEDESC* DDSDCreate();

            [DllImport(LIBRARY)]
            public static extern unsafe uint DDSDGetRGBBitCount(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetRGBBitCount(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe uint DDSDGetPitch(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetPitch(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDSDHasSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void* DDSDGetSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe int LockDDBackSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern void DDRelease();

            [DllImport(LIBRARY)]
            public static extern void DDUnregisterClass();

            [DllImport(LIBRARY)]
            public static extern int DDSurfaceSetClipper();

            [DllImport(LIBRARY)]
            public static extern int DDClipperSetHWnd(HWND hWnd);

            [DllImport(LIBRARY)]
            public static extern int DDCreateClipper();

            [DllImport(LIBRARY)]
            public static extern unsafe int DDGetDisplayMode(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDCreateBackSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe int DDCreateSurface(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwCaps(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwWidth(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwHeight(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwFlags(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern int DDSetCooperativeLevel(HWND hWnd, uint value);

            [DllImport(LIBRARY)]
            public static extern int DDCreate();

            [DllImport(LIBRARY)]
            public static extern int DDSetDisplayMode(uint x, uint y, uint depth);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSDSetdwBackBufferCount(DDSURFACEDESC* ddsd, uint value);

            [DllImport(LIBRARY)]
            public static extern unsafe DDSCAPS DDSDGetddsCaps(DDSURFACEDESC* ddsd);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSurfaceGetAttachedSurface(DDSCAPS* ddsCaps);

            [DllImport(LIBRARY)]
            public static extern int HasDDSurface();

            [DllImport(LIBRARY)]
            public static extern int DDSurfaceIsLost();

            [DllImport(LIBRARY)]
            public static extern int DDBackSurfaceIsLost();

            [DllImport(LIBRARY)]
            public static extern void DDSurfaceRestore();

            [DllImport(LIBRARY)]
            public static extern void DDBackSurfaceRestore();

            [DllImport(LIBRARY)]
            public static extern unsafe IDirectDrawPalette* DDCreatePalette(uint caps, PALETTEENTRY* pal);

            [DllImport(LIBRARY)]
            public static extern unsafe void DDSurfaceSetPalette(IDirectDrawPalette* ddPalette);

            [DllImport(LIBRARY)]
            public static extern IntPtr WndProc(IntPtr hWnd, uint msg, long wParam, long lParam);

            [DllImport(LIBRARY)]
            public static extern unsafe int RegisterWcex(HINSTANCE hInstance, void* lpfnWndProc, byte* lpszClassName, byte* lpszMenuName, uint style, void* hIcon, void* hCursor, void* hbrBackground);
        }

        public static class DirectSound
        {
            [DllImport(LIBRARY)]
            public static extern int DirectSoundHasBuffer();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundHasInterface();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundBufferRelease();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundCreateSoundBuffer();

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundInitialize(_GUID* guid);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundInterfaceRelease();

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundLock(ulong buffOffset, ushort length, void** sndPointer1,
                uint* sndLength1, void** sndPointer2, uint* sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2,
                uint sndLength2);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundSetCooperativeLevel(HWND hWnd);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundEnumerateSoundCards();

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetCurrentPosition(ulong position);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetupFormatDataStructure(ushort bitRate);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetupSecondaryBuffer(uint sndBuffLength);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundStopAndRelease();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundPlay();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundStop();

            [DllImport(LIBRARY)]
            public static extern unsafe long DirectSoundGetCurrentPosition(ulong* playCursor, ulong* writeCursor);
        }

        public static class GDI
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void GDIWriteTextOut(void* hdc, ushort x, ushort y, string message);

            [DllImport(LIBRARY)]
            public static extern unsafe void GDISetBkColor(void* hdc, uint color);

            [DllImport(LIBRARY)]
            public static extern unsafe void GDISetTextColor(void* hdc, uint color);

            [DllImport(LIBRARY)]
            public static extern unsafe void GDITextOut(void* hdc, int x, int y, string text, int textLength);

            [DllImport(LIBRARY)]
            public static extern unsafe void* GDIGetBrush();

            [DllImport(LIBRARY)]
            public static extern unsafe void* GDIGetCursor(byte fullscreen);

            [DllImport(LIBRARY)]
            public static extern unsafe void* GDIGetIcon(HINSTANCE resources);

            [DllImport(LIBRARY)]
            public static extern unsafe void GDIGetClientRect(HWND hWnd, RECT* clientSize);

            [DllImport(LIBRARY)]
            public static extern void CreateMainMenuFullScreen(HWND hWnd);

            [DllImport(LIBRARY)]
            public static extern void CreateMainMenuWindowed(HWND hWnd, HINSTANCE resources);
        }

        public static class Graphics
        {
            [DllImport(LIBRARY)]
            public static extern unsafe GraphicsState* GetGraphicsState();

            //[DllImport(LIBRARY)]
            //public static extern unsafe GraphicsSurfaces* GetGraphicsSurfaces();
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
            public static extern ushort get_pot_value(byte pot);

            [DllImport(LIBRARY)]
            public static extern void SetButtonStatus(byte side, byte state);

            [DllImport(LIBRARY)]
            public static extern void SetJoystick(ushort x, ushort y);

            [DllImport(LIBRARY)]
            public static extern byte SetMouseStatus(byte scanCode, byte phase);
        }

        public static class Keyboard
        {
            [DllImport(LIBRARY)]
            public static extern unsafe KeyboardState* GetKeyBoardState();
            //--Spelled funny because there's a GetKeyboardState() in User32.dll

            [DllImport(LIBRARY)]
            public static extern void GimeSetKeyboardInterruptState(byte state);

            [DllImport(LIBRARY)]
            public static extern void SetKeyTranslationsCoCo(KeyTranslationEntry[] value);

            [DllImport(LIBRARY)]
            public static extern void SetKeyTranslationsNatural(KeyTranslationEntry[] value);

            [DllImport(LIBRARY)]
            public static extern void SetKeyTranslationsCompact(KeyTranslationEntry[] value);

            [DllImport(LIBRARY)]
            public static extern void SetKeyTranslationsCustom(KeyTranslationEntry[] value);

            [DllImport(LIBRARY)]
            public static extern unsafe KeyTranslationEntry* GetKeyTranslationsCoCo();

            [DllImport(LIBRARY)]
            public static extern unsafe KeyTranslationEntry* GetKeyTranslationsNatural();

            [DllImport(LIBRARY)]
            public static extern unsafe KeyTranslationEntry* GetKeyTranslationsCompact();

            [DllImport(LIBRARY)]
            public static extern unsafe KeyTranslationEntry* GetKeyTranslationsCustom();

            [DllImport(LIBRARY)]
            public static extern void vccKeyboardClear();

            [DllImport(LIBRARY)]
            public static extern void vccKeyboardSort();

            [DllImport(LIBRARY)]
            public static extern unsafe void vccKeyboardCopyKeyTranslationEntry(KeyTranslationEntry* target, KeyTranslationEntry* source);

            [DllImport(LIBRARY)]
            public static extern unsafe void vccKeyboardCopy(KeyTranslationEntry* keyTransEntry, int index);

            [DllImport(LIBRARY)]
            public static extern void vccKeyboardUpdateRolloverTable();

            [DllImport(LIBRARY)]
            public static extern byte GimeGetKeyboardInterruptState();
        }

        public static class MenuCallbacks
        {
            [DllImport(LIBRARY)]
            public static extern unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, int menuId, int type);

            [DllImport(LIBRARY)]
            public static extern unsafe void DynamicMenuActivated(EmuState* emuState, int menuItem);
        }

        public static class MC6821
        {
            [DllImport(LIBRARY)]
            public static extern unsafe MC6821State* GetMC6821State();

            [DllImport(LIBRARY)]
            public static extern HANDLE MC6821_OpenPrintFile(string filename);
        } //--MC6821

        public static class PAKInterface
        {
            [DllImport(LIBRARY)]
            public static extern unsafe PakInterfaceState* GetPakInterfaceState();

            [DllImport(LIBRARY)]
            public static extern unsafe void UnloadDll(EmuState* emuState);

            [DllImport(LIBRARY)]
            public static extern void ResetBus();

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
            public static extern int HasSetInterruptCallPointer();

            [DllImport(LIBRARY)]
            public static extern void InvokeSetInterruptCallPointer();

            [DllImport(LIBRARY)]
            public static extern int HasModuleReset();

            [DllImport(LIBRARY)]
            public static extern void InvokeModuleReset();

            [DllImport(LIBRARY)]
            public static extern byte PakPortRead(byte port);

            [DllImport(LIBRARY)]
            public static extern void PakPortWrite(byte port, byte data);

            [DllImport(LIBRARY)]
            public static extern int HasModuleStatus();

            [DllImport(LIBRARY)]
            public static extern unsafe void InvokeModuleStatus(byte* statusLine);
        } //--PAKInterface

        public static class TC1014
        {
            [DllImport(LIBRARY)]
            public static extern unsafe TC1014MmuState* GetTC1014MmuState();

            [DllImport(LIBRARY)]
            public static extern unsafe TC1014RegistersState* GetTC1014RegistersState();

            [DllImport(LIBRARY)]
            public static extern byte MemRead8(ushort address);

            [DllImport(LIBRARY)]
            public static extern void MemWrite8(byte data, ushort address);

            [DllImport(LIBRARY)]
            public static extern void GimeAssertKeyboardInterrupt();
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState* GetVccState();
        }
    }
}
