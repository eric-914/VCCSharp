using System.Runtime.InteropServices;
using VCCSharp.Models;
using HINSTANCE = System.IntPtr;
using LPUNKNOWN = System.IntPtr;
using LPVOID = System.IntPtr;

namespace VCCSharp.Libraries
{
    public static class DInputDLL
    {
        public const string Dll = "DInput8.dll";

        [DllImport(Dll)]
        public static extern int DirectInput8Create(HINSTANCE handle, uint version, _GUID pId, ref LPVOID di, LPUNKNOWN unknown);
    }
}
