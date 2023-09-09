namespace VCCSharp.OpCodes.Tests.Model.MC6809;

internal class TestState
{
    public byte CC = Rnd.B();
    public ushort PC = Rnd.W();
    public ushort S = Rnd.W();
    public ushort U = Rnd.W();
    public byte DP = Rnd.B();
    public ushort D = Rnd.W();
    public ushort X = Rnd.W();
    public ushort Y = Rnd.W();
}
