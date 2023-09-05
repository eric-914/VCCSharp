using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.CPU.MC6809.Registers;
using VCCSharp.OpCodes;

namespace VCCSharp.Models.CPU.MC6809;

// ReSharper disable once InconsistentNaming
public interface IMC6809 : ICpuProcessor { }

// ReSharper disable once InconsistentNaming
public partial class MC6809 : IMC6809
{
    private readonly IModules _modules;

    private readonly MC6809CpuRegisters _cpu = new();

    private int _cycleCounter;

    //--Interrupt states
    private byte _irqWaiter;
    private byte _pendingInterrupts;

    public bool IsInInterrupt { get; set; }
    public bool IsSyncWaiting { get; set; }
    public int SyncCycle { get; set; }

    public MC6809(IModules modules)
    {
        _modules = modules;

        InitializeJmpVectors();

        OpCodes = OpCodesFactory.Create(this);
    }

    public void Init()
    {
    }

    public void ForcePc(ushort address)
    {
        _cpu.pc.Reg = address;

        _pendingInterrupts = 0;
        IsSyncWaiting = false;
    }

    public void DeAssertInterrupt(byte irq)
    {
        _pendingInterrupts &= (byte)~(1 << (irq - 1));
        IsInInterrupt = false;
    }

    public void AssertInterrupt(byte irq, byte flag)
    {
        IsSyncWaiting = false;
        _pendingInterrupts |= (byte)(1 << (irq - 1));
        _irqWaiter = flag;
    }

    public void Reset()
    {
        byte index;

        for (index = 0; index <= 5; index++)
        {
            PXF(index, 0);
        }

        for (index = 0; index <= 7; index++)
        {
            PUR(index, 0);
        }

        CC_E = false;
        CC_F = true;
        CC_H = false;
        CC_I = true;
        CC_N = false;
        CC_Z = false;
        CC_V = false;
        CC_C = false;

        DP_REG = 0;

        IsSyncWaiting = false;

        PC_REG = MemRead16(Define.VRESET);	//PC gets its reset vector

        //_modules.TCC1014.SetMapType(false);	//shouldn't be here
    }

    public int Exec(int cycleFor)
    {
        _cycleCounter = 0;

        while (_cycleCounter < cycleFor)
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

            if (IsSyncWaiting)
            {
                //Abort the run nothing happens asynchronously from the CPU
                // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                break;
            }

            SyncCycle = cycleFor;

            byte opCode = _modules.TCC1014.MemRead8(_cpu.pc.Reg++);   //PC_REG

            Exec(opCode);
        }

        return cycleFor - _cycleCounter;
    }

    public void Cpu_Nmi()
    {
        _cpu.cc.E = true;

        _modules.TCC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

        _modules.TCC1014.MemWrite8(_cpu.d.lsb, --_cpu.s.Reg); //B
        _modules.TCC1014.MemWrite8(_cpu.d.msb, --_cpu.s.Reg); //A

        _modules.TCC1014.MemWrite8(GetCC(), --_cpu.s.Reg);

        _cpu.cc.I = true;
        _cpu.cc.F = true;

        _cpu.pc.Reg = MemRead16(Define.VNMI);

        _pendingInterrupts &= 251;
    }

    public void Cpu_Firq()
    {
        if (!_cpu.cc.F)
        {
            IsInInterrupt = true; //Flag to indicate FIRQ has been asserted

            _cpu.cc.E = false; // Turn E flag off

            _modules.TCC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);

            _modules.TCC1014.MemWrite8(GetCC(), --_cpu.s.Reg);

            _cpu.cc.I = true;
            _cpu.cc.F = true;

            _cpu.pc.Reg = MemRead16(Define.VFIRQ);
        }

        _pendingInterrupts &= 253;
    }

    public void Cpu_Irq()
    {
        if (IsInInterrupt)
        {
            //If FIRQ is running postpone the IRQ
            return;
        }

        if (!_cpu.cc.I)
        {
            _cpu.cc.E = true;

            _modules.TCC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.d.lsb, --_cpu.s.Reg); //B
            _modules.TCC1014.MemWrite8(_cpu.d.msb, --_cpu.s.Reg); //A

            _modules.TCC1014.MemWrite8(GetCC(), --_cpu.s.Reg);

            _cpu.pc.Reg = MemRead16(Define.VIRQ);
            _cpu.cc.I = true;
        }

        _pendingInterrupts &= 254;
    }

    public byte GetCC()
    {
        int cc = 0;

        void Test(bool value, CCFlagMasks mask)
        {
            if (value) { cc |= (1 << (int)mask); }
        }

        Test(CC_E, CCFlagMasks.E);
        Test(CC_F, CCFlagMasks.F);
        Test(CC_H, CCFlagMasks.H);
        Test(CC_I, CCFlagMasks.I);
        Test(CC_N, CCFlagMasks.N);
        Test(CC_Z, CCFlagMasks.Z);
        Test(CC_V, CCFlagMasks.V);
        Test(CC_C, CCFlagMasks.C);

        return (byte)cc;
    }
}
