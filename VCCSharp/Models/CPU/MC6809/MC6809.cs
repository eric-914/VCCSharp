using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.CPU.MC6809.Registers;

namespace VCCSharp.Models.CPU.MC6809;

// ReSharper disable once InconsistentNaming
public interface IMC6809 : ICpuProcessor { }

// ReSharper disable once InconsistentNaming
public partial class MC6809 : IMC6809
{
    private readonly IModules _modules;

    private readonly MC6809CpuRegisters _cpu = new();

    private byte _inInterrupt;
    private int _cycleCounter;
    private uint _syncWaiting;
    private int _gCycleFor;

    //--Interrupt states
    private byte _irqWaiter;
    private byte _pendingInterrupts;

    public MC6809(IModules modules)
    {
        _modules = modules;

        InitializeJmpVectors();
    }

    public void Init()
    {
    }

    public void ForcePc(ushort address)
    {
        _cpu.pc.Reg = address;

        _pendingInterrupts = 0;
        _syncWaiting = 0;
    }

    public void DeAssertInterrupt(byte irq)
    {
        _pendingInterrupts &= (byte)~(1 << (irq - 1));
        _inInterrupt = 0;
    }

    public void AssertInterrupt(byte irq, byte flag)
    {
        _syncWaiting = 0;
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

        _syncWaiting = 0;

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

            if (_syncWaiting == 1)
            {
                //Abort the run nothing happens asynchronously from the CPU
                // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                break;
            }

            _gCycleFor = cycleFor;

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

        _modules.TCC1014.MemWrite8(_cpu.d.lsb, --_cpu.s.Reg);
        _modules.TCC1014.MemWrite8(_cpu.d.msb, --_cpu.s.Reg);

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
            _inInterrupt = 1; //Flag to indicate FIRQ has been asserted

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
        if (_inInterrupt == 1)
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
            _modules.TCC1014.MemWrite8(_cpu.d.lsb, --_cpu.s.Reg);
            _modules.TCC1014.MemWrite8(_cpu.d.msb, --_cpu.s.Reg);

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

    public ushort CalculateEA(byte postByte)
    {
        ushort ea = 0;

        byte reg = (byte)(((postByte >> 5) & 3) + 1);

        if ((postByte & 0x80) != 0)
        {
            switch (postByte & 0x1F)
            {
                case 0:
                    ea = PXF(reg);
                    PXF(reg, (ushort)(PXF(reg) + 1));
                    _cycleCounter += 2;
                    break;

                case 1:
                    ea = PXF(reg);
                    PXF(reg, (ushort)(PXF(reg) + 2));
                    _cycleCounter += 3;
                    break;

                case 2:
                    PXF(reg, (ushort)(PXF(reg) - 1));
                    ea = PXF(reg);
                    _cycleCounter += 2;
                    break;

                case 3:
                    PXF(reg, (ushort)(PXF(reg) - 2));
                    ea = PXF(reg);
                    _cycleCounter += 3;
                    break;

                case 4:
                    ea = PXF(reg);
                    break;

                case 5:
                    ea = (ushort)(PXF(reg) + ((sbyte)B_REG));
                    _cycleCounter += 1;
                    break;

                case 6:
                    ea = (ushort)(PXF(reg) + ((sbyte)A_REG));
                    _cycleCounter += 1;
                    break;

                case 7:
                    //ea = (ushort)(PXF(reg) + ((sbyte)E_REG));
                    _cycleCounter += 1;
                    break;

                case 8:
                    ea = (ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++));
                    _cycleCounter += 1;
                    break;

                case 9:
                    ea = (ushort)(PXF(reg) + MemRead16(PC_REG));
                    _cycleCounter += 4;
                    PC_REG += 2;
                    break;

                case 10:
                    //ea = (ushort)(PXF(reg) + ((sbyte)F_REG));
                    _cycleCounter += 1;
                    break;

                case 11:
                    ea = (ushort)(PXF(reg) + D_REG);
                    _cycleCounter += 4;
                    break;

                case 12:
                    ea = (ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1);
                    _cycleCounter += 1;
                    PC_REG++;
                    break;

                case 13: //MM
                    ea = (ushort)(PC_REG + MemRead16(PC_REG) + 2);
                    _cycleCounter += 5;
                    PC_REG += 2;
                    break;

                case 14:
                    //ea = (ushort)(PXF(reg) + W_REG);
                    _cycleCounter += 4;
                    break;

                case 15: //01111
                    sbyte signedByte = (sbyte)((postByte >> 5) & 3);

                    switch (signedByte)
                    {
                        case 0:
                            //ea = W_REG;
                            break;

                        case 1:
                            //ea = (ushort)(W_REG + MemRead16(PC_REG));
                            PC_REG += 2;
                            //_cycleCounter += 2;
                            break;

                        case 2:
                            //ea = W_REG;
                            break;

                        case 3:
                            //W_REG -= 2;
                            break;
                    }
                    break;

                case 16: //10000
                    signedByte = (sbyte)((postByte >> 5) & 3);

                    switch (signedByte)
                    {
                        case 0:
                            //ea = MemRead16(W_REG);
                            break;

                        case 1:
                            //ea = MemRead16((ushort)(W_REG + MemRead16(PC_REG)));
                            PC_REG += 2;
                            //_cycleCounter += 5;
                            break;

                        case 2:
                            //ea = MemRead16(W_REG);
                            break;

                        case 3:
                            //W_REG -= 2;
                            break;
                    }
                    break;

                case 17: //10001
                    ea = PXF(reg);
                    PXF(reg, (ushort)(PXF(reg) + 2));
                    ea = MemRead16(ea);
                    _cycleCounter += 6;
                    break;

                case 18: //10010
                    _cycleCounter += 6;
                    break;

                case 19: //10011
                    PXF(reg, (ushort)(PXF(reg) - 2));
                    ea = MemRead16(PXF(reg));
                    _cycleCounter += 6;
                    break;

                case 20: //10100
                    ea = MemRead16(PXF(reg));
                    _cycleCounter += 3;
                    break;

                case 21: //10101
                    ea = MemRead16((ushort)(PXF(reg) + ((sbyte)B_REG)));
                    _cycleCounter += 4;
                    break;

                case 22: //10110
                    ea = MemRead16((ushort)(PXF(reg) + ((sbyte)A_REG)));
                    _cycleCounter += 4;
                    break;

                case 23: //10111
                    //ea = MemRead16((ushort)(PXF(reg) + ((sbyte)E_REG)));
                    ea = MemRead16(ea);
                    _cycleCounter += 4;
                    break;

                case 24: //11000
                    ea = MemRead16((ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++)));
                    _cycleCounter += 4;
                    break;

                case 25: //11001
                    ea = MemRead16((ushort)(PXF(reg) + MemRead16(PC_REG)));
                    _cycleCounter += 7;
                    PC_REG += 2;
                    break;

                case 26: //11010
                    //ea = MemRead16((ushort)(PXF(reg) + ((sbyte)F_REG)));
                    ea = MemRead16(ea);
                    _cycleCounter += 4;
                    break;

                case 27: //11011
                    ea = MemRead16((ushort)(PXF(reg) + D_REG));
                    _cycleCounter += 7;
                    break;

                case 28: //11100
                    ea = MemRead16((ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1));
                    _cycleCounter += 4;
                    PC_REG++;
                    break;

                case 29: //11101
                    ea = MemRead16((ushort)(PC_REG + MemRead16(PC_REG) + 2));
                    _cycleCounter += 8;
                    PC_REG += 2;
                    break;

                case 30: //11110
                    //ea = MemRead16((ushort)(PXF(reg) + W_REG));
                    ea = MemRead16(ea);
                    _cycleCounter += 7;
                    break;

                case 31: //11111
                    ea = MemRead16(MemRead16(PC_REG));
                    _cycleCounter += 8;
                    PC_REG += 2;
                    break;
            }
        }
        else
        {
            sbyte signedByte = (sbyte)(postByte & 0x1F);
            signedByte <<= 3; //--Push the "sign" to the left-most bit.
            signedByte /= 8;

            ea = (ushort)(PXF(reg) + signedByte); //Was signed

            _cycleCounter += 1;
        }

        return ea;
    }
}
