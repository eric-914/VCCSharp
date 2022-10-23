namespace VCCSharp.Modules.TCC1014;

/// <summary>
/// Taking the place of the graphics and memory hardware in the CoCo 1 and 2 is an application-specific
/// integrated circuit called the GIME (Graphics Interrupt Memory Enhancement) chip.
///
/// Gone were the 6847 VDG and 6883 SAM, functionally replaced and enhanced with the TCC1014 Advanced Color Video Chip (ACVC)
/// which most folks reference as the GIME.
/// </summary>
// ReSharper disable once InconsistentNaming
public class GIME
{
    public byte[] Registers { get; } = new byte[256];

    public Interrupts Interrupts { get; } = new();

    public void Initialize()
    {
        Registers.Initialize();
        Interrupts.FIRQ.Reset();
        Interrupts.IRQ.Reset();
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
}
