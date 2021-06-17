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
            public static extern unsafe int DirectSoundInitialize(IntPtr* lpds, _GUID* guid);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundSetCooperativeLevel(IntPtr lpds, HWND hWnd, uint flag);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundCreateSoundBuffer(IntPtr lpds, DSBUFFERDESC* dsbd, IntPtr* buffer);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundLock(IntPtr buffer, ulong buffOffset, ushort length, void** sndPointer1, uint* sndLength1, void** sndPointer2, uint* sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe int DirectSoundUnlock(IntPtr buffer, void* sndPointer1, uint sndLength1, void* sndPointer2, uint sndLength2);

            [DllImport(LIBRARY)]
            public static extern unsafe long DirectSoundGetCurrentPosition(IntPtr buffer, ulong* playCursor, ulong* writeCursor);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundSetCurrentPosition(IntPtr buffer, ulong position);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundPlay(IntPtr buffer);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundStop(IntPtr buffer);

            [DllImport(LIBRARY)]
            public static extern void DirectSoundRelease(IntPtr lpds);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundBufferRelease(IntPtr buffer);

            [DllImport(LIBRARY)]
            public static extern int DirectSoundInterfaceRelease(IntPtr lpds);

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
