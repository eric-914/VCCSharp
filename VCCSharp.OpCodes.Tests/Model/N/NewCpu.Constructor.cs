using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Tests.Model;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : ISystemState
{
    private readonly MemoryTester _mem;

    public NewCpu(MemoryTester mem)
    {
        _mem = mem;
    }
}
