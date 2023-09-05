using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Page1;

namespace VCCSharp.OpCodes;

internal class OpCodes : IOpCodes, IPage1, ITempAccess
{
    public IOpCode[] Page1 { get; }

    public IExtendedAddressing EA => ((ITempAccess)Page1[0]).EA;

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
