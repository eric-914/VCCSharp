using VCCSharp.Models.CPU.Registers;

namespace VCCSharp.Models.CPU.HD6309.Registers;

internal class HD6309CpuRegisters
{
    internal Register16 pc { get; } = new();
    internal Register16 x { get; } = new();
    internal Register16 y { get; } = new();
    internal Register16 u { get; } = new();
    internal Register16 s { get; } = new();
    internal Register16 dp { get; } = new();
    internal Register16 v { get; } = new();
    internal Register16 z { get; } = new();

    internal HD6309WideRegister q { get; } = new();

    internal RegisterCC cc { get; } = new();
    internal RegisterMD md { get; } = new();
}
