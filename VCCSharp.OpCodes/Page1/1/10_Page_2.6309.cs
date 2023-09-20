using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page2;

namespace VCCSharp.OpCodes.Page1;

//PAGE 2
//0x10__
internal class _10_Page_2_6309 : OpCode, IOpCode, IPage2
{
    public IOpCode[] Page2 { get; } = new Page2Opcodes6309().OpCodes;

    public int CycleCount => throw new NotImplementedException();

    public int Exec()
    {
        byte opCode = M8[PC++];

        return Page2[opCode].Exec();
    }
}
