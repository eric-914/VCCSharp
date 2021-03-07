using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct VccState
    {
        public byte AutoStart;
        public byte BinaryRunning;
        public byte DialogOpen;
        public byte RunState;
        public byte Throttle;

        public unsafe fixed byte CpuName[20];
        public unsafe fixed byte AppName[100];

        public MSG msg;
    }
}
