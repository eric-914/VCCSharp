namespace VCCSharp.Models
{
    public struct MC6809State
    {
        public byte ccbits;
        public unsafe fixed uint cc[8];
    }
}
