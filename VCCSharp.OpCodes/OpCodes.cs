using VCCSharp.OpCodes.Page1;

namespace VCCSharp.OpCodes;

internal class OpCodes : IOpCodes, IPage1
{
    public IOpCode[] Page1 { get; }

    internal OpCodes(IOpCode[] page1)
    {
        Page1 = page1;
    }

    public int Exec(byte opcode)
    {
        IOpCode vector = Page1[opcode];

        return vector.Exec();
    }
}
