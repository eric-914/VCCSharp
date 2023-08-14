using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    // --[HITACHI]--
    //DIVQ
    //INDEXED
    public class _11AE_Divq_X : OpCode, IOpCode
    {
        private static IOpCode DivByZero = new DivByZero();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            short denominator = (short)cpu.MemRead16(address);

            if (denominator == 0)
            {
                DivByZero.Exec(cpu); //TODO: The cpu cycles for this should be much higher than the 4 returned

                return 4;
            }

            int numerator = (int)cpu.Q_REG;
            int result = numerator / denominator;

            if (result > 65535 || result < -65536) //Abort
            {
                cpu.CC_V = true;
                cpu.CC_N = false;
                cpu.CC_Z = false;
                cpu.CC_C = false;

                return Cycles._3635 - 21;
            }

            ushort remainder = (ushort)(numerator % denominator);

            cpu.D_REG = remainder;
            cpu.W_REG = (ushort)result;

            if (result > 32767 || result < -32768)
            {
                cpu.CC_V = true;
                cpu.CC_N = true;
            }
            else
            {
                cpu.CC_Z = ZTEST(cpu.W_REG);
                cpu.CC_N = NTEST16(cpu.W_REG);
                cpu.CC_V = false;
            }

            cpu.CC_C = (cpu.B_REG & 1) != 0;

            return Cycles._3635;
        }
    }
}
