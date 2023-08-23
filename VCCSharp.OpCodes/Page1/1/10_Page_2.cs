using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

//PAGE 2
//0x10__
internal class _10_Page_2 : OpCode, IOpCode
{
    internal _10_Page_2(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        throw new NotImplementedException();

        byte opCode = M8[PC++];

        //TODO: Need jump vectors defined here...

        //_jumpVectors0x10[opCode]();

        //TODO: Need cycle count penalty.  Probably from Page 2 opcodes.

        return 0;
    }
}
