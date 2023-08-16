using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// CMPR
    /// 🚫 6309 ONLY 🚫
    /// Compare Source Register from Destination Register
    /// IMMEDIATE
    /// TEMP ← r1 - r0
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// CMPR r0,r1      IMMEDIATE           1037        4           3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// The CMPR instruction subtracts the contents of a source register from the contents of a destination register and sets the Condition Codes accordingly. 
    /// Neither register is modified.
    ///         H The Half-Carry flag is not affected by this instruction.
    ///         N The Negative flag is set equal to the value of the high-order bit of the result.
    ///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow into the high-order bit was needed; cleared otherwise.
    /// 
    /// Any of the 6309 registers except Q and MD may be specified as the source operand, destination operand or both; however specifying the PC register as either the source or destination produces undefined results.
    /// 
    /// The CMPR instruction will perform either an 8-bit or 16-bit comparison according to the size of the destination register. 
    /// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
    /// See “6309 Inter-Register Operations” on page 143 for further details.
    /// 
    /// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
    /// See the description of the TFR instruction starting on page 137 for further details.
    /// 
    /// See Also: ADD (8-bit), ADD (16-bit)
    /// </remarks>
    public class _1037_Cmpr : OpCode, IOpCode
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

                if (source == 2)
                {
                    source8 = cpu.CC;
                }
                else
                {
                    source8 = cpu.PUR(source);
                }
            }
            else // 16 bit source - demote to 8 bit
            {
                source &= 7;
                source8 = (byte)cpu.PXF(source);
            }

            ushort difference = (ushort)(dest8 - source8);

            byte value = (byte)difference;

            cpu.CC_C = (difference & 0x100) >> 8 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((dest8 ^ value ^ source8) >> 7)) != 0;
            cpu.CC_N = value >> 7 != 0;
            cpu.CC_Z = ZTEST(value);
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

            uint difference = (uint)(dest16 - source16);

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = ((dest16 ^ source16 ^ difference ^ (difference >> 1)) & 0x8000) != 0;
            cpu.CC_N = (difference & 0x8000) >> 15 != 0;
            cpu.CC_Z = ZTEST(difference);
        }
    }
}
