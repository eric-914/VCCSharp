using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Page3;

namespace VCCSharp.OpCodes.Page1;

//PAGE 3
//0x11__
internal class _11_Page_3_6809 : OpCode, IOpCode, IPage3
{
    private readonly Executor _exec = new();

    public IOpCode[] Page3 { get; } = new Page3Opcodes6809().OpCodes;

    public int CycleCount => 0;

    public void Exec()
    {
        byte opCode = M8[PC++];

        Cycles = _exec.Exec(Page3[opCode]);
    }
}
