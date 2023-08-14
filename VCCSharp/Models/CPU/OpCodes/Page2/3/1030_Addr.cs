using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ADDR
    /// --> 6309 ONLY <--
    /// Add Source Register to Destination Register
    /// r1’ ← r1 + r0
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ADDR r0,r1    IMMEDIATE           1030         4           3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// The ADDR instruction adds the contents of a source register with the contents of a destination register. 
    /// The result is placed into the destination register.
    ///     H The Half-Carry flag is not affected by the ADDR instruction.
    ///     N The Negative flag is set equal to the value of the result’s high-order bit.
    ///     Z The Zero flag is set if the new value of the destination register is zero; cleared otherwise.
    ///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///     C The Carry flag is set if a carry out of the high-order bit occurred; cleared otherwise.
    /// Any of the 6309 registers except Q and MD may be specified as the source operand, destination operand or both; however specifying the PC register as either the source or destination produces undefined results.
    /// The ADDR instruction will perform either 8-bit or 16-bit addition according to the size of the destination register. 
    /// When registers of different sizes are specified, the source will be promoted, demoted or substituted depending on the size of the destination and on which specific 8-bit register is involved. 
    /// See “6309 Inter-Register Operations” on page 143 for further details.
    /// A Load Effective Address instruction which adds one of the 16-bit accumulators to an index register (such as LEAX D,X) could be replaced by an ADDR instruction (ADDR D,X) in order to save 4 cycles (2 cycles in Native Mode). 
    /// However, since more Condition Code flags are affected by the ADDR instruction, you should avoid this optimization if preservation of the affected flags is desired.
    /// The Immediate operand for this instruction is a postbyte which uses the same format as that used by the TFR and EXG instructions. 
    /// See the description of the TFR instruction for further details.
    /// 
    /// See Also: ADD (8-bit), ADD (16-bit)
    /// </remarks>
    public class _1030_Addr : OpCode, IOpCode
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
            else // 16 bit source - demote to 8 bit
            {
                source &= 7;
                source8 = (byte)cpu.PXF(source);
            }

            ushort sum = (ushort)(source8 + dest8);

            switch (destination)
            {
                case 2: cpu.CC = (byte)sum; break;
                case 4: case 5: break; // never assign to zero reg
                default: cpu.PUR(destination, (byte)sum); break;
            }

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, source8, dest8, (byte)sum);
            cpu.CC_N = NTEST8(cpu.PUR(destination));
            cpu.CC_Z = ZTEST(cpu.PUR(destination));
        }

            // 16 bit dest
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
                    case 0: case 1: source16 = cpu.D_REG; break; // A & B Reg
                    case 2: source16 = cpu.CC; break; // CC
                    case 3: source16 = cpu.DP_REG; break; // DP
                    case 4: case 5: source16 = 0; break; // Zero Reg
                    case 6: case 7: source16 = cpu.W_REG; break; // E & F Reg
                }
            }

            uint sum = (uint)(source16 + dest16);

            cpu.PXF(destination, (ushort)sum);

            cpu.CC_C = (sum & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, source16, dest16, (ushort)sum);
            cpu.CC_N = NTEST16(cpu.PXF(destination));
            cpu.CC_Z = ZTEST(cpu.PXF(destination));
        }
    }
}
