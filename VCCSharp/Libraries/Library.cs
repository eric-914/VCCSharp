using System;
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
            public static extern unsafe DirectSoundState* GetDirectSoundState();

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundInitialize(IntPtr* lpds, _GUID* guid);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundSetCooperativeLevel(IntPtr lpds, HWND hWnd, uint flag);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundSetupFormatDataStructure(DirectSoundState* ds, ushort bitRate);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundSetupSecondaryBuffer(DirectSoundState* ds, uint sndBuffLength, uint flags);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundCreateSoundBuffer(DirectSoundState* ds, IntPtr lpds);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundLock(DirectSoundState* ds, ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundUnlock(DirectSoundState* ds, void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe long DirectSoundGetCurrentPosition(DirectSoundState* ds, ulong* playCursor, ulong* writeCursor);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundSetCurrentPosition(DirectSoundState* ds, ulong position);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundPlay(DirectSoundState* ds);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundStop(DirectSoundState* ds);

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundStopAndRelease(DirectSoundState* ds, IntPtr lpds);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundHasBuffer(DirectSoundState* ds);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundBufferRelease(DirectSoundState* ds);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundInterfaceRelease(IntPtr lpds);

            //....................................................................//

            [DllImport(LIBRARY)]
            public static extern unsafe void DirectSoundEnumerateSoundCards(void* fn);

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
