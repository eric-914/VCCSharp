using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.Models.CPU.MC6809;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
public partial class MC6809
{
    public void Swi3_I() //113F 
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
        MemWrite8(B_REG, --S_REG);
        MemWrite8(A_REG, --S_REG);
        MemWrite8(GetCC(), --S_REG);
        PC_REG = MemRead16(Define.VSWI3);

        _cycleCounter += 20;
    }

    public void Cmpu_M() //1183 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Cmps_M() //118C 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Cmpu_D() //1193 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 7;
    }

    public void Cmps_D() //119C 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 7;
    }

    public void Cmpu_X() //11A3 
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postWord = MemRead16(address);
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 7;
    }

    public void Cmps_X() //11AC 
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postWord = MemRead16(address);
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 7;
    }

    public void Cmpu_E() //11B3 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;

        _cycleCounter += 8;
    }

    public void Cmps_E() //11BC 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;

        _cycleCounter += 8;
    }
}
// ReSharper restore IdentifierTypo
// ReSharper restore InconsistentNaming
