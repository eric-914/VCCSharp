namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public class MC6809CpuRegisters
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo

        public MC6809CpuRegister pc { get; } = new MC6809CpuRegister();
        public MC6809CpuRegister x { get; } = new MC6809CpuRegister();
        public MC6809CpuRegister y { get; } = new MC6809CpuRegister();
        public MC6809CpuRegister u { get; } = new MC6809CpuRegister();
        public MC6809CpuRegister s { get; } = new MC6809CpuRegister();
        public MC6809CpuRegister dp { get; } = new MC6809CpuRegister();

        public byte ccbits;

        public byte[] cc = new byte[8];
    }
}
