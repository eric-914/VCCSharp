namespace VCCSharp.OpCodes.Tests.Model.HD6309.O;

internal partial class OldOpcodes
{
    public ushort CalculateEA(byte postByte)
    {
        ushort ea = 0;

        byte reg = (byte)(((postByte >> 5) & 3) + 1);

        if ((postByte & 0x80) != 0)
        {
            switch (postByte & 0x1F)
            {
                case 0:
                    ea = PXF(reg);
                    PXF(reg, (ushort)(PXF(reg) + 1));
                    _cycleCounter += 2;
                    break;

                case 1:
                    ea = PXF(reg);
                    PXF(reg, (ushort)(PXF(reg) + 2));
                    _cycleCounter += 3;
                    break;

                case 2:
                    PXF(reg, (ushort)(PXF(reg) - 1));
                    ea = PXF(reg);
                    _cycleCounter += 2;
                    break;

                case 3:
                    PXF(reg, (ushort)(PXF(reg) - 2));
                    ea = PXF(reg);
                    _cycleCounter += 3;
                    break;

                case 4:
                    ea = PXF(reg);
                    break;

                case 5:
                    ea = (ushort)(PXF(reg) + ((sbyte)B_REG));
                    _cycleCounter += 1;
                    break;

                case 6:
                    ea = (ushort)(PXF(reg) + ((sbyte)A_REG));
                    _cycleCounter += 1;
                    break;

                case 7:
                    //ea = (ushort)(PXF(reg) + ((sbyte)E_REG));
                    _cycleCounter += 1;
                    break;

                case 8:
                    ea = (ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++));
                    _cycleCounter += 1;
                    break;

                case 9:
                    ea = (ushort)(PXF(reg) + MemRead16(PC_REG));
                    _cycleCounter += 4;
                    PC_REG += 2;
                    break;

                case 10:
                    //ea = (ushort)(PXF(reg) + ((sbyte)F_REG));
                    _cycleCounter += 1;
                    break;

                case 11:
                    ea = (ushort)(PXF(reg) + D_REG);
                    _cycleCounter += 4;
                    break;

                case 12:
                    ea = (ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1);
                    _cycleCounter += 1;
                    PC_REG++;
                    break;

                case 13: //MM
                    ea = (ushort)(PC_REG + MemRead16(PC_REG) + 2);
                    _cycleCounter += 5;
                    PC_REG += 2;
                    break;

                case 14:
                    //ea = (ushort)(PXF(reg) + W_REG);
                    _cycleCounter += 4;
                    break;

                case 15: //01111
                    sbyte signedByte = (sbyte)((postByte >> 5) & 3);

                    switch (signedByte)
                    {
                        case 0:
                            //ea = W_REG;
                            break;

                        case 1:
                            //ea = (ushort)(W_REG + MemRead16(PC_REG));
                            PC_REG += 2;
                            //_cycleCounter += 2;
                            break;

                        case 2:
                            //ea = W_REG;
                            break;

                        case 3:
                            //W_REG -= 2;
                            break;
                    }
                    break;

                case 16: //10000
                    signedByte = (sbyte)((postByte >> 5) & 3);

                    switch (signedByte)
                    {
                        case 0:
                            //ea = MemRead16(W_REG);
                            break;

                        case 1:
                            //ea = MemRead16((ushort)(W_REG + MemRead16(PC_REG)));
                            PC_REG += 2;
                            //_cycleCounter += 5;
                            break;

                        case 2:
                            //ea = MemRead16(W_REG);
                            break;

                        case 3:
                            //W_REG -= 2;
                            break;
                    }
                    break;

                case 17: //10001
                    ea = PXF(reg);
                    PXF(reg, (ushort)(PXF(reg) + 2));
                    ea = MemRead16(ea);
                    _cycleCounter += 6;
                    break;

                case 18: //10010
                    _cycleCounter += 6;
                    break;

                case 19: //10011
                    PXF(reg, (ushort)(PXF(reg) - 2));
                    ea = MemRead16(PXF(reg));
                    _cycleCounter += 6;
                    break;

                case 20: //10100
                    ea = MemRead16(PXF(reg));
                    _cycleCounter += 3;
                    break;

                case 21: //10101
                    ea = MemRead16((ushort)(PXF(reg) + ((sbyte)B_REG)));
                    _cycleCounter += 4;
                    break;

                case 22: //10110
                    ea = MemRead16((ushort)(PXF(reg) + ((sbyte)A_REG)));
                    _cycleCounter += 4;
                    break;

                case 23: //10111
                    //ea = MemRead16((ushort)(PXF(reg) + ((sbyte)E_REG)));
                    ea = MemRead16(ea);
                    _cycleCounter += 4;
                    break;

                case 24: //11000
                    ea = MemRead16((ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++)));
                    _cycleCounter += 4;
                    break;

                case 25: //11001
                    ea = MemRead16((ushort)(PXF(reg) + MemRead16(PC_REG)));
                    _cycleCounter += 7;
                    PC_REG += 2;
                    break;

                case 26: //11010
                    //ea = MemRead16((ushort)(PXF(reg) + ((sbyte)F_REG)));
                    ea = MemRead16(ea);
                    _cycleCounter += 4;
                    break;

                case 27: //11011
                    ea = MemRead16((ushort)(PXF(reg) + D_REG));
                    _cycleCounter += 7;
                    break;

                case 28: //11100
                    ea = MemRead16((ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1));
                    _cycleCounter += 4;
                    PC_REG++;
                    break;

                case 29: //11101
                    ea = MemRead16((ushort)(PC_REG + MemRead16(PC_REG) + 2));
                    _cycleCounter += 8;
                    PC_REG += 2;
                    break;

                case 30: //11110
                    //ea = MemRead16((ushort)(PXF(reg) + W_REG));
                    ea = MemRead16(ea);
                    _cycleCounter += 7;
                    break;

                case 31: //11111
                    ea = MemRead16(MemRead16(PC_REG));
                    _cycleCounter += 8;
                    PC_REG += 2;
                    break;
            }
        }
        else
        {
            sbyte signedByte = (sbyte)(postByte & 0x1F);
            signedByte <<= 3; //--Push the "sign" to the left-most bit.
            signedByte /= 8;

            ea = (ushort)(PXF(reg) + signedByte); //Was signed

            _cycleCounter += 1;
        }

        return ea;
    }
}
