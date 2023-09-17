using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.HD6309;

partial class HD6309
{
    private void PushStack(CPUInterrupts irq)
    {
        void W8(byte data) => _modules.TCC1014.MemWrite8(data, --S);
        void W16(ushort data) { W8((byte)data); W8((byte)(data >> 8)); };

        W16(PC);

        if (irq != CPUInterrupts.FIRQ)
        {
            W16(U);
            W16(Y);
            W16(X);
            W8(DP);

            if (_cpu.md.NATIVE6309)
            {
                W16(W);
            }

            W16(D);
        }

        W8(CC);
    }
}
