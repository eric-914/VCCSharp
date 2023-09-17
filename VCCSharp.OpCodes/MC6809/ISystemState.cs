using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

internal interface ISystemState
{
    IState State { get; }

    Memory8Bit M8 { get; }
    Memory16Bit M16 { get; }

    MemoryDirect DIRECT { get; }
    MemoryIndexed INDEXED { get; }

    IRegisters8Bit R8 { get; }
    IRegisters16Bit R16 { get; }

    Mode Mode { get; }

    int Cycles { get; set; }
}
