using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.A
{
    //CMPY
    //Compare memory from index register
    public class _10AC_Cmpy_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort _postWord = cpu.MemRead16(address);
            ushort _temp16 = (ushort)(cpu.Y_REG - _postWord);

            cpu.CC_C = _temp16 > cpu.Y_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, _postWord, _temp16, cpu.Y_REG);
            cpu.CC_N = NTEST16(_temp16);
            cpu.CC_Z = ZTEST(_temp16);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
