using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.HD6309.OpCodes;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes
{
    public interface IOpCode
    {
        int Exec(IMC6809 cpu);
        int Exec(IHD6309 cpu);
    }

    public abstract class OpCode
    {
        protected static readonly HD6309NatEmuCycles Cycles = new();

        protected static bool NTEST8(byte value) => value > 0x7F;
        protected static bool NTEST16(ushort value) => value > 0x7FFF;
        protected static bool NTEST32(uint value) => value > 0x7FFFFFFF;

        protected static bool ZTEST(byte value) => value == 0;
        protected static bool ZTEST(ushort value) => value == 0;
        protected static bool ZTEST(uint value) => value == 0;
    }

    public class UndefinedOpCode : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();
        public int Exec(IHD6309 cpu) => throw new NotImplementedException();
    }
}
