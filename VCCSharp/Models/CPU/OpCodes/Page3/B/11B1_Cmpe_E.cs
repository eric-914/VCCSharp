using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.B
{
    // --[HITACHI]--
    //CMPE
    //EXTENDED
    public class _11B1_Cmpe_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);
            byte compare = (byte)(cpu.E_REG - value);

            cpu.CC_C = compare > cpu.E_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.E_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            cpu.PC_REG += 2;

            return Cycles._65;
        }
    }
}
