using System;
using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Models;
using VCCSharp.Models.Pak;
using HANDLE = System.IntPtr;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;
using HMODULE = System.IntPtr;

namespace VCCSharp.Libraries
{
    [SuppressUnmanagedCodeSecurity]
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string LIBRARY = "library.dll";

        public static class Callbacks
        {
            [DllImport(LIBRARY)]
            public static extern void RefreshDynamicMenu(HWND hWnd);
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
            public static extern unsafe int DirectSoundLock(ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundUnlock(void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundSetCooperativeLevel(HWND hWnd);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundEnumerateSoundCards(void* fn);

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

        public static class FileOperations
        {
            [DllImport(LIBRARY)]
            public static extern HANDLE FileOpenFile(string filename, long desiredAccess);

            [DllImport(LIBRARY)]
            public static extern HANDLE FileCreateFile(string filename, long desiredAccess);

            [DllImport(LIBRARY)]
            public static extern long FileSetFilePointer(HANDLE handle, long moveMethod, long offset);

            [DllImport(LIBRARY)]
            public static extern unsafe int FileReadFile(HANDLE handle, byte* buffer, ulong size, ulong* moved);

            [DllImport(LIBRARY)]
            public static extern int FileCloseHandle(HANDLE handle);

            [DllImport(LIBRARY)]
            public static extern int FileFlushFileBuffers(HANDLE handle);

            [DllImport(LIBRARY)]
            public static extern unsafe int FileWriteFile(HANDLE handle, byte* buffer, int size);
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

        public static class Joystick
        {
            [DllImport(LIBRARY)]
            public static extern unsafe JoystickState* GetJoystickState();

            [DllImport(LIBRARY)]
            public static extern short EnumerateJoysticks();

            [DllImport(LIBRARY)]
            public static extern int InitJoyStick(byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern void get_pot_value(byte useLeft, byte useRight);
        }

        public static class MenuCallbacks
        {
            [DllImport(LIBRARY)]
            public static extern void SetMenuItem(string menuName, int menuId, int type);

            [DllImport(LIBRARY)]
            public static extern void SetWindowHandle(HWND hWnd);

            [DllImport(LIBRARY)]
            public static extern void RefreshDynamicMenu();

            [DllImport(LIBRARY)]
            public static extern void SetMenuIndex(byte value);

            [DllImport(LIBRARY)]
            public static extern byte GetMenuIndex();
        }

        public static class PAKInterface
        {
            [DllImport(LIBRARY)]
            public static extern unsafe PakInterfaceState* GetPakInterfaceState();

            [DllImport(LIBRARY)]
            public static extern unsafe PakInterfaceDelegates* GetPakInterfaceDelegates();

            [DllImport(LIBRARY)]
            public static extern HINSTANCE PAKLoadLibrary(string modulePath);

            [DllImport(LIBRARY)]
            public static extern void PAKFreeLibrary(HINSTANCE hInstLib);

            [DllImport(LIBRARY)]
            public static extern unsafe void FreeMemory(byte* target);

            [DllImport(LIBRARY)]
            public static extern unsafe void ResetRomBuffer(byte* buffer);

            [DllImport(LIBRARY)]
            public static extern unsafe void* GetFunction(HMODULE hModule, string lpProcName);

            [DllImport(LIBRARY)]
            public static extern int HasModulePortRead();

            [DllImport(LIBRARY)]
            public static extern byte ModulePortRead(byte port);

            [DllImport(LIBRARY)]
            public static extern byte PakMem8Read(ushort address);
        } //--PAKInterface

        public static class TC1014
        {
            [DllImport(LIBRARY)]
            public static extern unsafe TC1014State* GetTC1014State();

            [DllImport(LIBRARY)]
            public static extern byte MemRead8(ushort mmu, ushort mask);

            [DllImport(LIBRARY)]
            public static extern void MemWrite8(byte data, ushort mmu, ushort mask);
        }
    }
}
