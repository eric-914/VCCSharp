using System.Runtime.InteropServices;
using System.Security;
using VCCSharp.Models;

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
        }
    }
}
