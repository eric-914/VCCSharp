using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //PAGE 2
    //0x10__
    public class _10_Page_2 : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            throw new NotImplementedException();

            byte opCode = cpu.MemRead8(cpu.PC_REG++);

            //TODO: Need jump vectors defined here...

            //_jumpVectors0x10[opCode]();

            //TODO: Need cycle count penalty.  Probably from Page 2 opcodes.

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 0);
        public int Exec(IHD6309 cpu) => Exec(cpu, 0);
    }
}
