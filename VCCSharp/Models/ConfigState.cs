using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ConfigState
    {
        public unsafe fixed byte IniFilePath[Define.MAX_PATH];
    }
}
