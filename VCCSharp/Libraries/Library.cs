using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Models;
using HRESULT = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Libraries
{
    [SuppressUnmanagedCodeSecurity]
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string LIBRARY = "library.dll";

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
            public static extern int DirectSoundSetCooperativeLevel(HWND hWnd, uint flag);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundEnumerateSoundCards(void* fn);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetCurrentPosition(ulong position);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetupFormatDataStructure(ushort bitRate);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetupSecondaryBuffer(uint sndBuffLength, uint flags);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundStopAndRelease();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundPlay();

            [DllImport(LIBRARY)]
            public static extern int DirectSoundStop();

            [DllImport(LIBRARY)]
            public static extern unsafe long DirectSoundGetCurrentPosition(ulong* playCursor, ulong* writeCursor);
        }

        public static class Joystick
        {
            [DllImport(LIBRARY)]
            public static extern short EnumerateJoysticks();

            [DllImport(LIBRARY)]
            public static extern int InitJoyStick(byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern unsafe void* GetPollStick();

            [DllImport(LIBRARY)]
            public static extern unsafe HRESULT JoyStickPoll(void* js, byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern unsafe ushort StickX(void* stick);

            [DllImport(LIBRARY)]
            public static extern unsafe ushort StickY(void* stick);

            [DllImport(LIBRARY)]
            public static extern unsafe byte Button(void* stick, int index);
        }
    }
}
