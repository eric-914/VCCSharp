using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// LDBT
    /// 🚫 6309 ONLY 🚫
    /// Load Memory Bit into Register Bit
    /// r.dstBit’ ← (DPM).srcBit
    /// SOURCE FORM             ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LDBT r,sBit,dBit,addr   DIRECT              1136        7 / 6       4
    /// </summary>
    /// <remarks>
    /// The LDBT instruction loads the value of a specified bit in memory into a specified bit of either the A, B or CC registers. 
    /// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit will be affected. 
    /// The usefulness of the LDBT instruction is limited by the fact that only Direct Addressing is permitted.
    /// ──────────────────────────────────────────────────────────────────────────────────
    ///               Accumulator A                      Memory Location $0040
    ///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
    ///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
    ///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ 1 │ $0F   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
    ///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴───┴─┬─┴───┴───┴───┴───┴───╯
    ///  │                                                    │
    ///  │                            ╭───────────────────────╯
    ///  │                            ▼
    ///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
    ///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ 1 │ $0D   LDBT A,5,1,$40
    ///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
    /// ──────────────────────────────────────────────────────────────────────────────────
    /// The figure above shows an example of the LDBT instruction where bit 1 of Accumulator A is Loaded with bit 5 of the byte in memory at address $0040 (DP = 0).
    /// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
    /// 
    /// The object code format for the LDBT instruction is:
    /// ╭─────┬─────┬─────────-┬────────────-╮
    /// │ $11 │ $36 │ POSTBYTE │ ADDRESS LSB │
    /// ╰─────┴─────┴─────────-┴────────────-╯
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///                                 POSTBYTE FORMAT
    ///       7   6   5   4   3   2   1   0                                                  ╭────────┬─────────-╮
    ///     ╭───┬───┬───┬───┬───┬───┬───┬───╮                                                │  Code  │ Register │   
    ///     │   │   │   │   │   │   │   │   │                                                ├────────┼──────────┤   
    ///     ╰─┬─┴─┬─┴─┬─┴───┴─┬─┴─┬─┴───┴─┬─╯                                                │  0 0   │    CC    │   
    ///       ╰─┬─╯   ╰───┬───╯   ╰───┬───╯                                                  │  0 1   │    A     │   
    ///         │         │           ╰-─────── Destination (register) Bit Number (0 - 7)    │  1 0   │    B     │   
    ///         │         ╰-─────────────────── Source (memory) Bit Number (0 - 7)           │  1 1   │ Invalid  │   
    ///         ╰────────────────────────────── Register Code                                ╰────────┴─────────-╯   
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// 
    /// See Also: BAND, BEOR, BIAND, BIEOR, BIOR, BOR, STBT
    /// </remarks>
    public class _1136_Ldbt : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);
            byte _temp8 = cpu.MemRead8(cpu.DPADDRESS(cpu.PC_REG++));

            byte _source = (byte)((_postByte >> 3) & 7);
            byte _dest = (byte)(_postByte & 7);

            _postByte >>= 6;

            if (_postByte == 3)
            {
                return InvalidOpCode.Exec(cpu);
            }

            if ((_temp8 & (1 << _source)) != 0)
            {
                switch (_postByte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.OUR(_postByte, _dest);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC | (1 << _dest));
                        break;
                }
            }
            else
            {
                switch (_postByte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.AUR(_postByte, _dest);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC & ~(1 << _dest));
                        break;
                }
            }

            // Else nothing changes
            return Cycles._76;
        }
    }
}
