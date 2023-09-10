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
    private byte _irqWaiter;
    private byte _pendingInterrupts;

    private bool _isInFastInterrupt;
    private bool _isSyncWaiting;
    private int _syncCycle;

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
        _cpu.pc.Reg = address;

        _pendingInterrupts = 0;
        _isSyncWaiting = false;
    }

    public void DeAssertInterrupt(byte irq)
    {
        _pendingInterrupts &= (byte)~(1 << (irq - 1));
        _isInFastInterrupt = false;
    }

    public void AssertInterrupt(byte irq, byte flag)
    {
        _isSyncWaiting = false;
        _pendingInterrupts |= (byte)(1 << (irq - 1));
        _irqWaiter = flag;
    }

    public void Reset()
    {
        CC = 0b01010000;
        MD = 0b00000000;

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
        if (_pendingInterrupts != 0)
        {
            if ((_pendingInterrupts & 4) != 0)
            {
                Cpu_Nmi();
            }

            if ((_pendingInterrupts & 2) != 0)
            {
                Cpu_Firq();
            }

            if ((_pendingInterrupts & 1) != 0)
            {
                if (_irqWaiter == 0)
                {
                    // This is needed to fix a subtle timing problem
                    // It allows the CPU to see $FF03 bit 7 high before...
                    Cpu_Irq();
                }
                else
                {
                    // ...The IRQ is asserted.
                    _irqWaiter -= 1;
                }
            }
        }
    }

    public void Cpu_Nmi()
    {
        _cpu.cc.E = true;

        PushStack(CPUInterrupts.NMI);

        _cpu.cc.I = true;
        _cpu.cc.F = true;

        _cpu.pc.Reg = MemRead16(Define.VNMI);

        _pendingInterrupts &= 251;
    }

    public void Cpu_Firq()
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

                    _cpu.pc.Reg = MemRead16(Define.VFIRQ);

                    break;

                case true:		//6309
                    _cpu.cc.E = true;

                    PushStack(CPUInterrupts.IRQ);

                    _cpu.cc.I = true;
                    _cpu.cc.F = true;

                    _cpu.pc.Reg = MemRead16(Define.VFIRQ);

                    break;
            }
        }

        _pendingInterrupts &= 253;
    }

    private void Cpu_Irq()
    {
        if (_isInFastInterrupt)
        {
            //If FIRQ is running postpone the IRQ
            return;
        }

        if (!_cpu.cc.I)
        {
            _cpu.cc.E = true;

            PushStack(CPUInterrupts.IRQ);

            _cpu.pc.Reg = MemRead16(Define.VIRQ);

            //TODO: This doesn't look right compared to above.
            _cpu.cc.I = true;
        }

        _pendingInterrupts &= 254;
    }

    private void PushStack(CPUInterrupts irq)
    {
        void W8(byte data) => _modules.TCC1014.MemWrite8(data, --_cpu.s.Reg);
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
