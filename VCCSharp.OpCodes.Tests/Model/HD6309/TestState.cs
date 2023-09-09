namespace VCCSharp.OpCodes.Tests.Model.HD6309;

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

    //public uint Q = Rnd.D();
    public ushort W = Rnd.W();
    //public byte E = Rnd.B();
    //public byte F = Rnd.B();
    public ushort V = Rnd.W();
    public byte MD = Rnd.B();
}
