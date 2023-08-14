using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    // --[HITACHI]--
    //OIM
    //DIRECT
    public class _01_Oim_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);
            ushort _temp16 = cpu.DPADDRESS(cpu.PC_REG++);

            _postByte |= cpu.MemRead8(_temp16);

            cpu.MemWrite8(_postByte, _temp16);

            cpu.CC_N = NTEST8(_postByte);
            cpu.CC_Z = ZTEST(_postByte);
            cpu.CC_V = false;

            return 6;
        }
    }
}
