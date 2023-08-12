using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _1036_Eorr : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte source = (byte)(value >> 4);
            byte destination = (byte)(value & 15);

            if (destination > 7) // 8 bit dest
            {
                Exec_8bit(cpu, source, destination);
            }
            else // 16 bit dest
            {
                Exec_16bit(cpu, source, destination);
            }

            cpu.CC_V = false;

            return 4;
        }

        private static void Exec_8bit(IHD6309 cpu, byte source, byte destination)
        {
            destination &= 7;

            byte dest8 = destination == 2 ? cpu.CC : cpu.PUR(destination);
            byte source8;

            if (source > 7) // 8 bit source
            {
                source &= 7;

                source8 = source == 2 ? cpu.CC : cpu.PUR(source);
            }
            else // 16 bit source - demote to 8 bit
            {
                source &= 7;
                source8 = (byte)cpu.PXF(source);
            }

            byte xor = (byte)(dest8 ^ source8);

            switch (destination)
            {
                case 2:
                    cpu.CC = xor;
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    cpu.PUR(destination, xor);
                    break;
            }

            cpu.CC_N = xor >> 7 != 0;
            cpu.CC_Z = ZTEST(xor);
        }

        private static void Exec_16bit(IHD6309 cpu, byte source, byte destination)
        {
            ushort source16 = 0;
            ushort dest16 = cpu.PXF(destination);

            if (source < 8) // 16 bit source
            {
                source16 = cpu.PXF(source);
            }
            else // 8 bit source - promote to 16 bit
            {
                source &= 7;

                switch (source)
                {
                    case 0:
                    case 1:
                        source16 = cpu.D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = cpu.CC;
                        break; // CC

                    case 3:
                        source16 = cpu.DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = cpu.W_REG;
                        break; // E & F Reg
                }
            }

            ushort xor = (ushort)(dest16 ^ source16);

            cpu.PXF(destination, xor);

            cpu.CC_N = xor >> 15 != 0;
            cpu.CC_Z = ZTEST(xor);
        }
    }
}
