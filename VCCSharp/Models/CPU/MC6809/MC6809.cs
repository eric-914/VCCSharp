using VCCSharp.IoC;
using VCCSharp.Models.CPU.Registers;
using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.MC6809;

public interface IMC6809 : IProcessor { }

public partial class MC6809 : IMC6809
{
    private readonly IModules _modules;

    internal Register16 pc { get; } = new();
    internal Register16 d { get; } = new();
    internal Register16 x { get; } = new();
    internal Register16 y { get; } = new();
    internal Register16 u { get; } = new();
    internal Register16 s { get; } = new();
    internal Register16 dp { get; } = new();

    internal RegisterCC cc { get; } = new();

    private int _cycleCounter;

    //--Interrupt states
    private bool _irqWait;
    private bool _nmiFlag;
    private bool _irqFlag;
    private bool _firqFlag;
    private bool _isInFastInterrupt;
    private bool _isSyncWaiting;
    private int _syncCycle;

    public MC6809(IModules modules)
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

        _nmiFlag = false;
        _irqFlag = false;
        _firqFlag = false;
        _isSyncWaiting = false;
    }

    public void DeAssertInterrupt(CPUInterrupts irq)
    {
        switch(irq)
        {
            case CPUInterrupts.NMI: _nmiFlag = false; break;
            case CPUInterrupts.IRQ: _irqFlag = false; break;
            case CPUInterrupts.FIRQ: _firqFlag = false; break;
        }
        _isInFastInterrupt = false;
    }

    public void AssertInterrupt(CPUInterrupts irq, byte flag)
    {
        switch(irq)
        {
            case CPUInterrupts.NMI: _nmiFlag = true; break;
            case CPUInterrupts.IRQ: _irqFlag = true; break;
            case CPUInterrupts.FIRQ: _firqFlag = true; break;
        }
        _isSyncWaiting = false;
        _irqWait = flag != 0;
    }

    public void Reset()
    {
        CC = 0b01010000;

        _nmiFlag = false;
        _irqFlag = false;
        _firqFlag = false;
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

            byte opCode = _modules.TCC1014.MemRead8(pc.Reg++);

            _cycleCounter += OpCodes.Exec(opCode);
        }

        return cycleFor - _cycleCounter;
    }
}
