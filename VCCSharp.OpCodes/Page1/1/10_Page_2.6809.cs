﻿using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page2;

namespace VCCSharp.OpCodes.Page1;

//PAGE 2
//0x10__
internal class _10_Page_2_6809 : OpCode, IOpCode
{
    private readonly IOpCode[] _jumpVectors0x10 = new Page2Opcodes6809().OpCodes;

    public int Exec()
    {
        byte opCode = M8[PC++];

        return _jumpVectors0x10[opCode].Exec();
    }
}
