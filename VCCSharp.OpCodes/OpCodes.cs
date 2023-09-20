using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Page1;

namespace VCCSharp.OpCodes;

internal class OpCodes : IOpCodes, IPage1
{
    private readonly Executor _exec = new();

    public IOpCode[] Page1 { get; }

    internal OpCodes(IOpCode[] page1)
    {
        Page1 = page1;
    }

    public int Exec(byte opcode)
    {
        return _exec.Exec(Page1[opcode]);
    }
}
