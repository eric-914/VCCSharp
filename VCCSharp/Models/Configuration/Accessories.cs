namespace VCCSharp.Models.Configuration
{
    public class Accessories
    {
        public string ModulePath { get; set; } = "";

        public MultiPak MultiPak { get; } = new();
        public FloppyDisk FloppyDisk { get; } = new();
        public HardDisk HardDisk { get; } = new();
        public SuperIDE SuperIDE { get; } = new();
    }
}
