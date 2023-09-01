using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page3;

namespace VCCSharp.OpCodes.Page1;

//PAGE 3
//0x11__
internal class _11_Page_3 : OpCode, IOpCode
{
    private readonly IOpCode[] _jumpVectors0x11;

    internal _11_Page_3(MC6809.IState cpu) : base(cpu) => _jumpVectors0x11 = new Page3Opcodes6809(cpu).OpCodes;
    internal _11_Page_3(HD6309.IState cpu) : base(cpu) => _jumpVectors0x11 = new Page3Opcodes6309(cpu).OpCodes;

    public int Exec()
    {
        byte opCode = M8[PC++];

        return _jumpVectors0x11[opCode].Exec();
    }
}
