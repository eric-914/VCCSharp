namespace VCCSharp.OpCodes.Tests.Model.MC6809;

/// <summary>
/// MC6809 registers to test
/// </summary>
internal class MC6809TestState
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
