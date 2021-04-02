using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Enums;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using HANDLE = System.IntPtr;
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
        }

        public static class Audio
        {
            [DllImport(LIBRARY)]
            public static extern unsafe AudioState* GetAudioState();

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
            public static extern unsafe void RefreshDynamicMenu(EmuState* emuState);
        }

        public static class Cassette
        {
            [DllImport(LIBRARY)]
            public static extern unsafe CassetteState* GetCassetteState();

            [DllImport(LIBRARY)]
            public static extern unsafe void FlushCassetteBuffer(byte* buffer, uint length);
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
            public static extern void ExecuteAudioEvent();

            [DllImport(LIBRARY)]
            public static extern ushort SetAudioRate(ushort rate);

            [DllImport(LIBRARY)]
            public static extern void SetAudioEventAudioOut();

            [DllImport(LIBRARY)]
            public static extern void SetAudioEventCassOut();

            [DllImport(LIBRARY)]
            public static extern void SetAudioEventCassIn();
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

            [DllImport(LIBRARY)]
            public static extern void UpdateTapeDialog(ushort counter, byte tapeMode);

            [DllImport(LIBRARY)]
            public static extern int GetRememberSize();

            [DllImport(LIBRARY)]
            public static extern Point GetIniWindowSize();
        }

        public static class CPU
        {
            [DllImport(LIBRARY)]
            public static extern void CPUAssertInterrupt(byte irq, byte flag);

            [DllImport(LIBRARY)]
            public static extern void SetCPUToHD6309();

            [DllImport(LIBRARY)]
            public static extern void SetCPUToMC6809();
        }

        public static class DirectDraw
        {
            [DllImport(LIBRARY)]
            public static extern unsafe DirectDrawState* GetDirectDrawState();

            [DllImport(LIBRARY)]
            public static extern int CreateDirectDrawWindow(HINSTANCE resources, byte fullscreen);

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

        public static class Events
        {
            [DllImport(LIBRARY)]
            public static extern HWND CreateConfigurationDialog(HINSTANCE resources, HWND windowHandle);

            [DllImport(LIBRARY)]
            public static extern void ProcessMessage(HWND hWnd, uint message, IntPtr wParam, IntPtr lParam);
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
        }

        public static class Graphics
        {
            [DllImport(LIBRARY)]
            public static extern unsafe GraphicsState* GetGraphicsState();

            [DllImport(LIBRARY)]
            public static extern unsafe GraphicsSurfaces* GetGraphicsSurfaces();

            [DllImport(LIBRARY)]
            public static extern unsafe GraphicsColors* GetGraphicsColors();

            [DllImport(LIBRARY)]
            public static extern void SetGimeBorderColor(byte data);

            [DllImport(LIBRARY)]
            public static extern void SetMonitorTypePalettes(byte monType, byte palIndex);
        }

        public static class HD6309
        {
            [DllImport(LIBRARY)]
            public static extern unsafe HD6309State* GetHD6309State();

            [DllImport(LIBRARY)]
            public static extern void HD6309Reset();

            [DllImport(LIBRARY)]
            public static extern void HD6309AssertInterrupt(byte interrupt, byte waiter);

            [DllImport(LIBRARY)]
            public static extern byte HD6309_getcc();

            [DllImport(LIBRARY)]
            public static extern void Page_1(byte opCode);

            [DllImport(LIBRARY)]
            public static extern void Page_2(byte opCode);

            [DllImport(LIBRARY)]
            public static extern void Page_3(byte opCode);
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

        public static class MC6809
        {
            [DllImport(LIBRARY)]
            public static extern unsafe MC6809State * GetMC6809State();

            [DllImport(LIBRARY)]
            public static extern void MC6809Reset();

            [DllImport(LIBRARY)]
            public static extern void MC6809AssertInterrupt(byte interrupt, byte waiter);

            [DllImport(LIBRARY)]
            public static extern void MC6809ExecOpCode(int cycleFor, byte opCode);

            [DllImport(LIBRARY)]
            public static extern byte MC6809_getcc();
        }

        public static class MC6821
        {
            [DllImport(LIBRARY)]
            public static extern unsafe MC6821State* GetMC6821State();

            [DllImport(LIBRARY)]
            public static extern void MC6821_ClosePrintFile();

            [DllImport(LIBRARY)]
            public static extern void MC6821_SetSerialParams(byte textMode);

            [DllImport(LIBRARY)]
            public static extern int MC6821_OpenPrintFile(string filename);
        } //--MC6821

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
        } //--PAKInterface

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
            public static extern void MemWrite8(byte data, ushort address);

            [DllImport(LIBRARY)]
            public static extern void GimeAssertHorzInterrupt();

            [DllImport(LIBRARY)]
            public static extern void GimeAssertTimerInterrupt();

            [DllImport(LIBRARY)]
            public static extern byte MemRead8(ushort address);

            [DllImport(LIBRARY)]
            public static extern ushort MemRead16(ushort addr);

            [DllImport(LIBRARY)]
            public static extern void SetMapType(byte type);

            [DllImport(LIBRARY)]
            public static extern void SetRomMap(byte data);

            [DllImport(LIBRARY)]
            public static extern unsafe void SwitchMasterMode8(EmuState* emuState, byte masterMode, uint start, uint yStride);

            [DllImport(LIBRARY)]
            public static extern unsafe void SwitchMasterMode16(EmuState* emuState, byte masterMode, uint start, uint yStride);

            [DllImport(LIBRARY)]
            public static extern unsafe void SwitchMasterMode32(EmuState* emuState, byte masterMode, uint start, uint yStride);
        }

        public static class Vcc
        {
            [DllImport(LIBRARY)]
            public static extern unsafe VccState* GetVccState();
        }
    }
}
