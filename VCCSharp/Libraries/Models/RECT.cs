namespace VCCSharp.Libraries.Models
{
    // ReSharper disable InconsistentNaming
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString() => $"RECT(left={left},top={top},right={right},bottom={bottom})";
    }
    // ReSharper restore InconsistentNaming
}
