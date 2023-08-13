using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    public class _1138_Tfm1 : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            if (cpu.W_REG == 0)
            {
                cpu.PC_REG++;

                return 6;
            }

            var value = cpu.MemRead8(cpu.PC_REG);

            var source = (byte)(value >> 4);
            var destination = (byte)(value & 15);

            if (source > 4 || destination > 4)
            {
                return InvalidOpCode.Exec(cpu);
            }

            var mask = cpu.MemRead8(cpu.PXF(source));

            cpu.MemWrite8(mask, cpu.PXF(destination));

            cpu.PXF(destination, (ushort)(cpu.PXF(destination) + 1));
            cpu.PXF(source, (ushort)(cpu.PXF(source) + 1));

            cpu.W_REG--;
            cpu.PC_REG -= 2;

            return 3; //TODO: Too low?
        }
    }
}
