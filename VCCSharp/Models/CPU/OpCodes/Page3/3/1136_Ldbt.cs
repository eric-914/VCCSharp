using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    public class _1136_Ldbt : OpCode, IOpCode
    {
        private static readonly IOpCode InvalidOpCode = new InvalidOpCode();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);
            byte _temp8 = cpu.MemRead8(cpu.DPADDRESS(cpu.PC_REG++));

            byte _source = (byte)((_postByte >> 3) & 7);
            byte _dest = (byte)(_postByte & 7);

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
                        cpu.OUR(_postByte, _dest);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC | (1 << _dest));
                        break;
                }
            }
            else
            {
                switch (_postByte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        cpu.AUR(_postByte, _dest);
                        break;

                    case 2: // CC Reg
                        cpu.CC = (byte)(cpu.CC & ~(1 << _dest));
                        break;
                }
            }

            // Else nothing changes
            return Cycles._76;
        }
    }
}
