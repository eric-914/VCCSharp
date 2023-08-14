using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    // --[HITACHI]--
    //BITMD
    //IMMEDIATE
    public class _113C_Bitmd_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            byte masked = (byte)(value & 0xC0);
            byte and = (byte)(cpu.MD & masked);

            cpu.CC_Z = ZTEST(and);

            if ((and & 0x80) != 0) cpu.MD_ZERODIV = false;
            if ((and & 0x40) != 0) cpu.MD_ILLEGAL = false;

            return 4;
        }
    }
}
