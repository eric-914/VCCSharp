using VCCSharp.OpCodes.HD6309;

namespace VCCSharp.OpCodes.Tests.Model.HD6309.N;

internal partial class NewOpcodes : ISystemState
{
    private readonly MemoryTester _mem;

    public NewOpcodes(MemoryTester mem)
    {
        _mem = mem;
        Exceptions = new() { SS = this };
    }
}
