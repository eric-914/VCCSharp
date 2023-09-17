using VCCSharp.IoC;
using VCCSharp.Modules.TCC1014.Masks;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Modules.TCC1014;
// ReSharper disable InconsistentNaming

public interface IGIME
{
    byte[] Registers { get; }
    IInterrupts Interrupts { get; }
    IMMU MMU { get; }
    IVDG VDG { get; }

    void Initialize();
    byte ReadIRQ();
    byte ReadFIRQ();
    void LoadRom();

    void AssertKeyboardInterrupt();
    void AssertVerticalInterrupt();
    void AssertHorizontalInterrupt();
    void AssertTimerInterrupt();

    void SetMapTypeRam();
    void SetMapTypeRom(RomMapping romMap);
}

/// <summary>
/// Taking the place of the graphics and memory hardware in the CoCo 1 and 2 is an application-specific
/// integrated circuit called the GIME (Graphics Interrupt Memory Enhancement) chip.
///
/// Gone were the 6847 VDG and 6883 SAM, functionally replaced and enhanced with the TCC1014 Advanced Color Video Chip (ACVC)
/// which most folks reference as the GIME.
/// </summary>
public class GIME : IGIME
{
    private readonly IModules _modules;
    private readonly IRomLoader _romLoader;

    public byte[] Registers { get; } = new byte[256];

    public IInterrupts Interrupts { get; }
    public IMMU MMU { get; }
    public IVDG VDG { get; }

    public GIME(IModules modules, IRomLoader romLoader, IVDG vdg, IMMU mmu, IInterrupts interrupts)
    {
        _modules = modules;
        _romLoader = romLoader;

        VDG = vdg;
        MMU = mmu;
        Interrupts = interrupts;
    }

    public void Initialize()
    {
        Registers.Initialize();
        MMU.Initialize();
        Interrupts.FIRQ.Reset();
        Interrupts.IRQ.Reset();
        VDG.Reset();
    }

    public byte ReadIRQ()
    {
        InterruptFlags temp = Interrupts.IRQ.Last;
        Interrupts.IRQ.Last = 0;

        return (byte)temp;
    }

    public byte ReadFIRQ()
    {
        InterruptFlags temp = Interrupts.FIRQ.Last;
        Interrupts.FIRQ.Last = 0;

        return (byte)temp;
    }

    public void LoadRom()
    {
        MMU.ROM.Reset(MMU.IRB);

        _romLoader.CopyRom(MMU.IRB);
    }

    private void AssertInterrupt(CPUInterrupts interrupt, byte flag)
    {
        _modules.CPU.AssertInterrupt(interrupt, flag);
    }

    public void AssertKeyboardInterrupt()
    {
        if ((Registers[Ports.FIRQENR] & 2) != 0 && (Interrupts.FIRQ.EnhancedFlag))
        {
            AssertInterrupt(CPUInterrupts.FIRQ, 0);

            Interrupts.FIRQ.Last |= InterruptFlags.Keyboard;
        }
        else if ((Registers[Ports.IRQENR] & 2) != 0 && (Interrupts.IRQ.EnhancedFlag))
        {
            AssertInterrupt(CPUInterrupts.IRQ, 0);

            Interrupts.IRQ.Last |= InterruptFlags.Keyboard;
        }
    }

    public void AssertVerticalInterrupt()
    {
        if ((Registers[Ports.FIRQENR] & 8) != 0 && Interrupts.FIRQ.EnhancedFlag)
        {
            AssertInterrupt(CPUInterrupts.FIRQ, 0); //FIRQ

            Interrupts.FIRQ.Last |= InterruptFlags.Vertical;
        }
        else if ((Registers[Ports.IRQENR] & 8) != 0 && Interrupts.IRQ.EnhancedFlag)
        {
            AssertInterrupt(CPUInterrupts.IRQ, 0); //IRQ moon patrol demo using this

            Interrupts.IRQ.Last |= InterruptFlags.Vertical;
        }
    }

    public void AssertHorizontalInterrupt()
    {
        if ((Registers[Ports.FIRQENR] & 16) != 0 && Interrupts.FIRQ.EnhancedFlag)
        {
            AssertInterrupt(CPUInterrupts.FIRQ, 0);

            Interrupts.FIRQ.Last |= InterruptFlags.Horizontal;
        }
        else if ((Registers[Ports.IRQENR] & 16) != 0 && Interrupts.IRQ.EnhancedFlag)
        {
            AssertInterrupt(CPUInterrupts.IRQ, 0);

            Interrupts.IRQ.Last |= InterruptFlags.Horizontal;
        }
    }

    public void AssertTimerInterrupt()
    {
        if ((Registers[Ports.FIRQENR] & 32) != 0 && Interrupts.FIRQ.EnhancedFlag)
        {
            AssertInterrupt(CPUInterrupts.FIRQ, 0);

            Interrupts.FIRQ.Last |= InterruptFlags.Timer;
        }
        else if ((Registers[Ports.IRQENR] & 32) != 0 && Interrupts.IRQ.EnhancedFlag)
        {
            AssertInterrupt(CPUInterrupts.IRQ, 0);

            Interrupts.IRQ.Last |= InterruptFlags.Timer;
        }
    }

    private readonly VectorMasks _vectorMask = new();

    public void SetMapTypeRam()
    {
        MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 3] = MMU.RAM.GetBytePointer(0x2000 * (_vectorMask[MMU.CurrentRamConfiguration] - 3));
        MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 2] = MMU.RAM.GetBytePointer(0x2000 * (_vectorMask[MMU.CurrentRamConfiguration] - 2));
        MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 1] = MMU.RAM.GetBytePointer(0x2000 * (_vectorMask[MMU.CurrentRamConfiguration] - 1));
        MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration]] = MMU.RAM.GetBytePointer(0x2000 * _vectorMask[MMU.CurrentRamConfiguration]);

        MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 3] = 1;
        MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 2] = 1;
        MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 1] = 1;
        MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration]] = 1;
    }

    public void SetMapTypeRom(RomMapping romMap)
    {
        switch (romMap)
        {
            case RomMapping._16kInternal_16kExternal:
            case RomMapping._16kInternal_16kExternal_Alt: //16K Internal 16K External
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 3] = MMU.IRB.GetBytePointer(0x0000);
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 2] = MMU.IRB.GetBytePointer(0x2000);
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 1] = new BytePointer();
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration]] = new BytePointer();

                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 3] = 1;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 2] = 1;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 1] = 0;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration]] = 0x2000;

                return;

            case RomMapping._32kInternal: // 32K Internal
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 3] = MMU.IRB.GetBytePointer(0x0000);
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 2] = MMU.IRB.GetBytePointer(0x2000);
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 1] = MMU.IRB.GetBytePointer(0x4000);
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration]] = MMU.IRB.GetBytePointer(0x6000);

                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 3] = 1;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 2] = 1;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 1] = 1;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration]] = 1;

                return;

            case RomMapping._32kExternal: //32K External
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 1] = new BytePointer();
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration]] = new BytePointer();
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 3] = new BytePointer();
                MMU.Pages[_vectorMask[MMU.CurrentRamConfiguration] - 2] = new BytePointer();

                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 1] = 0;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration]] = 0x2000;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 3] = 0x4000;
                MMU.PageOffsets[_vectorMask[MMU.CurrentRamConfiguration] - 2] = 0x6000;

                return;
        }
    }
}
