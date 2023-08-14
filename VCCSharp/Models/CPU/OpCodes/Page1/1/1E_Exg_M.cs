using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //EXG R1, R2
    //Exchange R1 with R2 (R1, R2 = A, B, CC, DP)
    //TODO: Need to verify all this
    public class _1E_Exg_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            var value = cpu.MemRead8(cpu.PC_REG++);
            byte temp8;
            ushort temp16;

            if (((value & 0x80) >> 4) == (value & 0x08)) //Verify like size registers
            {
                if ((value & 0x08) != 0) //8 bit EXG
                {
                    temp8 = cpu.PUR(((value & 0x70) >> 4));
                    cpu.PUR(((value & 0x70) >> 4), cpu.PUR(value & 0x07));
                    cpu.PUR(value & 0x07, temp8);
                }
                else // 16 bit EXG
                {
                    temp16 = cpu.PXF((value & 0x70) >> 4);
                    cpu.PXF((value & 0x70) >> 4, cpu.PXF(value & 0x07));
                    cpu.PXF(value & 0x07, temp16);
                }
            }

            return 8;
        }

        public int Exec(IHD6309 cpu)
        {
            var value = cpu.MemRead8(cpu.PC_REG++);
            var source = (byte)(value >> 4);
            var destination = (byte)(value & 15);
            byte temp8;
            ushort temp16;

            if ((source & 0x08) == (destination & 0x08)) //Verify like size registers
            {
                if ((destination & 0x08) != 0) //8 bit EXG
                {
                    source &= 0x07;
                    destination &= 0x07;

                    temp8 = cpu.PUR(source);
                    cpu.PUR(source, cpu.PUR(destination));
                    cpu.PUR(destination, temp8);

                    cpu.O_REG = 0;
                }
                else // 16 bit EXG
                {
                    source &= 0x07;
                    destination &= 0x07;

                    temp16 = cpu.PXF(source);
                    cpu.PXF(source, destination);
                    cpu.PXF(destination, temp16);
                }
            }
            else
            {
                if ((destination & 0x08) != 0) // Swap 16 to 8 bit exchange to be 8 to 16 bit exchange (for convenience)
                {
                    temp8 = destination; destination = source; source = temp8;
                }

                source &= 0x07;
                destination &= 0x07;

                byte tmp;

                switch (source)
                {
                    case 0x04: // Z
                    case 0x05: // Z
                        cpu.PXF(destination, 0); // Source is Zero reg. Just zero the Destination.
                        break;

                    case 0x00: // A
                    case 0x03: // DP
                    case 0x06: // E
                        temp8 = cpu.PUR(source);
                        temp16 = (ushort)((temp8 << 8) | temp8);
                        tmp = (byte)(cpu.PXF(destination) >> 8);
                        cpu.PUR(source, tmp); // A, DP, E get high byte of 16 bit Dest
                        cpu.PXF(destination, temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;

                    case 0x01: // B
                    case 0x02: // CC
                    case 0x07: // F
                        temp8 = cpu.PUR(source);
                        temp16 = (ushort)((temp8 << 8) | temp8);
                        tmp = (byte)(cpu.PXF(destination) & 0xFF);
                        cpu.PUR(source, tmp); // B, CC, F get low byte of 16 bit Dest
                        cpu.PXF(destination, temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;
                }
            }

            return Cycles._85;
        }
    }
}