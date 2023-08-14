using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    // --[HITACHI]--
    //DIVQ
    //IMMEDIATE
    public class _118E_Divq_M : OpCode, IOpCode
    {
        private static IOpCode DivByZero = new DivByZero();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            short denominator = (short)cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;

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

                return 13; //34 - 21
            }

            int remainder = numerator % denominator;

            cpu.D_REG = (ushort)remainder;
            cpu.W_REG = (ushort)result;

            //TODO: Originally "result" was "_signedShort" which doesn't make sense.
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

            return 34;
        }
    }
}
