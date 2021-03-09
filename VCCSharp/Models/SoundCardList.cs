using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SoundCardList
    {
        public unsafe fixed byte CardName[64];
        public unsafe _GUID* Guid;
    }
}
