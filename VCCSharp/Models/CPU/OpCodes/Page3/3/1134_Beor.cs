using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    public class _1134_Beor : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var _postByte = cpu.MemRead8(cpu.PC_REG++);
            var _temp8 = cpu.MemRead8(cpu.DPADDRESS(cpu.PC_REG++));

            var _source = (byte)((_postByte >> 3) & 7);
            var _dest = (byte)(_postByte & 7);

            _postByte >>= 6;

            if (_postByte == 3)
            {
                return InvalidOpCode.Exec(cpu);
            }

            if ((_temp8 & (1 << _source)) != 0)
            {
                switch (_postByte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.XUR(_postByte, _dest);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC ^ (1 << _dest));
                        break;
                }
            }

            // Else nothing changes
            return Cycles._76;
        }
    }
}
