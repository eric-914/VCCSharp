using System.Collections.Generic;
using System.Windows.Input;
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Mappings
{
    public class KeyDefinitions : List<IKey>
    {
        public static KeyDefinitions Instance { get; } = new KeyDefinitions();

        private KeyDefinitions()
        {
            void Add(byte ascii, char scanCode, Key key, byte dik, string text, bool shift = false, bool ctrl = false)
            {
                this.Add(new KeyDefinition { ASCII = ascii, Key = key, DIK = dik, ScanCode = scanCode, Shift = shift, Control = ctrl, Text = text });
            }

            Add(0x00, ScanCodes.Null, Key.None, DIK.DIK_NONE, Chr.None);
            Add(0x09, ScanCodes.Tab, Key.Tab, DIK.DIK_TAB, Chr.Tab);
            Add(0x0D, ScanCodes.Return, Key.Return, DIK.DIK_RETURN, Chr.Return);
            Add(0x1B, ScanCodes.Undefined, Key.Escape, DIK.DIK_ESCAPE, "Esc"); //--Aka Cancel

            Add(0x20, ScanCodes.Space, Key.Space, DIK.DIK_SPACE, Chr.Space);

            Add(0x21 /*!*/, ScanCodes.D1, Key.D1, DIK.DIK_1, Chr.Exclamation, true);
            Add(0x22 /*"*/, ScanCodes.Quotes, Key.OemQuotes, DIK.DIK_APOSTROPHE, Chr.DoubleQuotes, true);
            Add(0x23 /*#*/, ScanCodes.D3, Key.D3, DIK.DIK_3, Chr.NumberSign, true);
            Add(0x24 /*$*/, ScanCodes.D4, Key.D4, DIK.DIK_4, Chr.DollarSign, true);
            Add(0x25 /*%*/, ScanCodes.D4, Key.D5, DIK.DIK_5, Chr.Percent, true);
            Add(0x26 /*&*/, ScanCodes.D7, Key.D7, DIK.DIK_7, Chr.Ampersand, true);
            Add(0x27 /*'*/, ScanCodes.Quotes, Key.OemQuotes, DIK.DIK_APOSTROPHE, Chr.SingleQuote);
            Add(0x28 /*(*/, ScanCodes.D9, Key.D9, DIK.DIK_9, Chr.LeftParenthesis, true);
            Add(0x29 /*)*/, ScanCodes.D0, Key.D0, DIK.DIK_0, Chr.RightParenthesis, true);

            Add(0x2A /***/, ScanCodes.D8, Key.D8, DIK.DIK_8, Chr.Multiply, true);
            Add(0x2B /*+*/, ScanCodes.Plus, Key.OemPlus, DIK.DIK_EQUALS, Chr.Plus, true);  //--Weird key name
            Add(0x2C /*,*/, ScanCodes.Comma, Key.OemComma, DIK.DIK_COMMA, Chr.Comma);
            Add(0x2D /*-*/, ScanCodes.Minus, Key.OemMinus, DIK.DIK_MINUS, Chr.Minus);
            Add(0x2E /*.*/, ScanCodes.Period, Key.OemPeriod, DIK.DIK_PERIOD, Chr.Period);
            Add(0x2F /*/*/, ScanCodes.Slash, Key.OemQuestion, DIK.DIK_SLASH, Chr.Slash); //--Weird key name

            Add(0x30 /*0*/, ScanCodes.D0, Key.D0, DIK.DIK_0, Chr.D0);
            Add(0x31 /*1*/, ScanCodes.D1, Key.D1, DIK.DIK_1, Chr.D1);
            Add(0x32 /*2*/, ScanCodes.D2, Key.D2, DIK.DIK_2, Chr.D2);
            Add(0x33 /*3*/, ScanCodes.D3, Key.D3, DIK.DIK_3, Chr.D3);
            Add(0x34 /*4*/, ScanCodes.D4, Key.D4, DIK.DIK_4, Chr.D4);
            Add(0x35 /*5*/, ScanCodes.D5, Key.D5, DIK.DIK_5, Chr.D5);
            Add(0x36 /*6*/, ScanCodes.D6, Key.D6, DIK.DIK_6, Chr.D6);
            Add(0x37 /*7*/, ScanCodes.D7, Key.D7, DIK.DIK_7, Chr.D7);
            Add(0x38 /*8*/, ScanCodes.D8, Key.D8, DIK.DIK_8, Chr.D8);
            Add(0x39 /*9*/, ScanCodes.D9, Key.D9, DIK.DIK_9, Chr.D9);

            Add(0x3A /*:*/, ScanCodes.Semicolon, Key.OemSemicolon, DIK.DIK_SEMICOLON, Chr.Colon, true);
            Add(0x3B /*;*/, ScanCodes.Semicolon, Key.OemSemicolon, DIK.DIK_SEMICOLON, Chr.Semicolon);
            Add(0x3C /*<*/, ScanCodes.Comma, Key.OemComma, DIK.DIK_COMMA, Chr.LessThan, true);
            Add(0x3D /*=*/, ScanCodes.Plus, Key.OemPlus, DIK.DIK_EQUALS, Chr.Equal); //--Weird key name
            Add(0x3E /*>*/, ScanCodes.Period, Key.OemPeriod, DIK.DIK_PERIOD, Chr.GreaterThan, true);
            Add(0x3F /*?*/, ScanCodes.Slash, Key.OemQuestion, DIK.DIK_SLASH, Chr.Question, true);
            Add(0x40 /*@*/, ScanCodes.D2, Key.D2, DIK.DIK_2, Chr.AtSign, true); //--This key is a pain

            Add(0x41 /*A*/, ScanCodes.A, Key.A, DIK.DIK_A, Chr.A, true);
            Add(0x42 /*B*/, ScanCodes.B, Key.B, DIK.DIK_B, Chr.B, true);
            Add(0x43 /*C*/, ScanCodes.C, Key.C, DIK.DIK_C, Chr.C, true);
            Add(0x44 /*D*/, ScanCodes.D, Key.D, DIK.DIK_D, Chr.D, true);
            Add(0x45 /*E*/, ScanCodes.E, Key.E, DIK.DIK_E, Chr.E, true);
            Add(0x46 /*F*/, ScanCodes.F, Key.F, DIK.DIK_F, Chr.F, true);
            Add(0x47 /*G*/, ScanCodes.G, Key.G, DIK.DIK_G, Chr.G, true);
            Add(0x48 /*H*/, ScanCodes.H, Key.H, DIK.DIK_H, Chr.H, true);
            Add(0x49 /*I*/, ScanCodes.I, Key.I, DIK.DIK_I, Chr.I, true);
            Add(0x4A /*J*/, ScanCodes.J, Key.J, DIK.DIK_J, Chr.J, true);
            Add(0x4B /*K*/, ScanCodes.K, Key.K, DIK.DIK_K, Chr.K, true);
            Add(0x4C /*L*/, ScanCodes.L, Key.L, DIK.DIK_L, Chr.L, true);
            Add(0x4D /*M*/, ScanCodes.M, Key.M, DIK.DIK_M, Chr.M, true);
            Add(0x4E /*N*/, ScanCodes.N, Key.N, DIK.DIK_N, Chr.N, true);
            Add(0x4F /*O*/, ScanCodes.O, Key.O, DIK.DIK_O, Chr.O, true);
            Add(0x50 /*P*/, ScanCodes.P, Key.P, DIK.DIK_P, Chr.P, true);
            Add(0x51 /*Q*/, ScanCodes.Q, Key.Q, DIK.DIK_Q, Chr.Q, true);
            Add(0x52 /*R*/, ScanCodes.R, Key.R, DIK.DIK_R, Chr.R, true);
            Add(0x53 /*S*/, ScanCodes.S, Key.S, DIK.DIK_S, Chr.S, true);
            Add(0x54 /*T*/, ScanCodes.T, Key.T, DIK.DIK_T, Chr.T, true);
            Add(0x55 /*U*/, ScanCodes.U, Key.U, DIK.DIK_U, Chr.U, true);
            Add(0x56 /*V*/, ScanCodes.V, Key.V, DIK.DIK_V, Chr.V, true);
            Add(0x57 /*W*/, ScanCodes.W, Key.W, DIK.DIK_W, Chr.W, true);
            Add(0x58 /*X*/, ScanCodes.X, Key.X, DIK.DIK_X, Chr.X, true);
            Add(0x59 /*Y*/, ScanCodes.Y, Key.Y, DIK.DIK_Y, Chr.Y, true);
            Add(0x5A /*Z*/, ScanCodes.Z, Key.Z, DIK.DIK_Z, Chr.Z, true);

            //--Not sure why these turn on Ctrl
            Add(0x5B /*[*/, ScanCodes.LBracket, Key.OemOpenBrackets, DIK.DIK_LBRACKET, Chr.LeftBracket, false, true);
            Add(0x5C /*\*/, ScanCodes.Backslash, Key.OemBackslash, DIK.DIK_BACKSLASH, Chr.Backslash, false, true); //--Might be Oem5
            Add(0x5D /*]*/, ScanCodes.RBracket, Key.OemCloseBrackets, DIK.DIK_RBRACKET, Chr.RightBracket, false, true);
            Add(0x5E /*^*/, ScanCodes.D6, Key.D6, DIK.DIK_6, Chr.Caret, true);
            Add(0x5F /*_*/, ScanCodes.Minus, Key.OemMinus, DIK.DIK_MINUS, Chr.Underscore, true);
            Add(0x60 /*`*/, ScanCodes.Tilde, Key.OemTilde, DIK.DIK_GRAVE, Chr.Grave); //--TODO: Verify

            Add(0x61 /*a*/, ScanCodes.A, Key.A, DIK.DIK_A, Chr.a);
            Add(0x62 /*b*/, ScanCodes.B, Key.B, DIK.DIK_B, Chr.b);
            Add(0x63 /*c*/, ScanCodes.C, Key.C, DIK.DIK_C, Chr.c);
            Add(0x64 /*d*/, ScanCodes.D, Key.D, DIK.DIK_D, Chr.d);
            Add(0x65 /*e*/, ScanCodes.E, Key.E, DIK.DIK_E, Chr.e);
            Add(0x66 /*f*/, ScanCodes.F, Key.F, DIK.DIK_F, Chr.f);
            Add(0x67 /*g*/, ScanCodes.G, Key.G, DIK.DIK_G, Chr.g);
            Add(0x68 /*h*/, ScanCodes.H, Key.H, DIK.DIK_H, Chr.h);
            Add(0x69 /*i*/, ScanCodes.I, Key.I, DIK.DIK_I, Chr.i);
            Add(0x6A /*j*/, ScanCodes.J, Key.J, DIK.DIK_J, Chr.j);
            Add(0x6B /*k*/, ScanCodes.K, Key.K, DIK.DIK_K, Chr.k);
            Add(0x6C /*l*/, ScanCodes.L, Key.L, DIK.DIK_L, Chr.l);
            Add(0x6D /*m*/, ScanCodes.M, Key.M, DIK.DIK_M, Chr.m);
            Add(0x6E /*n*/, ScanCodes.N, Key.N, DIK.DIK_N, Chr.n);
            Add(0x6F /*o*/, ScanCodes.O, Key.O, DIK.DIK_O, Chr.o);
            Add(0x70 /*p*/, ScanCodes.P, Key.P, DIK.DIK_P, Chr.p);
            Add(0x71 /*q*/, ScanCodes.Q, Key.Q, DIK.DIK_Q, Chr.q);
            Add(0x72 /*r*/, ScanCodes.R, Key.R, DIK.DIK_R, Chr.r);
            Add(0x73 /*s*/, ScanCodes.S, Key.S, DIK.DIK_S, Chr.s);
            Add(0x74 /*t*/, ScanCodes.T, Key.T, DIK.DIK_T, Chr.t);
            Add(0x75 /*u*/, ScanCodes.U, Key.U, DIK.DIK_U, Chr.u);
            Add(0x76 /*v*/, ScanCodes.V, Key.V, DIK.DIK_V, Chr.v);
            Add(0x77 /*w*/, ScanCodes.W, Key.W, DIK.DIK_W, Chr.w);
            Add(0x78 /*x*/, ScanCodes.X, Key.X, DIK.DIK_X, Chr.x);
            Add(0x79 /*y*/, ScanCodes.Y, Key.Y, DIK.DIK_Y, Chr.y);
            Add(0x7A /*z*/, ScanCodes.Z, Key.Z, DIK.DIK_Z, Chr.z);

            Add(0x7B /*{*/, ScanCodes.LBracket, Key.OemOpenBrackets, DIK.DIK_LBRACKET, Chr.LeftBrace, true);
            Add(0x7C /*|*/, ScanCodes.Backslash, Key.OemBackslash, DIK.DIK_BACKSLASH, Chr.Pipe, true);
            Add(0x7D /*}*/, ScanCodes.RBracket, Key.OemCloseBrackets, DIK.DIK_RBRACKET, Chr.RightBrace, true);
            Add(0x7E /*~*/, ScanCodes.Tilde, Key.OemTilde, DIK.DIK_GRAVE, Chr.Tilde, true);

            Add(0x7F, ScanCodes.Undefined, Key.Delete, DIK.DIK_DELETE, Chr.Delete);

            //TODO:
            //Break-Key
            //Cancel-Key
        }
    }
}
