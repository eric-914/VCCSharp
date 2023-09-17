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
    public ISystemState _system { get; set; } = default!;

    private IState _state => _system.State;

    public void Firq()
    {
        if (!_state.CC.BitF())
        {
            //_isInFastInterrupt = true; //Flag to indicate FIRQ has been asserted

            _state.CC.BitE(false); // Turn E flag off

            PushStack(CPUInterrupts.FIRQ);

            _state.CC.BitI(true);
            _state.CC.BitF(true);

            _state.PC = _state.MemRead16(Define.VFIRQ);
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

        if (!_state.CC.BitI())
        {
            _state.CC.BitE(true);

            PushStack(CPUInterrupts.IRQ);

            _state.PC = _state.MemRead16(Define.VIRQ);

            //TODO: This doesn't look right compared to others.
            _state.CC.BitI(true);
        }

        //_pendingInterrupts &= ~_masks[CPUInterrupts.IRQ];
    }

    public void Nmi()
    {
        _state.CC.BitE(true);

        PushStack(CPUInterrupts.NMI);

        _state.CC.BitI(true);
        _state.CC.BitF(true);

        _state.PC = _state.MemRead16(Define.VNMI);

        //_pendingInterrupts &= ~_masks[CPUInterrupts.NMI];
    }

    private void PushStack(CPUInterrupts irq)
    {
        void W8(byte data) => _state.MemWrite8(data, --_state.S);
        void W16(ushort data) { W8((byte)data); W8((byte)(data >> 8)); };

        W16(_state.PC);

        if (irq != CPUInterrupts.FIRQ)
        {
            W16(_state.U);
            W16(_state.Y);
            W16(_state.X);
            W8(_state.DP);
            W16(_state.D);
        }

        W8(_state.CC);
    }
}
