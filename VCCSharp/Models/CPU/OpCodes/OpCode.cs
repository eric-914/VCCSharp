using System.Security.Policy;
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

        protected static bool OVERFLOW8(bool c, byte a, ushort b, byte r) => ((c ? 1 : 0) ^ (((a ^ b ^ r) >> 7) & 1)) != 0;
        protected static bool OVERFLOW16(bool c, uint a, ushort b, ushort r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 15) & 1)) != 0;
    }

    public class UndefinedOpCode : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();
        public int Exec(IHD6309 cpu) => throw new NotImplementedException();
    }

    //--Apparently Hitachi has an error handlers
    public abstract class ErrorOpCode : UndefinedOpCode, IOpCode
    {
        protected int ErrorVector(IHD6309 cpu, int cycles)
        {
            cpu.CC_E = true;

            cpu.MemWrite8(cpu.PC_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.PC_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.U_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.U_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.Y_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.Y_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.X_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.X_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.DPA, --cpu.S_REG);

            if (cpu.MD_NATIVE6309)
            {
                cpu.MemWrite8(cpu.F_REG, --cpu.S_REG);
                cpu.MemWrite8(cpu.E_REG, --cpu.S_REG);

                cycles += 2;
            }

            cpu.MemWrite8(cpu.B_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.A_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.CC, --cpu.S_REG);

            cpu.PC_REG = cpu.MemRead16(Define.VTRAP);

            cycles += 12 + Cycles._54;  //One for each byte +overhead? Guessing from PSHS

            return cycles;
        }
    }

    public class InvalidOpCode : ErrorOpCode, IOpCode
    {
        int IOpCode.Exec(IHD6309 cpu)
        {
            cpu.MD_ILLEGAL = true;

            return ErrorVector(cpu, 0);
        }
    }

    public class DivByZero : ErrorOpCode, IOpCode
    {
        int IOpCode.Exec(IHD6309 cpu)
        {
            cpu.MD_ZERODIV = true;

            return ErrorVector(cpu, 0);
        }
    }
}
