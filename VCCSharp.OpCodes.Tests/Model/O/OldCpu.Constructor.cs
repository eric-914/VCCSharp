using VCCSharp.OpCodes.Tests.Model;

namespace VCCSharp.OpCodes.Tests;

internal partial class OldCpu
{
    private readonly MemoryTester _mem;

    public OldCpu(MemoryTester mem)
    {
        _mem = mem;
        _page1 = Page1Vectors();
        _page2 = Page2Vectors();
        _page3 = Page3Vectors();
    }
}
