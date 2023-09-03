using VCCSharp.OpCodes.Model;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.HD6309;

internal interface ISystemState
{
    IState cpu { get; }

    Memory8Bit M8 { get; }
    Memory16Bit M16 { get; }
    Memory32Bit M32 { get; }

    MemoryDP DIRECT { get; }
    MemoryIndexed INDEXED { get; }

    IRegisters8Bit R8 { get; }
    IRegisters16Bit R16 { get; }

    Exceptions Exceptions { get; }

    DynamicCycles DynamicCycles { get; }
}
