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
}
