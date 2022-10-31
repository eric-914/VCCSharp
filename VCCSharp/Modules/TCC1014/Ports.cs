namespace VCCSharp.Modules.TCC1014;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
public static class Ports
{
    //PIA-0 (IC5)
    //MC6821 P.I.A  Keyboard access $FF00-$FF03
    public const byte PIA_0_0 = 0x00;   // Keyboard Row
    public const byte PIA_0_1 = 0x01;   // HSYNC Interrupt Control
    public const byte PIA_0_2 = 0x02;   // Keyboard Column
    public const byte PIA_0_3 = 0x03;   // VSYNC Interrupt Control

    //PIA-1 (IC4)
    //MC6821 P.I.A	Sound and VDG Control
    public const byte PIA_1_0 = 0x20;   // Cassette Input / RS-232 Output
    public const byte PIA_1_1 = 0x21;   // Cassette/RS-232 Control
    public const byte PIA_1_2 = 0x22;   // Cassette Output / RS-232 Input / VDG Control Output
    public const byte PIA_1_3 = 0x23;

    //TCC1014 G.I.M.E. (IC6)

    //Chip Control Registers
    public const byte INITO = 0x90;     // Initialization Register 0 (INITO)
    public const byte INIT1 = 0x91;     // Initialization Register 1 (INIT1)

    public const byte IRQENR = 0x92;    // Interrupt Request Enable Register (IRQENR)
    public const byte FIRQENR = 0x93;   // Fast Interrupt Request Enable Register (FIRQENR)

    public const byte TIMER_MS = 0x94;  // Timer Most Significant Nibble
    public const byte TIMER_LS = 0x95;  // Timer Least Significant Byte (12-bit)

    public const byte RESERVED_96 = 0x96;  // RESERVED
    public const byte RESERVED_97 = 0x97;  // RESERVED

    public const byte VideoModeRegister = 0x98;         // Video Mode Register
    public const byte VideoResolutionRegister = 0x99;   // Video Resolution Register
    public const byte BorderRegister = 0x9A;            // Border Register

    public const byte RESERVED_9B = 0x9B;  // RESERVED

    public const byte VerticalScrollRegister = 0x9C;    // Vertical Scroll Register
    public const byte VerticalOffset1Register = 0x9D;   // Vertical Offset 1 Register
    public const byte VerticalOffset0Register = 0x9E;   // Vertical Offset 0 Register
    public const byte HorizontalOffsetRegister = 0x9F;  // Horizontal Offset Register

    private const PortHandlers PAK_ = PortHandlers.PAK;
    private const PortHandlers PIA0 = PortHandlers.PIA0;
    private const PortHandlers PIA1 = PortHandlers.PIA1;
    private const PortHandlers GIME = PortHandlers.GIME;
    private const PortHandlers SAM_ = PortHandlers.SAM;

    private static readonly PortHandlers[] _handlers =
    {
        /*00-0F*/ PIA0, PIA0, PIA0, PIA0, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*10-1F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*20-23*/ PIA1, PIA1, PIA1, PIA1, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*30-3F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,

        /*40-4F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*50-5F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*60-6F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*70-7F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,

        /*80-8F*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*90-9F*/ GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME,
        /*A0-AF*/ GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME,
        /*B0-BF*/ GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME,

        /*C0-CF*/ SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_,
        /*D0-DF*/ SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_,
        /*E0-EF*/ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /*F0-FF*/ SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_,
    };

    public static PortHandlers Handler(byte port) => _handlers[port];
}
