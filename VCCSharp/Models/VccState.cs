using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct VccState
    {
        public byte BinaryRunning;
        public byte DialogOpen;

        public MSG msg;
    }
}
