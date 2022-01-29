namespace VCCSharp.Models.Configuration
{
    public class Accessories
    {
        public string ModulePath { get; set; } = "";

        public MultiPak MultiPak { get; } = new MultiPak();
        public FloppyDisk FloppyDisk { get; } = new FloppyDisk();
        public HardDisk HardDisk { get; } = new HardDisk();
        public SuperIDE SuperIDE { get; } = new SuperIDE();
    }
}
