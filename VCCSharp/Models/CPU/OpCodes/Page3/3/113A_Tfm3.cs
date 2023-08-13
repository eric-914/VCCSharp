using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    public class _113A_Tfm3 : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new UndefinedOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            if (cpu.W_REG == 0)
            {
                cpu.PC_REG++;

                return 6;
            }

            var _postByte = cpu.MemRead8(cpu.PC_REG);

            var _source = (byte)(_postByte >> 4);
            var _dest = (byte)(_postByte & 15);

            if (_source > 4 || _dest > 4)
            {
                return InvalidOpCode.Exec(cpu);
            }

            var _temp8 = cpu.MemRead8(cpu.PXF(_source));
            
            cpu.MemWrite8(_temp8, cpu.PXF(_dest));
            
            cpu.PXF(_source, (ushort)(cpu.PXF(_source) + 1));
            
            cpu.W_REG--;

            cpu.PC_REG -= 2; //Hit the same instruction on the next loop if not done copying

            return 3; //TODO: Too low?
        }
    }
}
