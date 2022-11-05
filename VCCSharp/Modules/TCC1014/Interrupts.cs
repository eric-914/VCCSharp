namespace VCCSharp.Modules.TCC1014;

[Flags]
public enum InterruptFlags : byte
{
    Keyboard = 0x02,
    Vertical = 0x08,
    Horizontal = 0x10,
    Timer = 0x20
}

public interface IInterrupts
{
    Interrupt FIRQ { get; }
    Interrupt IRQ { get; }
    void SetFlags(byte data);
}

/// <summary>
/// In CoCo2, the three interrupt sources CART*, HSYNC* and VSYNC*
/// requested an interrupt to the CPU from PIA as IRQ* and FIRQ*.
/// </summary>
public class Interrupts : IInterrupts
{
    public Interrupt FIRQ { get; } = new();
    public Interrupt IRQ { get; } = new();

    public void SetFlags(byte data)
    {
        FIRQ.SetFlag((byte)((data & 16) >> 4));
        IRQ.SetFlag((byte)((data & 32) >> 5));
    }
}

public class Interrupt
{
    public bool EnhancedFlag { get; private set; }
    public InterruptFlags Last { get; set; }

    public void Reset()
    {
        EnhancedFlag = false;
        Last = 0;
    }

    public void SetFlag(byte flag)
    {
        EnhancedFlag = flag != 0;
    }
}
