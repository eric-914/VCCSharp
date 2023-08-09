namespace VCCSharp.Models.CPU.OpCodes
{
    public interface IRegisters
    {
        ushort PC_REG { get; set; }
        ushort DP_REG { get; set; }
        ushort D_REG { get; set; }
        ushort S_REG { get; set; }

        byte A_REG { get; set; }
        byte B_REG { get; set; }

        byte PC_L { get; set; }
        byte PC_H { get; set; }

        bool CC_E { get; set; }
        bool CC_F { get; set; }
        bool CC_H { get; set; }
        bool CC_I { get; set; }
        bool CC_N { get; set; }
        bool CC_Z { get; set; }
        bool CC_V { get; set; }
        bool CC_C { get; set; }

        byte CC { get; set; }

        byte PUR(int i);
        void PUR(int i, byte value);

        ushort PXF(int i);
        void PXF(int i, ushort value);
    }
}
