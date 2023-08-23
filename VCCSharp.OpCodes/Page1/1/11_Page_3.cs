using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

//PAGE 3
//0x11__
internal class _11_Page_3 : OpCode, IOpCode
{
    internal _11_Page_3(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        throw new NotImplementedException();

        byte opCode = M8[PC++];

        //TODO: Need jump vectors defined here...

        //_jumpVectors0x11[opCode]();

        //TODO: Need cycle count penalty.  Probably from Page 3 opcodes.
        return 0;
    }
}
