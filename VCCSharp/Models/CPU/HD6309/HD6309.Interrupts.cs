using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.HD6309;

partial class HD6309
{
    private void CheckInterrupts()
    {
        switch (_pendingInterrupts)
        {
            case 0b000:                         break;
            case 0b001:                 Irq();  break;
            case 0b010:         Firq();         break;
            case 0b011:         Firq(); Irq();  break;
            case 0b100: Nmi();                  break;
            case 0b101: Nmi();          Irq();  break;
            case 0b110: Nmi();  Firq();         break;
            case 0b111: Nmi();  Firq(); Irq();  break;
        }
    }

    private void Nmi()
    {
        _cpu.cc.E = true;

        PushStack(CPUInterrupts.NMI);

        _cpu.cc.I = true;
        _cpu.cc.F = true;

        PC = MemRead16(Define.VNMI);

        _pendingInterrupts &= ~_masks[CPUInterrupts.NMI];
    }

    private void Firq()
    {
        if (!_cpu.cc.F)
        {
            _isInFastInterrupt = true; //Flag to indicate FIRQ has been asserted

            switch (_cpu.md.FIRQMODE)
            {
                case false:
                    _cpu.cc.E = false; // Turn E flag off

                    PushStack(CPUInterrupts.FIRQ);

                    _cpu.cc.I = true;
                    _cpu.cc.F = true;

                    PC = MemRead16(Define.VFIRQ);

                    break;

                case true:		//6309
                    _cpu.cc.E = true;

                    PushStack(CPUInterrupts.IRQ);

                    _cpu.cc.I = true;
                    _cpu.cc.F = true;

                    PC = MemRead16(Define.VFIRQ);

                    break;
            }
        }

        _pendingInterrupts &= ~_masks[CPUInterrupts.FIRQ];
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

        _pendingInterrupts &= ~_masks[CPUInterrupts.IRQ];
    }
}
