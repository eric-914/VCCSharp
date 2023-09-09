using VCCSharp.OpCodes.MC6809;

namespace VCCSharp.OpCodes.Tests.Model.MC6809.N;

internal partial class NewOpcodes : ISystemState
{
    private readonly MemoryTester _mem;

    public NewOpcodes(MemoryTester mem)
    {
        _mem = mem;
    }
}
