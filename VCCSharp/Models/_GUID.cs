namespace VCCSharp.Models
{
    public struct _GUID
    {
        public ulong Data1;
        public ushort Data2;
        public ushort Data3;
        public unsafe fixed byte Data4[8];
    }
}
