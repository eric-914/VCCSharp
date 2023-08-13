using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.B
{
    public class _11BD_Divd_E : OpCode, IOpCode
    {
        private static IOpCode DivByZero = new DivByZero();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            sbyte denominator = (sbyte)cpu.MemRead8(address);

            cpu.PC_REG += 2;

            if (denominator == 0)
            {
                DivByZero.Exec(cpu); //TODO: The cpu cycles for this should be much higher than the 3 returned

                return 3;
            }

            short numerator = (short)cpu.D_REG;
            short result = (short)(numerator / denominator);

            if (result > 255 || result < -256) //Abort
            {
                cpu.CC_V = true;
                cpu.CC_N = false;
                cpu.CC_Z = false;
                cpu.CC_C = false;

                return 17;
            }

            byte remainder = (byte)(numerator % denominator);

            cpu.A_REG = remainder;
            cpu.B_REG = (byte)result;

            if (result > 127 || result < -128)
            {
                cpu.CC_V = true;
                cpu.CC_N = true;
            }
            else
            {
                cpu.CC_Z = ZTEST(cpu.B_REG);
                cpu.CC_N = NTEST8(cpu.B_REG);
                cpu.CC_V = false;
            }

            cpu.CC_C = (cpu.B_REG & 1) != 0;

            return 25;
        }
    }
}
