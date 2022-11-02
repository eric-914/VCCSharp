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

    // Display Model Control
    public const byte V0_C = 0xC0; //--Clear
    public const byte V0_S = 0xC1; //--Set
    public const byte V1_C = 0xC2; //--Clear
    public const byte V1_S = 0xC3; //--Set
    public const byte V2_C = 0xC4; //--Clear
    public const byte V2_S = 0xC5; //--Set

    // Display Offset
    public const byte F0_C = 0xC6; //--Clear
    public const byte F0_S = 0xC7; //--Set
    public const byte F1_C = 0xC8; //--Clear
    public const byte F1_S = 0xC9; //--Set
    public const byte F2_C = 0xCA; //--Clear
    public const byte F2_S = 0xCB; //--Set
    public const byte F3_C = 0xCC; //--Clear
    public const byte F3_S = 0xCD; //--Set
    public const byte F4_C = 0xCE; //--Clear
    public const byte F4_S = 0xCF; //--Set
    public const byte F5_C = 0xD0; //--Clear
    public const byte F5_S = 0xD1; //--Set
    public const byte F6_C = 0xD2; //--Clear
    public const byte F6_S = 0xD3; //--Set

    // Page #1
    public const byte P1_C = 0xD4; //--Clear
    public const byte P1_S = 0xD5; //--Set

    // CPU Rate
    public const byte R0_C = 0xD6; //--Clear
    public const byte R0_S = 0xD7; //--Set
    public const byte R1_C = 0xD8; //--Clear
    public const byte R1_S = 0xD9; //--Set

    // Memory Size
    public const byte M0_C = 0xDA; //--Clear
    public const byte M0_S = 0xDB; //--Set
    public const byte M1_C = 0xDC; //--Clear
    public const byte M1_S = 0xDD; //--Set

    // Map Type
    public const byte TY_C = 0xDE; //--Clear
    public const byte TY_S = 0xDF; //--Set



    private const PortHandlers PAK_ = PortHandlers.PAK;
    private const PortHandlers PIA0 = PortHandlers.PIA0;
    private const PortHandlers PIA1 = PortHandlers.PIA1;
    private const PortHandlers GIME = PortHandlers.GIME;
    private const PortHandlers SAM_ = PortHandlers.SAM;
    private const PortHandlers VECT = PortHandlers.VECT;

    //--Handler map for memory range: $FF00 - $FFFF
    private static readonly PortHandlers[] _handlers =
    {
        // $FF__:
        /*        $_0   $_1   $_2   $_3   $_4   $_5   $_6   $_7   $_8   $_9   $_A   $_B   $_C   $_D   $_E   $_F  */
        /* $0_ */ PIA0, PIA0, PIA0, PIA0, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $1_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $2_ */ PIA1, PIA1, PIA1, PIA1, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $3_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,

        /* $4_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $5_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $6_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $7_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,

        /* $8_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $9_ */ GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME,
        /* $A_ */ GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME,
        /* $B_ */ GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME, GIME,

        /* $C_ */ SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_,
        /* $D_ */ SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_, SAM_,
        /* $E_ */ PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_, PAK_,
        /* $F_ */ VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT, VECT
    };

    public static PortHandlers Handler(byte port) => _handlers[port];

    private const SAMHandlers DMC = SAMHandlers.DisplayModelControl;
    private const SAMHandlers DOF = SAMHandlers.DisplayOffset;
    private const SAMHandlers CPU = SAMHandlers.CPURate;
    private const SAMHandlers MAP = SAMHandlers.MapType;
    private const SAMHandlers MEM = SAMHandlers.MemorySize;
    private const SAMHandlers PG1 = SAMHandlers.Page_1;

    //--Handler map for memory range: $FFC0 - $FFDF
    private static readonly SAMHandlers[] _sam =
    {
        DMC, DMC, DMC, DMC, DMC, DMC, DOF, DOF, DOF, DOF, DOF, DOF, DOF, DOF, DOF, DOF,
        DOF, DOF, DOF, DOF, PG1, PG1, CPU, CPU, CPU, CPU, MEM, MEM, MEM, MEM, MAP, MAP
    };

    public static SAMHandlers SAMHandler(byte port) => _sam[port - 0xC0];
}
