using VCCSharp.OpCodes.Tests.Model;

namespace VCCSharp.OpCodes.Tests;

internal partial class OldCpu
{
    private readonly MemoryTester _mem;

    public OldCpu(MemoryTester mem)
    {
        _mem = mem;
        _jumpVectors = JumpVectors();
    }
}
