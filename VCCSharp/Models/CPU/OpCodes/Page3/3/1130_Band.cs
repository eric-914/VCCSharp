using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// BAND
    /// --> 6309 ONLY <--
    /// Logically AND Register Bit with Memory Bit
    /// DIRECT
    /// r.dstBit’ ← r.dstBit AND (DPM).srcBit
    /// SOURCE FORM             ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// BAND r,sBit,dBit,addr   DIRECT              1130         7 / 6       4
    /// </summary>
    /// <remarks>
    /// The BAND instruction logically ANDs the value of a specified bit in either the A, B or CC registers with a specified bit in memory. 
    /// The resulting value is placed back into the register bit. 
    /// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit may be affected. 
    /// The usefulness of the BAND instruction is limited by the fact that only Direct Addressing is permitted.
    /// 
    /// --------------------------------------------------------------------------------
    ///               Accumulator A                      Memory Location $0040
    ///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
    ///     +---+---+---+---+---+---+---+---+       +---+---+---+---+---+---+---+---+
    ///     | 0 | 0 | 0 | 0 | 1 | 1 | 1 | 1 | $0F   | 1 | 1 | 0 | 0 | 0 | 1 | 1 | 0 | $C6
    ///  |  +---+---+---+---+---+---+---+---+       +---+---+---+---+---+---+---+---+
    ///  |                                                    |
    ///  |                          +---+                     |
    ///  |                     AND  | 0 | ◀-------------------+
    ///  |                          +---+
    ///  |                          
    ///  ▼  +---+---+---+---+---+---+---+---+    
    ///     | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | $0D   BAND A,5,1,$40
    ///     +---+---+---+---+---+---+---+---+    
    /// --------------------------------------------------------------------------------
    /// 
    /// The figure above shows an example of the BAND instruction where bit 1 of Accumulator A is ANDed with bit 5 of the byte in memory at address $0040 (DP = 0).
    /// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
    /// Since the Condition Code flags are not affected by the operation, additional instructions would be needed to test the result for conditional branching.
    /// 
    /// The object code format for the BAND instruction is:
    /// +-----+-----+----------+-------------+
    /// | $11 | $30 | POSTBYTE | ADDRESS LSB |
    /// +-----+-----+----------+-------------+
    /// 
    /// --------------------------------------------------------------------------------
    ///                                 POSTBYTE FORMAT
    ///       7   6   5   4   3   2   1   0  
    ///     +---+---+---+---+---+---+---+---+                                                     +--------+----------+
    ///     |   |   |   |   |   |   |   |   |                                                     |  Code  | Register |
    ///     +---+---+---+---+---+---+---+---+                                                     +--------+----------+
    ///       |___|   |_______|   |_______|                                                       |  0 0   |    CC    |
    ///         |         |           |                                                           |  0 1   |    A     |
    ///         |         |           +---------- Destination (register) Bit Number (0 - 7)       |  1 0   |    B     |
    ///         |         +---------------------- Source (memory) Bit Number (0 - 7)              |  1 1   | Invalid  |
    ///         +-------------------------------- Register Code                                   +--------+----------+
    /// --------------------------------------------------------------------------------
    /// 
    /// See Also: BEOR, BIAND, BIEOR, BIOR, BOR, LDBT, STBT
    /// </remarks>
    public class _1130_Band : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte mask = cpu.MemRead8(address);

            byte source = (byte)((value >> 3) & 7);
            byte destination = (byte)(value & 7);

            value >>= 6;

            if (value == 3)
            {
                return InvalidOpCode.Exec(cpu);
            }

            if ((mask & (1 << source)) == 0)
            {
                switch (value)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.AUR(value, destination);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC & ~(1 << destination));
                        break;
                }
            }

            // Else nothing changes
            return Cycles._76;
        }
    }
}
