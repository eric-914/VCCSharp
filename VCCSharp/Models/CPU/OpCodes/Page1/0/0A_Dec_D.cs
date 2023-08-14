using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //DEC
    //Decrement accumulator or memory location
    public class _0A_Dec_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = (byte)(cpu.MemRead8(address) - 1);

            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = NTEST8(value);
            cpu.CC_V = value == 0x7F;

            cpu.MemWrite8(value, address);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
