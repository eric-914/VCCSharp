namespace VCCSharp.Modules.TC1014;

[Flags]
public enum InterruptFlags : byte
{
    Keyboard = 0x02,
    Vertical = 0x08,
    Horizontal = 0x10,
    Timer = 0x20
}

public class Interrupts
{
    public Interrupt FIRQ { get; } = new();
    public Interrupt IRQ { get; } = new();
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
