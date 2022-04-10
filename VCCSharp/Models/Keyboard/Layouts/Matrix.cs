//TODO: Why are there public static bytes here?
#pragma warning disable CA2211

namespace VCCSharp.Models.Keyboard.Layouts;

public class Matrix
{
    #region PIA/6821 Pins

    // ReSharper disable InconsistentNaming
    public static byte PA0 = 1; //Scan row #1
    public static byte PA1 = 2; //Scan row #2
    public static byte PA2 = 3; //Scan row #3
    public static byte PA3 = 4; //Scan row #4
    public static byte PA4 = 5; //Scan row #5
    public static byte PA5 = 6; //Scan row #6
    public static byte PA6 = 7; //Scan row #6

    public static byte PB0 = 0; //Scan column #0
    public static byte PB1 = 1; //Scan column #1
    public static byte PB2 = 2; //Scan column #2
    public static byte PB3 = 3; //Scan column #3
    public static byte PB4 = 4; //Scan column #4
    public static byte PB5 = 5; //Scan column #5
    public static byte PB6 = 6; //Scan column #6
    public static byte PB7 = 7; //Scan column #7

    // ReSharper restore InconsistentNaming

    #endregion

    private static byte R1 => PA0;
    private static byte R2 => PA1;
    private static byte R3 => PA2;
    private static byte R4 => PA3;
    private static byte R5 => PA4;
    private static byte R6 => PA5;
    private static byte R7 => PA6;

    private static byte C0 => PB0;
    private static byte C1 => PB1;
    private static byte C2 => PB2;
    private static byte C3 => PB3;
    private static byte C4 => PB4;
    private static byte C5 => PB5;
    private static byte C6 => PB6;
    private static byte C7 => PB7;

    private static byte[] Add(byte[] a, byte[] b) => a.Concat(b).ToArray();

    public static byte[] AtSign /*          */ = { R1, C0 };
    public static byte[] A /*               */ = { R1, C1 };
    public static byte[] B /*               */ = { R1, C2 };
    public static byte[] C /*               */ = { R1, C3 };
    public static byte[] D /*               */ = { R1, C4 };
    public static byte[] E /*               */ = { R1, C5 };
    public static byte[] F /*               */ = { R1, C6 };
    public static byte[] G /*               */ = { R1, C7 };

    public static byte[] H /*               */ = { R2, C0 };
    public static byte[] I /*               */ = { R2, C1 };
    public static byte[] J /*               */ = { R2, C2 };
    public static byte[] K /*               */ = { R2, C3 };
    public static byte[] L /*               */ = { R2, C4 };
    public static byte[] M /*               */ = { R2, C5 };
    public static byte[] N /*               */ = { R2, C6 };
    public static byte[] O /*               */ = { R2, C7 };

    public static byte[] P /*               */ = { R3, C0 };
    public static byte[] Q /*               */ = { R3, C1 };
    public static byte[] R /*               */ = { R3, C2 };
    public static byte[] S /*               */ = { R3, C3 };
    public static byte[] T /*               */ = { R3, C4 };
    public static byte[] U /*               */ = { R3, C5 };
    public static byte[] V /*               */ = { R3, C6 };
    public static byte[] W /*               */ = { R3, C7 };

    public static byte[] X /*               */ = { R4, C0 };
    public static byte[] Y /*               */ = { R4, C1 };
    public static byte[] Z /*               */ = { R4, C2 };
    public static byte[] Up /*              */ = { R4, C3 };
    public static byte[] Down /*            */ = { R4, C4 };
    public static byte[] Left /*            */ = { R4, C5 };
    public static byte[] Right /*           */ = { R4, C6 };
    public static byte[] Space /*           */ = { R4, C7 };

    public static byte[] D0 /*              */ = { R5, C0 };
    public static byte[] D1 /*              */ = { R5, C1 };
    public static byte[] D2 /*              */ = { R5, C2 };
    public static byte[] D3 /*              */ = { R5, C3 };
    public static byte[] D4 /*              */ = { R5, C4 };
    public static byte[] D5 /*              */ = { R5, C5 };
    public static byte[] D6 /*              */ = { R5, C6 };
    public static byte[] D7 /*              */ = { R5, C7 };

    public static byte[] D8 /*              */ = { R6, C0 };
    public static byte[] D9 /*              */ = { R6, C1 };
    public static byte[] Colon /*           */ = { R6, C2 };
    public static byte[] SemiColon /*       */ = { R6, C3 };
    public static byte[] Comma /*           */ = { R6, C4 };
    public static byte[] Minus /*           */ = { R6, C5 };
    public static byte[] Period /*          */ = { R6, C6 };
    public static byte[] ForwardSlash /*    */ = { R6, C7 };

    public static byte[] Enter /*           */ = { R7, C0 };
    public static byte[] Clear /*           */ = { R7, C1 };
    public static byte[] Break /*           */ = { R7, C2 };
    public static byte[] Alt /*             */ = { R7, C3 };
    public static byte[] Control /*         */ = { R7, C4 };
    public static byte[] F1 /*              */ = { R7, C5 };
    public static byte[] F2 /*              */ = { R7, C6 };
    public static byte[] Shift /*           */ = { R7, C7 };

    public static byte[] Plus /*            */ = Add(Shift, SemiColon);
    public static byte[] Equal /*           */ = Add(Shift, Minus);
    public static byte[] QuestionMark /*    */ = Add(Shift, ForwardSlash);

    public static byte[] Exclamation /*     */ = Add(Shift, D1);
    public static byte[] DoubleQuotes /*    */ = Add(Shift, D2);
    public static byte[] NumberSign /*      */ = Add(Shift, D3);
    public static byte[] DollarSign /*      */ = Add(Shift, D4);
    public static byte[] Percent /*         */ = Add(Shift, D5);
    public static byte[] Ampersand /*       */ = Add(Shift, D6);
    public static byte[] SingleQuote /*     */ = Add(Shift, D7); //--16, 7, 64, 4?

    public static byte[] LeftParenthesis /* */ = Add(Shift, D8);
    public static byte[] RightParenthesis /**/ = Add(Shift, D9);
    public static byte[] Multiply /*        */ = Add(Shift, D7);

    public static byte[] ShiftZero /*       */ = Add(Shift, D0); //--64, 4, 16, 0?  //--16, 0, 64, 7?
    public static byte[] ShiftRight /*      */ = Add(Shift, Right); //--END OF LINE
    public static byte[] ShiftUp /*         */ = Add(Shift, Up); //--PAGE UP
    public static byte[] ShiftDown /*       */ = Add(Shift, Down); //--PAGE DOWN
    public static byte[] ShiftAt /*         */ = Add(Shift, AtSign); //--PAUSE (BASIC)

    //--Some aliases
    public static byte[] Backspace => Left;
    public static byte[] CapsLock => ShiftZero;
    public static byte[] Caret => SingleQuote; //--There is no caret on the CoCo, so make it the single quote
}
