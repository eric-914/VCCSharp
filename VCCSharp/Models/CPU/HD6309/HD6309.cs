using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.CPU.HD6309.Registers;
using VCCSharp.OpCodes;

namespace VCCSharp.Models.CPU.HD6309;

public interface IHD6309 : IProcessor { }

public partial class HD6309 : IHD6309
{
    private readonly IModules _modules;

    private readonly HD6309CpuRegisters _cpu = new();

    private int _cycleCounter;

    //--Interrupt states
    private bool _irqWait;
    private int _pendingInterrupts;

    private bool _isInFastInterrupt;
    private bool _isSyncWaiting;
    private int _syncCycle;

    private Dictionary<CPUInterrupts, byte> _masks = new()
    {
        { CPUInterrupts.IRQ, 0b001},
        { CPUInterrupts.FIRQ, 0b010},
        { CPUInterrupts.NMI, 0b100}
    };

    public HD6309(IModules modules)
    {
        _modules = modules;

        OpCodes = OpCodesFactory.Create(this);
    }

    public void Init()
    {
    }

    public void ForcePc(ushort address)
    {
        PC = address;

        _pendingInterrupts = 0;
        _isSyncWaiting = false;
    }

    public void DeAssertInterrupt(CPUInterrupts irq)
    {
        _pendingInterrupts &= _masks[irq];
        _isInFastInterrupt = false;
    }

    public void AssertInterrupt(CPUInterrupts irq, byte flag)
    {
        _isSyncWaiting = false;
        _pendingInterrupts |= _masks[irq];
        _irqWait = flag != 0;
    }

    public void Reset()
    {
        CC = 0b01010000; MD = 0b00000000;

        _isSyncWaiting = false;

        DP = 0;

        PC = MemRead16(Define.VRESET);	//PC gets its reset vector

        //_modules.TCC1014.SetMapType(false);	//shouldn't be here
    }

    public int Exec(int cycleFor)
    {
        _cycleCounter = 0;

        while (_cycleCounter < cycleFor)
        {
            CheckInterrupts();

            if (_isSyncWaiting)
            {
                //Abort the run nothing happens asynchronously from the CPU
                // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                break;
            }

            _syncCycle = cycleFor;

            byte opCode = _modules.TCC1014.MemRead8(_cpu.pc.Reg++);

            _cycleCounter += OpCodes.Exec(opCode);
        }

        return cycleFor - _cycleCounter;
    }

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
