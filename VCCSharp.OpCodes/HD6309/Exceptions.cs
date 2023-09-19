using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.HD6309;

//--Apparently Hitachi has error handlers
internal class Exceptions : OpCode6309
{
    private int ErrorVector()
    {
        int cycles = 12 + DynamicCycles._54;  // One cycle for each byte pushed + Overhead

        CC_E = true; //--Everything is going on stack

        Push(PC);
        Push(U);
        Push(Y);
        Push(X);
        Push(DP);

        if (MD_NATIVE6309)
        {
            Push(W);

            cycles += 2;
        }

        Push(D);
        Push(CC);

        PC = M16[Define.VTRAP];

        return cycles;
    }

    public int DivideByZero()
    {
        MD_ZERODIV = true;

        return ErrorVector();
    }

    public int IllegalInstruction()
    {
        MD_ILLEGAL = true;

        return ErrorVector();
    }
}
