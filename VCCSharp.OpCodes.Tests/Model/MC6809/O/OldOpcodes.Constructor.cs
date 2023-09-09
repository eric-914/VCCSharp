namespace VCCSharp.OpCodes.Tests.Model.MC6809.O;

internal partial class OldOpcodes
{
    private readonly MemoryTester _mem;

    public OldOpcodes(MemoryTester mem)
    {
        _mem = mem;
        _page1 = Page1Vectors();
        _page2 = Page2Vectors();
        _page3 = Page3Vectors();
    }
}
