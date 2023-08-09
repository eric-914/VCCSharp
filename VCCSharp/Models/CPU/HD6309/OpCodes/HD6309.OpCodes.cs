namespace VCCSharp.Models.CPU.HD6309;

// ReSharper disable once InconsistentNaming
public partial class HD6309
{
    private byte _temp8;
    private ushort _temp16;
    private uint _temp32;

    private byte _postByte;
    private ushort _postWord;

    private short _signedShort;
    private int _signedInt;

    private byte _source;
    private byte _dest;

    #region Jump Vectors

    private static Action[] _jumpVectors = new Action[256];
    private static Action[] _jumpVectors0x10 = new Action[256]; //--Page 2 instructions
    private static Action[] _jumpVectors0x11 = new Action[256]; //--Page 3 instructions

    private void InitializeJmpVectors()
    {
        _jumpVectors = JumpVectors();
        _jumpVectors0x10 = JumpVectors0x10();
        _jumpVectors0x11 = JumpVectors0x11();
    }

    #endregion

    private static void Exec(byte opCode) => _jumpVectors[opCode]();

    /// <summary>
    /// Invalid Instruction Handler
    /// </summary>
    private void _____()
    {
        MD_ILLEGAL = true;
        _cpu.md.bits = GetMD();

        ErrorVector();
    }

    private void ErrorVector()
    {
        CC_E = true; //1;

        MemWrite8(PC_L, --S_REG);
        MemWrite8(PC_H, --S_REG);
        MemWrite8(U_L, --S_REG);
        MemWrite8(U_H, --S_REG);
        MemWrite8(Y_L, --S_REG);
        MemWrite8(Y_H, --S_REG);
        MemWrite8(X_L, --S_REG);
        MemWrite8(X_H, --S_REG);
        MemWrite8(DPA, --S_REG);

        if (MD_NATIVE6309)
        {
            MemWrite8(F_REG, --S_REG);
            MemWrite8(E_REG, --S_REG);

            _cycleCounter += 2;
        }

        MemWrite8(B_REG, --S_REG);
        MemWrite8(A_REG, --S_REG);
        MemWrite8(GetCC(), --S_REG);

        PC_REG = MemRead16(Define.VTRAP);

        _cycleCounter += 12 + _instance._54;	//One for each byte +overhead? Guessing from PSHS
    }

    public void DivByZero()
    {
        MD_ZERODIV = true; //1;

        _cpu.md.bits = GetMD();

        ErrorVector();
    }
}
