using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page1;
using VCCSharp.OpCodes.Page2;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : ISystemState
{
    IOpCode[] _page1 = new Page1OpCodes6809().OpCodes;
    IOpCode[] _page2 = new Page2Opcodes6809().OpCodes;

    public void Exec(byte opCode)
    {
        IOpCode iop = _page1[opCode];
        OpCode op = (OpCode)iop;

        op.SS = this;
        op.Cycles = 0;

        Cycles = iop.Exec();
    }

    public void Exec2(byte opCode)
    {
        IOpCode iop = _page2[opCode];
        OpCode op = (OpCode)iop;

        op.SS = this;
        op.Cycles = 0;

        Cycles = iop.Exec();
    }
}
