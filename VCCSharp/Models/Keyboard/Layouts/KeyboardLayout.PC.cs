#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming
// ReSharper disable InvalidXmlDocComment
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Layouts
{
    /*
      PC Keyboard:
      ┌────────────────────────────────────────────────────────────────────────────────────────┐
      │ [Esc]   [F1][F2][F3][F4][F5][F6][F7][F8][F9][F10][F11][F12]   [PrtScn][ScrLk][Pause]   │
      │                                                                                        │
      │ [`~][1!][2@][3#][4$][5%][6^][7&][8*][9(][0]][-_][=+][BkSpc]       [Ins][Hom][PgUp]     │
      │ [  Tab][Qq][Ww][Ee][Rr][Tt][Yy][Uu][Ii][Oo][Pp][[{][]}][\|]       [Del][End][PgDn]     │
      │ [  Caps][Aa][Ss][Dd][Ff][Gg][Hh][Jj][Kk][Ll][;:]['"][Enter]                            │
      │ [  Shift ][Zz][Xx][Cc][Vv][Bb][Nn][Mm][,<][.>][/?][ Shift ]             [↑]            │
      │ [Ctrl][Win][Alt][        Space       ][Alt][Win][Prp][Ctrl]          [←][↓][→]         │
      └────────────────────────────────────────────────────────────────────────────────────────┘
     */
    public partial class KeyboardLayout
    {
        private static KeyTranslationEntry[] _keyTranslationsNatural;

        public class PC
        {
            #region Key Definitions

            #region Printable ASCII

            private static byte[] A => Key(Chr.A, DIK.DIK_A, 0, Matrix.A);
            private static byte[] B => Key(Chr.B, DIK.DIK_B, 0, Matrix.B);
            private static byte[] C => Key(Chr.C, DIK.DIK_C, 0, Matrix.C);
            private static byte[] D => Key(Chr.D, DIK.DIK_D, 0, Matrix.D);
            private static byte[] E => Key(Chr.E, DIK.DIK_E, 0, Matrix.E);
            private static byte[] F => Key(Chr.F, DIK.DIK_F, 0, Matrix.F);
            private static byte[] G => Key(Chr.G, DIK.DIK_G, 0, Matrix.G);
            private static byte[] H => Key(Chr.H, DIK.DIK_H, 0, Matrix.H);
            private static byte[] I => Key(Chr.I, DIK.DIK_I, 0, Matrix.I);
            private static byte[] J => Key(Chr.J, DIK.DIK_J, 0, Matrix.J);
            private static byte[] K => Key(Chr.K, DIK.DIK_K, 0, Matrix.K);
            private static byte[] L => Key(Chr.L, DIK.DIK_L, 0, Matrix.L);
            private static byte[] M => Key(Chr.M, DIK.DIK_M, 0, Matrix.M);
            private static byte[] N => Key(Chr.N, DIK.DIK_N, 0, Matrix.N);
            private static byte[] O => Key(Chr.O, DIK.DIK_O, 0, Matrix.O);
            private static byte[] P => Key(Chr.P, DIK.DIK_P, 0, Matrix.P);
            private static byte[] Q => Key(Chr.Q, DIK.DIK_Q, 0, Matrix.Q);
            private static byte[] R => Key(Chr.R, DIK.DIK_R, 0, Matrix.R);
            private static byte[] S => Key(Chr.S, DIK.DIK_S, 0, Matrix.S);
            private static byte[] T => Key(Chr.T, DIK.DIK_T, 0, Matrix.T);
            private static byte[] U => Key(Chr.U, DIK.DIK_U, 0, Matrix.U);
            private static byte[] V => Key(Chr.V, DIK.DIK_V, 0, Matrix.V);
            private static byte[] W => Key(Chr.W, DIK.DIK_W, 0, Matrix.W);
            private static byte[] X => Key(Chr.X, DIK.DIK_X, 0, Matrix.X);
            private static byte[] Y => Key(Chr.Y, DIK.DIK_Y, 0, Matrix.Y);
            private static byte[] Z => Key(Chr.Z, DIK.DIK_Z, 0, Matrix.Z);

            private static byte[] D0 => Key(Chr.D0, DIK.DIK_0, 0, Matrix.D0);
            private static byte[] D1 => Key(Chr.D1, DIK.DIK_1, 0, Matrix.D1);
            private static byte[] D2 => Key(Chr.D2, DIK.DIK_2, 0, Matrix.D2);
            private static byte[] D3 => Key(Chr.D3, DIK.DIK_3, 0, Matrix.D3);
            private static byte[] D4 => Key(Chr.D4, DIK.DIK_4, 0, Matrix.D4);
            private static byte[] D5 => Key(Chr.D5, DIK.DIK_5, 0, Matrix.D5);
            private static byte[] D6 => Key(Chr.D6, DIK.DIK_6, 0, Matrix.D6);
            private static byte[] D7 => Key(Chr.D7, DIK.DIK_7, 0, Matrix.D7);
            private static byte[] D8 => Key(Chr.D8, DIK.DIK_8, 0, Matrix.D8);
            private static byte[] D9 => Key(Chr.D9, DIK.DIK_9, 0, Matrix.D9);

            private static byte[] Exclamation => Key(Chr.Exclamation, DIK.DIK_1, DIK.DIK_LSHIFT, Matrix.Exclamation);
            private static byte[] AtSign => Key(Chr.AtSign, DIK.DIK_2, DIK.DIK_LSHIFT, Matrix.AtSign);
            private static byte[] NumberSign => Key(Chr.NumberSign, DIK.DIK_3, DIK.DIK_LSHIFT, Matrix.NumberSign);
            private static byte[] DollarSign => Key(Chr.DollarSign, DIK.DIK_4, DIK.DIK_LSHIFT, Matrix.DollarSign);
            private static byte[] Percent => Key(Chr.Percent, DIK.DIK_5, DIK.DIK_LSHIFT, Matrix.Percent);
            private static byte[] Caret => Key(Chr.Caret, DIK.DIK_6, DIK.DIK_LSHIFT, Matrix.Caret);
            private static byte[] Ampersand => Key(Chr.Ampersand, DIK.DIK_7, DIK.DIK_LSHIFT, Matrix.Ampersand);
            private static byte[] Multiply => Key(Chr.Multiply, DIK.DIK_8, DIK.DIK_LSHIFT, Matrix.Multiply);
            private static byte[] LeftParenthesis => Key(Chr.LeftParenthesis, DIK.DIK_9, DIK.DIK_LSHIFT, Matrix.LeftParenthesis);
            private static byte[] RightParenthesis => Key(Chr.RightParenthesis, DIK.DIK_0, DIK.DIK_LSHIFT, Matrix.RightParenthesis);

            private static byte[] SemiColon => Key(Chr.Semicolon, DIK.DIK_SEMICOLON, 0, Matrix.Colon);
            private static byte[] Colon => Key(Chr.Colon, DIK.DIK_SEMICOLON, DIK.DIK_LSHIFT, Matrix.SemiColon);
            private static byte[] SingleQuote => Key(Chr.SingleQuote, DIK.DIK_APOSTROPHE, 0, Matrix.SingleQuote);
            private static byte[] DoubleQuotes => Key(Chr.DoubleQuotes, DIK.DIK_APOSTROPHE, DIK.DIK_LSHIFT, Matrix.DoubleQuotes);

            private static byte[] Comma => Key(Chr.Comma, DIK.DIK_COMMA, 0, Matrix.Comma);
            private static byte[] Period => Key(Chr.Period, DIK.DIK_PERIOD, 0, Matrix.Period);
            private static byte[] QuestionMark => Key(Chr.Question, DIK.DIK_SLASH, DIK.DIK_LSHIFT, Matrix.QuestionMark);
            private static byte[] ForwardSlash => Key(Chr.Slash, DIK.DIK_SLASH, 0, Matrix.ForwardSlash);
            private static byte[] Plus => Key(Chr.Plus, DIK.DIK_EQUALS, DIK.DIK_LSHIFT, Matrix.Plus);
            private static byte[] Equal => Key(Chr.Equal, DIK.DIK_EQUALS, 0, Matrix.Equal);
            private static byte[] Minus => Key(Chr.Minus, DIK.DIK_MINUS, 0, Matrix.Minus);

            private static byte[] Space => Key(Chr.Space, DIK.DIK_SPACE, 0, Matrix.Space);

            #endregion

            #region Other Character Keys

            private static byte[] ArrowUp => Key(Chr.Up, DIK.DIK_UPARROW, 0, Matrix.Up);
            private static byte[] ArrowDown => Key(Chr.Down, DIK.DIK_DOWNARROW, 0, Matrix.Down);
            private static byte[] ArrowLeft => Key(Chr.Left, DIK.DIK_LEFTARROW, 0, Matrix.Left);
            private static byte[] ArrowRight => Key(Chr.Right, DIK.DIK_RIGHTARROW, 0, Matrix.Right);

            private static byte[] NumPadUp => Key(Chr.Up, DIK.DIK_NUMPAD8, 0, Matrix.Up);
            private static byte[] NumPadDown => Key(Chr.Down, DIK.DIK_NUMPAD2, 0, Matrix.Down);
            private static byte[] NumPadLeft => Key(Chr.Left, DIK.DIK_NUMPAD4, 0, Matrix.Left);
            private static byte[] NumPadRight => Key(Chr.Right, DIK.DIK_NUMPAD6, 0, Matrix.Right);

            private static byte[] Tab => Key(Chr.Tab, DIK.DIK_TAB, 0, Matrix.Clear);
            private static byte[] Enter => Key(Chr.Return, DIK.DIK_RETURN, 0, Matrix.Enter); //--Enter/Return

            #endregion

            #region Command Keys

            private static byte[] Escape => Key(DIK.DIK_ESCAPE, 0, Matrix.Break); //--BREAK

            private static byte[] NumPadHome => Key(DIK.DIK_NUMPAD7, 0, Matrix.Clear); //--CLEAR
            private static byte[] NumPadEnd => Key(DIK.DIK_NUMPAD1, 0, Matrix.ShiftRight); //--END OF LINE
            private static byte[] NumPadPageUp => Key(DIK.DIK_NUMPAD9, 0, Matrix.ShiftUp); //--PAGE-UP
            private static byte[] NumPadPageDown => Key(DIK.DIK_NUMPAD3, 0, Matrix.ShiftDown); //--PAGE-DOWN

            private static byte[] F1 => Key(DIK.DIK_F1, 0, Matrix.F1); //--F1 - CoCo3
            private static byte[] F2 => Key(DIK.DIK_F2, 0, Matrix.F2); //--F2 - CoCo3

            private static byte[] BackSpace => Key(DIK.DIK_BACK, 0, Matrix.Backspace); //--CoCo left arrow

            private static byte[] CapsLock => Key(DIK.DIK_CAPSLOCK, 0, Matrix.CapsLock); //--(CoCo CTRL 0 for OS-9)

            private static byte[] Alt => Key(DIK.DIK_LALT, 0, Matrix.Alt);
            private static byte[] Control => Key(DIK.DIK_LCONTROL, 0, Matrix.Control);
            private static byte[] Shift => Key(DIK.DIK_LSHIFT, 0, Matrix.Shift);

            #endregion

            #endregion

            public static readonly byte[][] KeyTranslations =
            {
                A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
                D0, D1, D2, D3, D4, D5, D6, D7, D8, D9,
                Exclamation, AtSign, NumberSign, DollarSign, Percent, Caret, Ampersand, Multiply, LeftParenthesis, RightParenthesis,
                SemiColon, Colon, SingleQuote, DoubleQuotes,
                Comma, Period, QuestionMark, ForwardSlash, Plus, Equal, Minus,
                ArrowUp, ArrowDown, ArrowLeft, ArrowRight, NumPadUp, NumPadDown, NumPadLeft, NumPadRight,
                Space, Enter, Tab, NumPadHome, Escape, NumPadEnd, NumPadPageUp, NumPadPageDown,
                F1, F2, BackSpace,
                CapsLock, Alt, Control, Shift
            };
        }

        private static KeyTranslationEntry[] GetKeyTranslationsNatural()
        {
            return _keyTranslationsNatural ??= ToArray(PC.KeyTranslations);
        }
    }
}
