using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.MC6809;

partial class MC6809
{
    private void CheckInterrupts()
    {
        if (_nmiFlag) Nmi();
        if (_firqFlag) Firq();
        if (_irqFlag) Irq();
    }

    private void Nmi()
    {
        _cpu.cc.E = true;

        PushStack(CPUInterrupts.NMI);

        _cpu.cc.I = true;
        _cpu.cc.F = true;

        PC = MemRead16(Define.VNMI);

        _nmiFlag = false;
    }

    private void Firq()
    {
        if (!_cpu.cc.F)
        {
            _isInFastInterrupt = true; //Flag to indicate FIRQ has been asserted

            _cpu.cc.E = false; // Turn E flag off

            PushStack(CPUInterrupts.FIRQ);

            _cpu.cc.I = true;
            _cpu.cc.F = true;

            PC = MemRead16(Define.VFIRQ);
        }

        _firqFlag = false;
    }

    private void Irq()
    {
        // This is needed to fix a subtle timing problem.  Wait one cycle and it allows the CPU to see $FF03 bit 7 high before the IRQ is asserted.
        if (_irqWait)
        {
            _irqWait = false;
            return;
        }

        if (_isInFastInterrupt)
        {
            //If FIRQ is running postpone the IRQ
            return;
        }

        if (!_cpu.cc.I)
        {
            _cpu.cc.E = true;

            PushStack(CPUInterrupts.IRQ);

            PC = MemRead16(Define.VIRQ);

            //TODO: This doesn't look right compared to above.
            _cpu.cc.I = true;
        }

        _irqFlag = false;
    }
}
