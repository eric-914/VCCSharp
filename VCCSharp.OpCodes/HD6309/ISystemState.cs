using VCCSharp.OpCodes.Model.Memory;

namespace VCCSharp.OpCodes.HD6309;

internal interface ISystemState : MC6809.ISystemState
{
    new IState State { get; }

    Memory32Bit M32 { get; }

    Exceptions Exceptions { get; }
}
