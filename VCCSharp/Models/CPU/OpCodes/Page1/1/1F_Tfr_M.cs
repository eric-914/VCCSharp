using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    internal class _1F_Tfr_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            byte source = (byte)(value >> 4);
            byte destination = (byte)(value & 15);

            switch (destination)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    cpu.PXF(destination, 0xFFFF);

                    if (source == 12 || source == 13)
                    {
                        cpu.PXF(destination, 0);
                    }
                    else if (source <= 7)
                    {
                        cpu.PXF(destination, cpu.PXF(source));
                    }

                    break;

                case 8:
                case 9:
                case 10:
                case 11:
                case 14:
                case 15:
                    cpu.PUR(destination & 7, 0xFF);

                    if ((source == 12) || (source == 13))
                    {
                        cpu.PUR(destination & 7, 0);
                    }
                    else if (source > 7)
                    {
                        cpu.PUR(destination & 7, cpu.PUR(source & 7));
                    }

                    break;
            }

            return 6;
        }

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            byte source = (byte)(value >> 4);
            byte destination = (byte)(value & 15);

            if (destination < 8)
            {
                if (source < 8)
                {
                    cpu.PXF(destination, cpu.PXF(source));
                }
                else
                {
                    cpu.PXF(destination, (ushort)((cpu.PUR(source & 7) << 8) | cpu.PUR(source & 7)));
                }
            }
            else
            {
                destination &= 7;

                if (source < 8)
                    switch (destination)
                    {
                        case 0:  // A
                        case 3: // DP
                        case 6: // E
                            cpu.PUR(destination, (byte)(cpu.PXF(source) >> 8));
                            break;

                        case 1:  // B
                        case 2: // CC
                        case 7: // F
                            cpu.PUR(destination, (byte)(cpu.PXF(source) & 0xFF));
                            break;
                    }
                else
                {
                    cpu.PUR(destination, cpu.PUR(source & 7));
                }

                cpu.O_REG = 0;
            }

            return Cycles._64;
        }
    }
}
