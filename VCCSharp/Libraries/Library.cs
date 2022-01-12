using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Models;
using VCCSharp.Models.DirectX;
using HRESULT = System.IntPtr;

namespace VCCSharp.Libraries
{
    [SuppressUnmanagedCodeSecurity]
    public static class Library
    {
        // ReSharper disable once InconsistentNaming
        public const string LIBRARY = "library.dll";

        public static class Joystick
        {
            [DllImport(LIBRARY)]
            public static extern DIDATAFORMAT GetDataFormat();

            [DllImport(LIBRARY)]
            public static extern unsafe _GUID* GetRangeGuid();

            [DllImport(LIBRARY)]
            public static extern unsafe DIJOYSTATE2* GetJoystickState();

            [DllImport(LIBRARY)]
            public static extern unsafe HRESULT JoystickPoll(DIJOYSTATE2* state, byte stickNumber);

            [DllImport(LIBRARY)]
            public static extern unsafe ushort StickX(DIJOYSTATE2* state);

            [DllImport(LIBRARY)]
            public static extern unsafe ushort StickY(DIJOYSTATE2* state);

            [DllImport(LIBRARY)]
            public static extern unsafe byte Button(DIJOYSTATE2* state, int index);
        }
    }
}
