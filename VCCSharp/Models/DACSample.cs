namespace VCCSharp.Models
{
    public class DACSample
    {
        public int OutLeft { get; set; }
        public int OutRight { get; set; }
        public int LastLeft { get; set; }
        public int LastRight { get; set; }

        public uint Sample => (uint)((LastLeft << 16) + (OutRight));
    }
}
