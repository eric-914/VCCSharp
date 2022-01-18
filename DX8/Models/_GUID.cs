using System.Runtime.InteropServices;
using DX8.Converters;

namespace DX8.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
    public struct _GUID
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        public unsafe fixed byte Data4[8];

        public string Guid => GuidConverter.ToString(this);
    }
}
