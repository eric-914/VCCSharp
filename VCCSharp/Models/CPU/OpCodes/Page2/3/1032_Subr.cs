using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// SUBR
    /// 🚫 6309 ONLY 🚫
    /// Subtract Source Register from Destination Register
    /// IMMEDIATE
    /// r1’ ← r1 - r0
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SUBR r0,r1      IMMEDIATE           1032        4           3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// The SUBR instruction subtracts the value contained in the source register from the value contained in the destination register. 
    /// The result is placed into the destination register.
    /// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
    ///         H The Half-Carry flag is not affected by the SUBR instruction.
    ///         N The Negative flag is set equal to the value of the result’s high-order bit.
    ///         Z The Zero flag is set if the new value of the destination register is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow into the high-order bit was needed; cleared otherwise.
    ///         
    /// All of the 6309 registers except Q and MD can be specified as either the source or destination; however specifying the PC register as either the source or destination produces undefined results.
    /// 
    /// The SUBR instruction will perform either 8-bit or 16-bit subtraction according to the size of the destination register. 
    /// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
    /// See “6309 Inter-Register Operations” on page 143 for further details.
    /// 
    /// Although the SUBR instruction is capable of altering the flow of program execution by specifying the PC register as the destination, you should avoid doing so because the prefetch capability of the 6309 can produce un-predictable results.
    /// 
    /// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
    /// See the description of the TFR instruction for further details.
    /// 
    /// See Also: SUB (8-bit), SUB (16-bit)
    /// </remarks>
    public class _1032_Subr : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte source = (byte)(value >> 4);
            byte destination = (byte)(value & 15);

            if (destination > 7)
            {
                Exec_8bit(cpu, source, destination);
            }
            else
            {
                Exec_16bit(cpu, source, destination);
            }

            return 4;
        }

        // 8 bit dest
        private static void Exec_8bit(IHD6309 cpu, byte source, byte destination)
        {
            destination &= 7;

            byte dest8 = destination == 2 ? cpu.CC : cpu.PUR(destination);

            byte source8;

            if (source > 7)
            {
                // 8 bit source
                source &= 7;

                source8 = source == 2 ? cpu.CC : cpu.PUR(source);
            }
            else
            { // 16 bit source - demote to 8 bit
                source &= 7;

                source8 = (byte)cpu.PXF(source);
            }

            ushort difference = (ushort)(dest8 - source8);

            switch (destination)
            {
                case 2:
                    cpu.CC = (byte)difference;
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    cpu.PUR(destination, (byte)difference);
                    break;
            }

            cpu.CC_C = (difference & 0x100) >> 8 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((dest8 ^ cpu.PUR(destination) ^ source8) >> 7)) != 0;
            cpu.CC_N = cpu.PUR(destination) >> 7 != 0;
            cpu.CC_Z = ZTEST(cpu.PUR(destination));
        }

        // 16 bit dest
        private static void Exec_16bit(IHD6309 cpu, byte _source, byte _dest)
        {
            ushort source16 = 0;
            ushort dest16 = cpu.PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = cpu.PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
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

            uint difference = (uint)(dest16 - source16);

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = ((dest16 ^ source16 ^ difference ^ (difference >> 1)) & 0x8000) != 0;

            cpu.PXF(_dest, (ushort)difference);

            cpu.CC_N = (difference & 0x8000) >> 15 != 0;
            cpu.CC_Z = ZTEST(difference);
        }
    }
}
