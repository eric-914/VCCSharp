using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CMPX
    //Compare memory from index register
    public class _9C_Cmpx_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);
            var _postWord = cpu.MemRead16(address);
            var _temp16 = (ushort)(cpu.X_REG - _postWord);

            cpu.CC_C = _temp16 > cpu.X_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, _postWord, _temp16, cpu.X_REG);
            cpu.CC_N = NTEST16(_temp16);
            cpu.CC_Z = ZTEST(_temp16);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._64);
    }
}
