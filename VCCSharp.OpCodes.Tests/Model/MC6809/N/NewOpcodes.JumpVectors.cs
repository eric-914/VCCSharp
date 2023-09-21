using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page1;
using VCCSharp.OpCodes.Page2;
using VCCSharp.OpCodes.Page3;

namespace VCCSharp.OpCodes.Tests.Model.MC6809.N;

internal partial class NewOpcodes : ISystemState
{
    IOpCode[] _page1 = new Page1OpCodes6809().OpCodes;
    IOpCode[] _page2 = new Page2Opcodes6809().OpCodes;
    IOpCode[] _page3 = new Page3Opcodes6809().OpCodes;

    private IOpCode GetOpCode(IOpCode[] page, byte opCode)
    {
        IOpCode iop = page[opCode];

        OpCode op = (OpCode)iop;

        op.System = this;
        op.Cycles = 0;

        return iop;
    }

    public void Exec(byte opCode)
    {
        IOpCode iop = GetOpCode(_page1, opCode);

        iop.Cycles = iop.CycleCount;

        iop.Exec();

        Cycles = iop.Cycles;
    }

    public void Exec2(byte opCode)
    {
        IOpCode iop = GetOpCode(_page2, opCode);

        iop.Cycles = iop.CycleCount;

        iop.Exec();

        Cycles = iop.Cycles;
    }

    public void Exec3(byte opCode)
    {
        IOpCode iop = GetOpCode(_page3, opCode);

        iop.Cycles = iop.CycleCount;

        iop.Exec();

        Cycles = iop.Cycles;
    }
}
