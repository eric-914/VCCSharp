namespace VCCSharp.OpCodes.Tests.Model.HD6309;

/// <summary>
/// HD6309 registers to test
/// </summary>
internal class HD6309TestState
{
    public byte CC = Rnd.B();
    public ushort PC = Rnd.W();
    public ushort S = Rnd.W();
    public ushort U = Rnd.W();
    public byte DP = Rnd.B();
    public ushort D = Rnd.W();
    public ushort X = Rnd.W();
    public ushort Y = Rnd.W();

    //public uint Q = Rnd.D();   //Q=W|D
    public ushort W = Rnd.W();
    //public byte E = Rnd.B();   //W=E|F
    //public byte F = Rnd.B();
    public ushort V = Rnd.W();
    public byte MD = Rnd.B();
}
