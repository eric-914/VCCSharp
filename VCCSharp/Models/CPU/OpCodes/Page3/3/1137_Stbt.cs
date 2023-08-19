using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// STBT
    /// 🚫 6309 ONLY 🚫
    /// Store value of a Register Bit into Memory
    /// DIRECT
    /// (DPM).dstBit’ ← r.srcBit
    /// SOURCE FORM             ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// STBT r,sBit,dBit,addr   DIRECT              1137        8 / 7       4
    /// </summary>
    /// <remarks>
    /// The STBT instruction stores the value of a specified bit in either the A, B or CC registers to a specified bit in memory. 
    /// None of the Condition Code flags are affected by the operation. 
    /// The usefulness of the STBT instruction is limited by the fact that only Direct Addressing is permitted.
    /// ──────────────────────────────────────────────────────────────────────────────────
    ///           Memory Location $0040                       Accumulator A                      
    ///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
    ///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
    ///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ 1 | $0F   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
    ///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴───┴─┬─┴───┴───┴───┴───┴───╯
    ///  |                                                    │
    ///  │                            ╭───────────────────────╯
    ///  │                            ▼
    ///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
    ///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ 1 | $0D   STBT A,5,1,$40
    ///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
    /// ──────────────────────────────────────────────────────────────────────────────────
    /// The figure above shows an example of the STBT instruction where bit 5 from Accumulator A is stored into bit 1 of memory location $0040 (DP = 0).
    /// ╭─────┬─────┬─────────-┬────────────-╮
    /// │ $11 │ $37 │ POSTBYTE │ ADDRESS LSB │
    /// ╰─────┴─────┴─────────-┴────────────-╯
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///                                 POSTBYTE FORMAT
    ///       7   6   5   4   3   2   1   0                                                  ╭────────┬─────────-╮
    ///     ╭───┬───┬───┬───┬───┬───┬───┬───╮                                                │  Code  │ Register │   
    ///     |   |   |   |   |   |   |   |   |                                                ├────────┼──────────┤   
    ///     ╰─┬─┴─┬─┴─┬─┴───┴─┬─┴─┬─┴───┴─┬─╯                                                │  0 0   │    CC    │   
    ///       ╰─┬─╯   ╰───┬───╯   ╰───┬───╯                                                  │  0 1   │    A     │   
    ///         │         │           ╰-─────── Destination (memory) Bit Number (0 - 7)      │  1 0   │    B     │   
    ///         │         ╰-─────────────────── Source (register) Bit Number (0 - 7)         |  1 1   | Invalid  |   
    ///         ╰────────────────────────────── Register Code                                ╰────────┴─────────-╯   
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// 
    /// The object code format for the STBT instruction is:
    /// </remarks>
    public class _1137_Stbt : OpCode, IOpCode
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

            switch (value)
            {
                case 0: // A Reg
                case 1: // B Reg
                    value = cpu.PUR(value);
                    break;

                case 2: // CC Reg
                    value = cpu.CC;
                    break;
            }

            if ((value & (1 << source)) != 0)
            {
                mask |= (byte)(1 << destination);
            }
            else
            {
                mask &= (byte)~(1 << destination);
            }

            cpu.MemWrite8(mask, address);

            return Cycles._87;
        }
    }
}
