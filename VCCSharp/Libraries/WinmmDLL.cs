using System.Runtime.InteropServices;

namespace VCCSharp.Libraries
{
    public static class WinmmDLL
    {
        public const string DLL = "Winmm.dll";

        [DllImport(DLL)]
        public static extern ushort timeBeginPeriod(ushort uPeriod);

        [DllImport(DLL)]
        public static extern ushort timeEndPeriod(ushort uPeriod);
    }
}
