using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page2;

namespace VCCSharp.OpCodes.Page1;

//PAGE 2
//0x10__
internal class _10_Page_2 : OpCode, IOpCode
{
    private readonly IOpCode[] _jumpVectors0x10;

    internal _10_Page_2(MC6809.IState cpu) : base(cpu) => _jumpVectors0x10 = new Page2Opcodes6809(cpu).OpCodes;
    internal _10_Page_2(HD6309.IState cpu) : base(cpu) => _jumpVectors0x10 = new Page2Opcodes6309(cpu).OpCodes;

    public int Exec()
    {
        byte opCode = M8[PC++];

        return _jumpVectors0x10[opCode].Exec();
    }
}
