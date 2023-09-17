using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.MC6809;

namespace VCCSharp.OpCodes.Model;

public interface IInterruptHandlers
{
    /// <summary>
    /// Non-Maskable Interrupt
    /// </summary>
    void Nmi();

    /// <summary>
    /// Fast Interrupt Request
    /// </summary>
    void Firq();

    /// <summary>
    /// Interrupt Request
    /// </summary>
    void Irq();
}

/// <summary>
/// The interrupt handlers aren't true OpCodes, but they behave as one.
/// So consider them meta-opcodes.
/// </summary>
internal class InterruptHandlers : IInterruptHandlers
{
    public ISystemState SS { get; set; } = default!;

    private IState cpu => SS.cpu;

    public void Firq()
    {
        if (!cpu.CC.BitF())
        {
            //_isInFastInterrupt = true; //Flag to indicate FIRQ has been asserted

            cpu.CC.BitE(false); // Turn E flag off

            PushStack(CPUInterrupts.FIRQ);

            cpu.CC.BitI(true);
            cpu.CC.BitF(true);

            cpu.PC = cpu.MemRead16(Define.VFIRQ);
        }

        //_pendingInterrupts &= ~_masks[CPUInterrupts.FIRQ];
    }

    public void Irq()
    {
        //// This is needed to fix a subtle timing problem.  Wait one cycle and it allows the CPU to see $FF03 bit 7 high before the IRQ is asserted.
        //if (_irqWait)
        //{
        //    _irqWait = false;
        //    return;
        //}

        //if (_isInFastInterrupt)
        //{
        //    //If FIRQ is running postpone the IRQ
        //    return;
        //}

        if (!cpu.CC.BitI())
        {
            cpu.CC.BitE(true);

            PushStack(CPUInterrupts.IRQ);

            cpu.PC = cpu.MemRead16(Define.VIRQ);

            //TODO: This doesn't look right compared to others.
            cpu.CC.BitI(true);
        }

        //_pendingInterrupts &= ~_masks[CPUInterrupts.IRQ];
    }

    public void Nmi()
    {
        cpu.CC.BitE(true);

        PushStack(CPUInterrupts.NMI);

        cpu.CC.BitI(true);
        cpu.CC.BitF(true);

        cpu.PC = cpu.MemRead16(Define.VNMI);

        //_pendingInterrupts &= ~_masks[CPUInterrupts.NMI];
    }

    private void PushStack(CPUInterrupts irq)
    {
        void W8(byte data) => cpu.MemWrite8(data, --cpu.S);
        void W16(ushort data) { W8((byte)data); W8((byte)(data >> 8)); };

        W16(cpu.PC);

        if (irq != CPUInterrupts.FIRQ)
        {
            W16(cpu.U);
            W16(cpu.Y);
            W16(cpu.X);
            W8(cpu.DP);
            W16(cpu.D);
        }

        W8(cpu.CC);
    }
}
