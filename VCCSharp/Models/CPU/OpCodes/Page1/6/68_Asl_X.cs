using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ASL
    //Arithmetic shift of accumulator or memory 
    public class _68_Asl_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);

            cpu.CC_C = value > 0x7F;
            cpu.CC_V = cpu.CC_C ^ ((value & 0x40) >> 6 != 0);

            value = (byte)(value << 1);

            cpu.CC_N = NTEST8(value);
            cpu.CC_Z = ZTEST(value);

            cpu.MemWrite8(value, address);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, 6);
    }
}
