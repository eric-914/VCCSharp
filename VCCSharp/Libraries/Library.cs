using System;
using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Models;
using HRESULT = System.IntPtr;

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
