using System.Windows.Input;
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Mappings;

/// <summary>
/// There are different ways to represent a character, and this is an attempt to bring them all together in matrix form.
/// ASCII code
/// The actual character displayed when typed.
/// The text description of the character.
/// The scan-code as interpreted by the CoCo.
/// How .NET represents: Key
/// How Windows represents: DIK
/// And if you need to hit the SHIFT key as part of the key-press
/// TODO: Not sure what's happening with the CTRL key press flag.
/// </summary>
public class KeyDefinitions : List<IKey>
{
    public static KeyDefinitions Instance { get; } = new();

    private static IEnumerable<Key> Arrows => new[] { Key.Left, Key.Right, Key.Up, Key.Down };
    private static IEnumerable<Key> Control => new[] { Key.Tab, Key.Back, Key.Enter, Key.Escape };
    private static IEnumerable<Key> ExtendedControl => new[] { Key.Insert, Key.Delete, Key.Home, Key.End, Key.PageUp, Key.PageDown };

    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numeric = "1234567890";
    private const string LowerSymbols = "`-=[]\\;',./"; //--All non-shifted symbols

    private static KeyDefinition Null => new()
    {
        Key = Key.None,
        DIK = DIK.DIK_NONE,
        ScanCode = ScanCodes.Undefined,
        Text = ChrText.None
    };

    public static IKey Shift => Instance.ByKey(Key.LeftShift);
    public static IKey Return => Instance.ByKey(Key.Return);

