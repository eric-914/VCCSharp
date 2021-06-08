using System.Runtime.InteropServices;

namespace VCCSharp.Libraries
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public static class WinmmDLL
    {
        public const string Dll = "Winmm.dll";

        [DllImport(Dll)]
        public static extern ushort timeBeginPeriod(ushort uPeriod);

        [DllImport(Dll)]
        public static extern ushort timeEndPeriod(ushort uPeriod);
    }
}
