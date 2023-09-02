using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.HD6309;

//--Apparently Hitachi has error handlers
internal class Exceptions : OpCode6309
{
    private int ErrorVector()
    {
        int cycles = 0;

        CC_E = true; //--Everything is going on stack

        //PC
        M8[--S] = PC_L;
        M8[--S] = PC_H;

        //U
        M8[--S] = U_L;
        M8[--S] = U_H;

        //Y
        M8[--S] = Y_L;
        M8[--S] = Y_H;

        //X
        M8[--S] = X_L;
        M8[--S] = X_H;

        //DP
        M8[--S] = DP;

        if (MD_NATIVE6309)
        {
            //W/F/E
            M8[--S] = F;
            M8[--S] = E;

            cycles += 2;
        }

        //D/B/A
        M8[--S] = B;
        M8[--S] = A;

        //CC
        M8[--S] = CC;

        PC = M16[Define.VTRAP];

        cycles += 12 + DynamicCycles._54;  //One for each byte +overhead? Guessing from PSHS

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
