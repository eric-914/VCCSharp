using VCCSharp.OpCodes.HD6309;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.Support;

internal interface ITransferMemory : IRegisterPC, IRegisterW
{
    Memory8Bit M8 { get; }
    IRegisters16Bit R16 { get; }
    Exceptions Exceptions { get; }

    void Swap(byte source, byte destination);
}

internal class TransferMemory
{
    public int CycleCount = 6;

    public int Exec(ITransferMemory tm)
    {
        if (tm.W == 0)
        {
            tm.PC++;

            return CycleCount;
        }

        byte value = tm.M8[tm.PC];

        byte source = (byte)(value >> 4);
        byte destination = (byte)(value & 0x0F);

        if (source > 4 || destination > 4)
        {
            return tm.Exceptions.IllegalInstruction();
        }

        byte mask = tm.M8[tm.R16[source]];

        tm.M8[tm.R16[destination]] = mask;

        tm.Swap(source, destination);

        tm.W--;

        //SNEAKY!!!  Force the CPU to call the same OpCode over and over until done.
        tm.PC -= 2;

        return 3;
    }
}
