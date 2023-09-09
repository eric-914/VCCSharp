using VCCSharp.OpCodes.HD6309;
using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page1;
using VCCSharp.OpCodes.Page2;
using VCCSharp.OpCodes.Page3;

namespace VCCSharp.OpCodes.Tests.Model.HD6309.N;

internal partial class NewOpcodes : ISystemState
{
    IOpCode[] _page1 = new Page1OpCodes6309().OpCodes;
    IOpCode[] _page2 = new Page2Opcodes6309().OpCodes;
    IOpCode[] _page3 = new Page3Opcodes6309().OpCodes;

    private IOpCode GetOpCode(IOpCode[] page, byte opCode)
    {
        IOpCode iop = page[opCode];

        if (iop is OpCode6309)
        {
            OpCode6309 op = (OpCode6309)iop;

            op.SS = this;
            op.Cycles = 0;
        }
        else
        {
            OpCode op = (OpCode)iop;

            op.SS = this;
            op.Cycles = 0;
        }

        return iop;
    }

    public void Exec(byte opCode)
    {
        IOpCode iop = GetOpCode(_page1, opCode);

        Cycles = iop.Exec();
    }

    public void Exec2(byte opCode)
    {
        IOpCode iop = GetOpCode(_page2, opCode);

        Cycles = iop.Exec();
    }

    public void Exec3(byte opCode)
    {
        IOpCode iop = GetOpCode(_page3, opCode);

        Cycles = iop.Exec();
    }
}
