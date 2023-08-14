using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //LSR
    //Logical shift right accumulator or memory location
    //EXTENDED
    public class _74_Lsr_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _temp16 = cpu.MemRead16(cpu.PC_REG);
            var _temp8 = cpu.MemRead8(_temp16);

            cpu.CC_C = (_temp8 & 1) != 0;

            _temp8 = (byte)(_temp8 >> 1);

            cpu.CC_Z = ZTEST(_temp8);
            cpu.CC_N = false;

            cpu.MemWrite8(_temp8, _temp16);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
