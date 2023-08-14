using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //PULU
    //Pull A, B, CC, DP, D, X, Y, S or PC from hardware stack
    public class _37_Pulu_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);

            if ((_postByte & 0x01) != 0)
            {
                cpu.CC = cpu.MemRead8(cpu.U_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x02) != 0)
            {
                cpu.A_REG = cpu.MemRead8(cpu.U_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x04) != 0)
            {
                cpu.B_REG = cpu.MemRead8(cpu.U_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x08) != 0)
            {
                cpu.DPA = cpu.MemRead8(cpu.U_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x10) != 0)
            {
                cpu.X_H = cpu.MemRead8(cpu.U_REG++);
                cpu.X_L = cpu.MemRead8(cpu.U_REG++);

                cycles += 2;
            }

            if ((_postByte & 0x20) != 0)
            {
                cpu.Y_H = cpu.MemRead8(cpu.U_REG++);
                cpu.Y_L = cpu.MemRead8(cpu.U_REG++);

                cycles += 2;
            }

            if ((_postByte & 0x40) != 0)
            {
                cpu.S_H = cpu.MemRead8(cpu.U_REG++);
                cpu.S_L = cpu.MemRead8(cpu.U_REG++);

                cycles += 2;
            }

            if ((_postByte & 0x80) != 0)
            {
                cpu.PC_H = cpu.MemRead8(cpu.U_REG++);
                cpu.PC_L = cpu.MemRead8(cpu.U_REG++);

                cycles += 2;
            }

            return cycles;
        }

        public int Exec(IMC6809 cpu)=> Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