    private KeyDefinitions()
    {
        // ReSharper disable once InconsistentNaming
        const bool SHIFT = true;

        void K(byte ascii, char character, string text, byte scanCode, Key key, byte dik, bool shift = false)
        {
            Add(new KeyDefinition { ASCII = ascii, Key = key, DIK = dik, ScanCode = scanCode, Shift = shift, Character = character, Text = text });
        }

        //--Special keys
        void S(Key key, byte dik, string text)
        {
            Add(new KeyDefinition { ASCII = 0xFF, Key = key, DIK = dik, ScanCode = ScanCodes.Null, Shift = false, Character = Chr.None, Text = text });
        }

        K(0x00, Chr.None, /*  */ ChrText.None, /*  */ ScanCodes.Null, /*  */ Key.None, /*  */ DIK.DIK_NONE);
        K(0x09, Chr.Tab, /*   */ ChrText.Tab, /*   */ ScanCodes.Tab, /*   */ Key.Tab, /*   */ DIK.DIK_TAB);
        K(0x0D, Chr.Return, /**/ ChrText.Return, /**/ ScanCodes.Return, /**/ Key.Return, /**/ DIK.DIK_RETURN);

        K(0x20, Chr.Space, ChrText.Space, ScanCodes.Space, Key.Space, DIK.DIK_SPACE);

        K(0x21 /*!*/, Chr.Exclamation, /*     */ ChrText.Exclamation, /*     */ ScanCodes.D1, /*    */ Key.D1, /*       */ DIK.DIK_1 /*         */, SHIFT);
        K(0x22 /*"*/, Chr.DoubleQuotes, /*    */ ChrText.DoubleQuotes, /*    */ ScanCodes.Quotes, /**/ Key.OemQuotes, /**/ DIK.DIK_APOSTROPHE /**/, SHIFT);
        K(0x23 /*#*/, Chr.NumberSign, /*      */ ChrText.NumberSign, /*      */ ScanCodes.D3, /*    */ Key.D3, /*       */ DIK.DIK_3 /*         */, SHIFT);
        K(0x24 /*$*/, Chr.DollarSign, /*      */ ChrText.DollarSign, /*      */ ScanCodes.D4, /*    */ Key.D4, /*       */ DIK.DIK_4 /*         */, SHIFT);
        K(0x25 /*%*/, Chr.Percent, /*         */ ChrText.Percent, /*         */ ScanCodes.D4, /*    */ Key.D5, /*       */ DIK.DIK_5 /*         */, SHIFT);
        K(0x26 /*&*/, Chr.Ampersand, /*       */ ChrText.Ampersand, /*       */ ScanCodes.D7, /*    */ Key.D7, /*       */ DIK.DIK_7 /*         */, SHIFT);
        K(0x27 /*'*/, Chr.SingleQuote, /*     */ ChrText.SingleQuote, /*     */ ScanCodes.Quotes, /**/ Key.OemQuotes, /**/ DIK.DIK_APOSTROPHE /**/);
        K(0x28 /*(*/, Chr.LeftParenthesis, /* */ ChrText.LeftParenthesis, /* */ ScanCodes.D9, /*    */ Key.D9, /*       */ DIK.DIK_9 /*         */, SHIFT);
        K(0x29 /*)*/, Chr.RightParenthesis, /**/ ChrText.RightParenthesis, /**/ ScanCodes.D0, /*    */ Key.D0, /*       */ DIK.DIK_0 /*         */, SHIFT);

        K(0x2A /***/, Chr.Multiply, /**/ ChrText.Multiply, /**/ ScanCodes.D8, /*    */ Key.D8, /*         */ DIK.DIK_8 /*     */, SHIFT);
        K(0x2B /*+*/, Chr.Plus, /*    */ ChrText.Plus, /*    */ ScanCodes.Plus, /*  */ Key.OemPlus, /*    */ DIK.DIK_EQUALS /**/, SHIFT);  //--Weird key name
        K(0x2C /*,*/, Chr.Comma, /*   */ ChrText.Comma, /*   */ ScanCodes.Comma, /* */ Key.OemComma, /*   */ DIK.DIK_COMMA /* */);
        K(0x2D /*-*/, Chr.Minus, /*   */ ChrText.Minus, /*   */ ScanCodes.Minus, /* */ Key.OemMinus, /*   */ DIK.DIK_MINUS /* */);
        K(0x2E /*.*/, Chr.Period, /*  */ ChrText.Period, /*  */ ScanCodes.Period, /**/ Key.OemPeriod, /*  */ DIK.DIK_PERIOD /**/);
        K(0x2F /*/*/, Chr.Slash, /*   */ ChrText.Slash, /*   */ ScanCodes.Slash, /* */ Key.OemQuestion, /**/ DIK.DIK_SLASH /* */); //--Weird key name

        K(0x30 /*0*/, Chr.D0, ChrText.D0, ScanCodes.D0, Key.D0, DIK.DIK_0);
        K(0x31 /*1*/, Chr.D1, ChrText.D1, ScanCodes.D1, Key.D1, DIK.DIK_1);
        K(0x32 /*2*/, Chr.D2, ChrText.D2, ScanCodes.D2, Key.D2, DIK.DIK_2);
        K(0x33 /*3*/, Chr.D3, ChrText.D3, ScanCodes.D3, Key.D3, DIK.DIK_3);
        K(0x34 /*4*/, Chr.D4, ChrText.D4, ScanCodes.D4, Key.D4, DIK.DIK_4);
        K(0x35 /*5*/, Chr.D5, ChrText.D5, ScanCodes.D5, Key.D5, DIK.DIK_5);
        K(0x36 /*6*/, Chr.D6, ChrText.D6, ScanCodes.D6, Key.D6, DIK.DIK_6);
        K(0x37 /*7*/, Chr.D7, ChrText.D7, ScanCodes.D7, Key.D7, DIK.DIK_7);
        K(0x38 /*8*/, Chr.D8, ChrText.D8, ScanCodes.D8, Key.D8, DIK.DIK_8);
        K(0x39 /*9*/, Chr.D9, ChrText.D9, ScanCodes.D9, Key.D9, DIK.DIK_9);

        K(0x3A /*:*/, Chr.Colon, /*      */ ChrText.Colon, /*      */ ScanCodes.Semicolon, /**/ Key.OemSemicolon, /**/ DIK.DIK_SEMICOLON /**/, SHIFT);
        K(0x3B /*;*/, Chr.Semicolon, /*  */ ChrText.Semicolon, /*  */ ScanCodes.Semicolon, /**/ Key.OemSemicolon, /**/ DIK.DIK_SEMICOLON /**/);
        K(0x3C /*<*/, Chr.LessThan, /*   */ ChrText.LessThan, /*   */ ScanCodes.Comma, /*    */ Key.OemComma, /*    */ DIK.DIK_COMMA /*    */, SHIFT);
        K(0x3D /*=*/, Chr.Equal, /*      */ ChrText.Equal, /*      */ ScanCodes.Plus, /*     */ Key.OemPlus, /*     */ DIK.DIK_EQUALS /*   */); //--Weird key name
        K(0x3E /*>*/, Chr.GreaterThan, /**/ ChrText.GreaterThan, /**/ ScanCodes.Period, /*   */ Key.OemPeriod, /*   */ DIK.DIK_PERIOD /*   */, SHIFT);
        K(0x3F /*?*/, Chr.Question, /*   */ ChrText.Question, /*   */ ScanCodes.Slash, /*    */ Key.OemQuestion, /* */ DIK.DIK_SLASH /*    */, SHIFT);
        K(0x40 /*@*/, Chr.AtSign, /*     */ ChrText.AtSign, /*     */ ScanCodes.D2, /*       */ Key.D2, /*          */ DIK.DIK_2 /*        */, SHIFT); //--This key is a pain

        K(0x41 /*A*/, Chr.A, ChrText.A, ScanCodes.A, Key.A, DIK.DIK_A, SHIFT);
        K(0x42 /*B*/, Chr.B, ChrText.B, ScanCodes.B, Key.B, DIK.DIK_B, SHIFT);
        K(0x43 /*C*/, Chr.C, ChrText.C, ScanCodes.C, Key.C, DIK.DIK_C, SHIFT);
        K(0x44 /*D*/, Chr.D, ChrText.D, ScanCodes.D, Key.D, DIK.DIK_D, SHIFT);
        K(0x45 /*E*/, Chr.E, ChrText.E, ScanCodes.E, Key.E, DIK.DIK_E, SHIFT);
        K(0x46 /*F*/, Chr.F, ChrText.F, ScanCodes.F, Key.F, DIK.DIK_F, SHIFT);
        K(0x47 /*G*/, Chr.G, ChrText.G, ScanCodes.G, Key.G, DIK.DIK_G, SHIFT);
        K(0x48 /*H*/, Chr.H, ChrText.H, ScanCodes.H, Key.H, DIK.DIK_H, SHIFT);
        K(0x49 /*I*/, Chr.I, ChrText.I, ScanCodes.I, Key.I, DIK.DIK_I, SHIFT);
        K(0x4A /*J*/, Chr.J, ChrText.J, ScanCodes.J, Key.J, DIK.DIK_J, SHIFT);
        K(0x4B /*K*/, Chr.K, ChrText.K, ScanCodes.K, Key.K, DIK.DIK_K, SHIFT);
        K(0x4C /*L*/, Chr.L, ChrText.L, ScanCodes.L, Key.L, DIK.DIK_L, SHIFT);
        K(0x4D /*M*/, Chr.M, ChrText.M, ScanCodes.M, Key.M, DIK.DIK_M, SHIFT);
        K(0x4E /*N*/, Chr.N, ChrText.N, ScanCodes.N, Key.N, DIK.DIK_N, SHIFT);
        K(0x4F /*O*/, Chr.O, ChrText.O, ScanCodes.O, Key.O, DIK.DIK_O, SHIFT);
        K(0x50 /*P*/, Chr.P, ChrText.P, ScanCodes.P, Key.P, DIK.DIK_P, SHIFT);
        K(0x51 /*Q*/, Chr.Q, ChrText.Q, ScanCodes.Q, Key.Q, DIK.DIK_Q, SHIFT);
        K(0x52 /*R*/, Chr.R, ChrText.R, ScanCodes.R, Key.R, DIK.DIK_R, SHIFT);
        K(0x53 /*S*/, Chr.S, ChrText.S, ScanCodes.S, Key.S, DIK.DIK_S, SHIFT);
        K(0x54 /*T*/, Chr.T, ChrText.T, ScanCodes.T, Key.T, DIK.DIK_T, SHIFT);
        K(0x55 /*U*/, Chr.U, ChrText.U, ScanCodes.U, Key.U, DIK.DIK_U, SHIFT);
        K(0x56 /*V*/, Chr.V, ChrText.V, ScanCodes.V, Key.V, DIK.DIK_V, SHIFT);
        K(0x57 /*W*/, Chr.W, ChrText.W, ScanCodes.W, Key.W, DIK.DIK_W, SHIFT);
        K(0x58 /*X*/, Chr.X, ChrText.X, ScanCodes.X, Key.X, DIK.DIK_X, SHIFT);
        K(0x59 /*Y*/, Chr.Y, ChrText.Y, ScanCodes.Y, Key.Y, DIK.DIK_Y, SHIFT);
        K(0x5A /*Z*/, Chr.Z, ChrText.Z, ScanCodes.Z, Key.Z, DIK.DIK_Z, SHIFT);

        K(0x5B /*[*/, Chr.LeftBracket /* */, ChrText.LeftBracket, /* */ ScanCodes.LBracket, /* */ Key.OemOpenBrackets, /* */ DIK.DIK_LBRACKET /* */);
        K(0x5C /*\*/, Chr.Backslash /*   */, ChrText.Backslash, /*   */ ScanCodes.Backslash, /**/ Key.OemBackslash, /*    */ DIK.DIK_BACKSLASH /**/); //--Might be Oem5
        K(0x5D /*]*/, Chr.RightBracket /**/, ChrText.RightBracket, /**/ ScanCodes.RBracket, /* */ Key.OemCloseBrackets, /**/ DIK.DIK_RBRACKET /* */);
        K(0x5E /*^*/, Chr.Caret /*       */, ChrText.Caret, /*       */ ScanCodes.D6, /*       */ Key.D6, /*              */ DIK.DIK_6 /*        */, SHIFT);
        K(0x5F /*_*/, Chr.Underscore /*  */, ChrText.Underscore, /*  */ ScanCodes.Minus, /*    */ Key.OemMinus, /*        */ DIK.DIK_MINUS /*    */, SHIFT);
        K(0x60 /*`*/, Chr.Grave /*       */, ChrText.Grave, /*       */ ScanCodes.Tilde, /*    */ Key.OemTilde, /*        */ DIK.DIK_GRAVE /*    */); //--TODO: Verify

        K(0x61 /*a*/, Chr.a, ChrText.a, ScanCodes.A, Key.A, DIK.DIK_A);
        K(0x62 /*b*/, Chr.b, ChrText.b, ScanCodes.B, Key.B, DIK.DIK_B);
        K(0x63 /*c*/, Chr.c, ChrText.c, ScanCodes.C, Key.C, DIK.DIK_C);
        K(0x64 /*d*/, Chr.d, ChrText.d, ScanCodes.D, Key.D, DIK.DIK_D);
        K(0x65 /*e*/, Chr.e, ChrText.e, ScanCodes.E, Key.E, DIK.DIK_E);
        K(0x66 /*f*/, Chr.f, ChrText.f, ScanCodes.F, Key.F, DIK.DIK_F);
        K(0x67 /*g*/, Chr.g, ChrText.g, ScanCodes.G, Key.G, DIK.DIK_G);
        K(0x68 /*h*/, Chr.h, ChrText.h, ScanCodes.H, Key.H, DIK.DIK_H);
        K(0x69 /*i*/, Chr.i, ChrText.i, ScanCodes.I, Key.I, DIK.DIK_I);
        K(0x6A /*j*/, Chr.j, ChrText.j, ScanCodes.J, Key.J, DIK.DIK_J);
        K(0x6B /*k*/, Chr.k, ChrText.k, ScanCodes.K, Key.K, DIK.DIK_K);
        K(0x6C /*l*/, Chr.l, ChrText.l, ScanCodes.L, Key.L, DIK.DIK_L);
        K(0x6D /*m*/, Chr.m, ChrText.m, ScanCodes.M, Key.M, DIK.DIK_M);
        K(0x6E /*n*/, Chr.n, ChrText.n, ScanCodes.N, Key.N, DIK.DIK_N);
        K(0x6F /*o*/, Chr.o, ChrText.o, ScanCodes.O, Key.O, DIK.DIK_O);
        K(0x70 /*p*/, Chr.p, ChrText.p, ScanCodes.P, Key.P, DIK.DIK_P);
        K(0x71 /*q*/, Chr.q, ChrText.q, ScanCodes.Q, Key.Q, DIK.DIK_Q);
        K(0x72 /*r*/, Chr.r, ChrText.r, ScanCodes.R, Key.R, DIK.DIK_R);
        K(0x73 /*s*/, Chr.s, ChrText.s, ScanCodes.S, Key.S, DIK.DIK_S);
        K(0x74 /*t*/, Chr.t, ChrText.t, ScanCodes.T, Key.T, DIK.DIK_T);
        K(0x75 /*u*/, Chr.u, ChrText.u, ScanCodes.U, Key.U, DIK.DIK_U);
        K(0x76 /*v*/, Chr.v, ChrText.v, ScanCodes.V, Key.V, DIK.DIK_V);
        K(0x77 /*w*/, Chr.w, ChrText.w, ScanCodes.W, Key.W, DIK.DIK_W);
        K(0x78 /*x*/, Chr.x, ChrText.x, ScanCodes.X, Key.X, DIK.DIK_X);
        K(0x79 /*y*/, Chr.y, ChrText.y, ScanCodes.Y, Key.Y, DIK.DIK_Y);
        K(0x7A /*z*/, Chr.z, ChrText.z, ScanCodes.Z, Key.Z, DIK.DIK_Z);

        K(0x7B /*{*/, Chr.LeftBrace, /* */ ChrText.LeftBrace, /* */ ScanCodes.LBracket, /* */ Key.OemOpenBrackets, /* */ DIK.DIK_LBRACKET /* */, SHIFT);
        K(0x7C /*|*/, Chr.Pipe, /*      */ ChrText.Pipe, /*      */ ScanCodes.Backslash, /**/ Key.OemBackslash, /*    */ DIK.DIK_BACKSLASH /**/, SHIFT);
        K(0x7D /*}*/, Chr.RightBrace, /**/ ChrText.RightBrace, /**/ ScanCodes.RBracket, /* */ Key.OemCloseBrackets, /**/ DIK.DIK_RBRACKET /* */, SHIFT);
        K(0x7E /*~*/, Chr.Tilde, /*     */ ChrText.Tilde, /*     */ ScanCodes.Tilde, /*    */ Key.OemTilde, /*        */ DIK.DIK_GRAVE /*    */, SHIFT);

        //--Other keys used.
        S(Key.LeftShift, DIK.DIK_LSHIFT, ChrText.Shift);
        S(Key.RightShift, DIK.DIK_LSHIFT, ChrText.Shift); //--Intentional

        S(Key.Left, DIK.DIK_LEFTARROW, ChrText.Left);
        S(Key.Right, DIK.DIK_RIGHTARROW, ChrText.Right);
        S(Key.Up, DIK.DIK_UPARROW, ChrText.Up);
        S(Key.Down, DIK.DIK_DOWNARROW, ChrText.Down);

        S(Key.Escape, DIK.DIK_ESCAPE, ChrText.Escape); //--Aka Cancel
        S(Key.Back, DIK.DIK_LEFTARROW, ChrText.Backspace);
        S(Key.CapsLock, DIK.DIK_CAPSLOCK, ChrText.CapsLock); //--Alternate for SHIFT-0

        S(Key.Insert, DIK.DIK_INSERT, ChrText.Insert);
        S(Key.Delete, DIK.DIK_DELETE, ChrText.Delete);
        S(Key.Home, DIK.DIK_HOME, ChrText.Home); //--Clear button
        S(Key.End, DIK.DIK_END, ChrText.End);
        S(Key.PageUp, DIK.DIK_PGUP, ChrText.PageUp);
        S(Key.PageDown, DIK.DIK_PGDN, ChrText.PageDown);

        //TODO:
        //Break-Key
        //Cancel-Key

        foreach (var key in this)
        {
            var item = (KeyDefinition)key;

            if (IsMappable(item)) item.IsMappable = true;
            if (IsAscii(item)) item.IsAscii = true;
        }
    }

    private static bool IsMappable(IKey item) =>
        Alphabet.Contains(item.Character)
        || Numeric.Contains(item.Character)
        || LowerSymbols.Contains(item.Character)
        || Arrows.Contains(item.Key)
        || Control.Contains(item.Key)
        || ExtendedControl.Contains(item.Key);

    private static bool IsAscii(IKey item) => item.Character is > (char)32 and < (char)127;
            
    public IKey ByKey(Key key) => this.FirstOrDefault(x => x.Key == key) ?? Null;
    public IKey ByCharacter(char c) => this.FirstOrDefault(x => x.Character == c) ?? Null;
}
