namespace VCCSharp.Models.Keyboard.Definitions;

public static class ScanCodes
{
    public const byte Null = 0x00;   

    public const byte D1 = 0x01;   // 1,!
    public const byte D2 = 0x03;   // 2,@
    public const byte D3 = 0x04;   // 3,#
    public const byte D4 = 0x05;   // 4,$
    public const byte D5 = 0x06;   // 5,%
    public const byte D6 = 0x07;   // 6,^
    public const byte D7 = 0x08;   // 7,&
    public const byte D8 = 0x09;   // 8,*
    public const byte D9 = 0x0A;   // 9,(
    public const byte D0 = 0x0B;   // 0,)

    public const byte A = 0x1E;
    public const byte B = 0x30;
    public const byte C = 0x2E;
    public const byte D = 0x20;
    public const byte E = 0x12;
    public const byte F = 0x21;
    public const byte G = 0x22;
    public const byte H = 0x23;
    public const byte I = 0x17;
    public const byte J = 0x24;
    public const byte K = 0x25;
    public const byte L = 0x26;
    public const byte M = 0x32;
    public const byte N = 0x31;
    public const byte O = 0x18;
    public const byte P = 0x19;
    public const byte Q = 0x10;
    public const byte R = 0x13;
    public const byte S = 0x1F;
    public const byte T = 0x14;
    public const byte U = 0x16;
    public const byte V = 0x2F;
    public const byte W = 0x11;
    public const byte X = 0x2D;
    public const byte Y = 0x15;
    public const byte Z = 0x2C;

    public const byte Space = 0x39;

    public const byte Minus = 0x0C;       // -,_
    public const byte Plus = 0x0D;        // =,+
    public const byte LBracket = 0x1A;    // [,{
    public const byte RBracket = 0x1B;    // ],}
    public const byte Semicolon = 0x27;   // ;,:
    public const byte Quotes = 0x28;      // ',"
    public const byte Tilde = 0x29;       // `,~
    public const byte Slash = 0x35;       // /,?
    public const byte Comma = 0x33;       // ,,<
    public const byte Period = 0x34;      // .,>
    public const byte Backslash = 0x2B;   // \,|

    public const byte Return = 0x1C;      // \n - Enter
    public const byte Tab = 0x39;         // \t - Tab

    public const byte Undefined = 0xFF;
}
