using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Page1;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : ISystemState
{
    IOpCode[] _jumpVectors = new Page1OpCodes6809().OpCodes;

    public void Exec(byte opCode)
    {
        var v = _jumpVectors[opCode];
        ((OpCode)v).SS = this;
        v.Exec();
    }
}
