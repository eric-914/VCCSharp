using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page1;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : ISystemState
{
    IOpCode[] _jumpVectors = new Page1OpCodes6809().OpCodes;

    public void Exec(byte opCode)
    {
        IOpCode iop = _jumpVectors[opCode];
        OpCode op = (OpCode)iop;

        op.SS = this;
        op.Cycles = 0;

        Cycles = iop.Exec();
    }
}
