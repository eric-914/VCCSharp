using System.DirectoryServices.ActiveDirectory;
using System.Windows.Documents;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using static System.Net.Mime.MediaTypeNames;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// BOR
    /// 🚫 6309 ONLY 🚫
    /// Logically OR Memory Bit with Register Bit
    /// DIRECT
    /// r.dstBit’ ← r.dstBit OR (DPM).srcBit
    /// SOURCE FORM             ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// BOR r,sBit,dBit,addr    DIRECT              1132        7 / 6       4
    /// </summary>
    /// <remarks>
    /// The BOR instruction logically ORs the value of a specified bit in either the A, B or CC registers with a specified bit in memory. 
    /// The resulting value is placed back into the register bit. 
    /// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit may be affected.
    /// 
    /// ──────────────────────────────────────────────────────────────────────────────────
    ///               Accumulator A                      Memory Location $0040
    ///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
    ///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
    ///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 0 │ 0 │ 0 │ $08   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
    ///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴─┬─┴───┴───┴───┴───┴───┴───╯
    ///  │                          ╭───╮                 │
    ///  │                       OR │ 1 │ ◀──────────────-╯
    ///  │                          ╰───╯   
    ///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
    ///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 0 │ 1 │ 0 │ $0A   BOR A,6,1,$40
    ///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
    /// ──────────────────────────────────────────────────────────────────────────────────
    /// 
    /// The figure above shows an example of the BOR instruction where bit 1 of Accumulator A is ORed with bit 6 of the byte in memory at address $0040 (DP = 0).
    /// 
    /// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
    /// 
    /// The usefulness of the BOR instruction is limited by the fact that only Direct Addressing is permitted. 
    /// Since the Condition Code flags are not affected by the operation, additional instructions would be needed to test the result for conditional branching.
    /// 
    /// The object code format for the BOR instruction is:
    /// ╭─────┬─────┬─────────-┬────────────-╮
    /// │ $11 │ $32 │ POSTBYTE │ ADDRESS LSB │
    /// ╰─────┴─────┴─────────-┴────────────-╯
    /// 
    /// See the description of the BAND instruction on page 20 for details about the postbyte format used by this instruction.
    /// 
    /// See Also: BAND, BEOR, BIAND, BIEOR, BIOR, LDBT, STBT
    /// </remarks>
    public class _1132_Bor : OpCode, IOpCode
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

            if ((mask & (1 << source)) != 0)
            {
                switch (value)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.OUR(value, destination);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC | (1 << destination));
                        break;
                }
            }

            // Else nothing changes
            return Cycles._76;
        }
    }
}
