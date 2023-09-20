using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Page2;

namespace VCCSharp.OpCodes.Page1;

//PAGE 2
//0x10__
internal class _10_Page_2_6309 : OpCode, IOpCode, IPage2
{
    private readonly Executor _exec = new();

    public IOpCode[] Page2 { get; } = new Page2Opcodes6309().OpCodes;

    public int CycleCount => 0;

    public int Exec()
    {
        byte opCode = M8[PC++];

        return _exec.Exec(Page2[opCode]);
    }
}
