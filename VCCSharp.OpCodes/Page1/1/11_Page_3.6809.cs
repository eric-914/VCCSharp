using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page3;

namespace VCCSharp.OpCodes.Page1;

//PAGE 3
//0x11__
internal class _11_Page_3_6809 : OpCode, IOpCode, IPage3
{
    public IOpCode[] Page3 { get; } = new Page3Opcodes6809().OpCodes;

    public int Exec()
    {
        byte opCode = M8[PC++];

        return Page3[opCode].Exec();
    }
}
