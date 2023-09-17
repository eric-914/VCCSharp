using VCCSharp.Models.CPU.Registers;

namespace VCCSharp.Models.CPU.MC6809.Registers;

// ReSharper disable once InconsistentNaming
internal class MC6809CpuRegisters
{
    internal Register16 pc { get; } = new();
    internal Register16 d { get; } = new();
    internal Register16 x { get; } = new();
    internal Register16 y { get; } = new();
    internal Register16 u { get; } = new();
    internal Register16 s { get; } = new();
    internal Register16 dp { get; } = new();

    internal RegisterCC cc { get; } = new();
}
