using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //PSHS
    //Push A, B, CC, DP, D, X, Y, U, or PC onto hardware stack
    public class _34_Pshs_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);

            if ((_postByte & 0x80) != 0)
            {
                cpu.MemWrite8(cpu.PC_L, --cpu.S_REG);
                cpu.MemWrite8(cpu.PC_H, --cpu.S_REG);

                cycles += 2;
            }

            if ((_postByte & 0x40) != 0)
            {
                cpu.MemWrite8(cpu.U_L, --cpu.S_REG);
                cpu.MemWrite8(cpu.U_H, --cpu.S_REG);

                cycles += 2;
            }

            if ((_postByte & 0x20) != 0)
            {
                cpu.MemWrite8(cpu.Y_L, --cpu.S_REG);
                cpu.MemWrite8(cpu.Y_H, --cpu.S_REG);

                cycles += 2;
            }

            if ((_postByte & 0x10) != 0)
            {
                cpu.MemWrite8(cpu.X_L, --cpu.S_REG);
                cpu.MemWrite8(cpu.X_H, --cpu.S_REG);

                cycles += 2;
            }

            if ((_postByte & 0x08) != 0)
            {
                cpu.MemWrite8(cpu.DPA, --cpu.S_REG);

                cycles += 1;
            }

            if ((_postByte & 0x04) != 0)
            {
                cpu.MemWrite8(cpu.B_REG, --cpu.S_REG);

                cycles += 1;
            }

            if ((_postByte & 0x02) != 0)
            {
                cpu.MemWrite8(cpu.A_REG, --cpu.S_REG);

                cycles += 1;
            }

            if ((_postByte & 0x01) != 0)
            {
                cpu.MemWrite8(cpu.CC, --cpu.S_REG);

                cycles += 1;
            }

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
