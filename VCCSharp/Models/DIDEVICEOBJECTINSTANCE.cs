using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit, Size = 316, CharSet = CharSet.Ansi)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public struct DIDEVICEOBJECTINSTANCE
    {
        [FieldOffset(0)]
        public uint dwSize;
        
        [FieldOffset(4)]
        public _GUID guidType;
        
        [FieldOffset(20)]
        public uint dwOfs;
        
        [FieldOffset(24)]
        public uint dwType;
        
        [FieldOffset(28)]
        public uint dwFlags;
        
        [FieldOffset(32)]
        public unsafe fixed byte tszName[260];
        
        [FieldOffset(292)]
        public uint dwFFMaxForce;
        
        [FieldOffset(296)]
        public uint dwFFForceResolution;
        
        [FieldOffset(300)]
        public ushort wCollectionNumber;
        
        [FieldOffset(302)]
        public ushort wDesignatorIndex;
        
        [FieldOffset(304)]
        public ushort wUsagePage;
        
        [FieldOffset(306)]
        public ushort wUsage;
        
        [FieldOffset(308)]
        public uint dwDimension;
        
        [FieldOffset(312)]
        public ushort wExponent;

        [FieldOffset(314)]
        public ushort wReportId;
    }
}
