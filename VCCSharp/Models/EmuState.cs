using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EmuState
    {
        public byte EmulationRunning;
        public byte ResetPending;

        public unsafe fixed byte PakPath[256];
    }
}
