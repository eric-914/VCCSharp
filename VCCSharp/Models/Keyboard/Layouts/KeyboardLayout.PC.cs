// ReSharper disable InconsistentNaming
// ReSharper disable InvalidXmlDocComment
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Layouts;

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
    private static KeyTranslationEntry[]? _keyTranslationsNatural;

    public class PC
    {
        #region Key Definitions

        #region Printable ASCII

        private static byte[] A => Key(Chr.A, Matrix.A, DIK.DIK_A);
        private static byte[] B => Key(Chr.B, Matrix.B, DIK.DIK_B);
        private static byte[] C => Key(Chr.C, Matrix.C, DIK.DIK_C);
        private static byte[] D => Key(Chr.D, Matrix.D, DIK.DIK_D);
        private static byte[] E => Key(Chr.E, Matrix.E, DIK.DIK_E);
        private static byte[] F => Key(Chr.F, Matrix.F, DIK.DIK_F);
        private static byte[] G => Key(Chr.G, Matrix.G, DIK.DIK_G);
        private static byte[] H => Key(Chr.H, Matrix.H, DIK.DIK_H);
        private static byte[] I => Key(Chr.I, Matrix.I, DIK.DIK_I);
        private static byte[] J => Key(Chr.J, Matrix.J, DIK.DIK_J);
        private static byte[] K => Key(Chr.K, Matrix.K, DIK.DIK_K);
        private static byte[] L => Key(Chr.L, Matrix.L, DIK.DIK_L);
        private static byte[] M => Key(Chr.M, Matrix.M, DIK.DIK_M);
        private static byte[] N => Key(Chr.N, Matrix.N, DIK.DIK_N);
        private static byte[] O => Key(Chr.O, Matrix.O, DIK.DIK_O);
        private static byte[] P => Key(Chr.P, Matrix.P, DIK.DIK_P);
        private static byte[] Q => Key(Chr.Q, Matrix.Q, DIK.DIK_Q);
        private static byte[] R => Key(Chr.R, Matrix.R, DIK.DIK_R);
        private static byte[] S => Key(Chr.S, Matrix.S, DIK.DIK_S);
        private static byte[] T => Key(Chr.T, Matrix.T, DIK.DIK_T);
        private static byte[] U => Key(Chr.U, Matrix.U, DIK.DIK_U);
        private static byte[] V => Key(Chr.V, Matrix.V, DIK.DIK_V);
        private static byte[] W => Key(Chr.W, Matrix.W, DIK.DIK_W);
        private static byte[] X => Key(Chr.X, Matrix.X, DIK.DIK_X);
        private static byte[] Y => Key(Chr.Y, Matrix.Y, DIK.DIK_Y);
        private static byte[] Z => Key(Chr.Z, Matrix.Z, DIK.DIK_Z);

        private static byte[] D0 => Key(Chr.D0, Matrix.D0, DIK.DIK_0);
        private static byte[] D1 => Key(Chr.D1, Matrix.D1, DIK.DIK_1);
        private static byte[] D2 => Key(Chr.D2, Matrix.D2, DIK.DIK_2);
        private static byte[] D3 => Key(Chr.D3, Matrix.D3, DIK.DIK_3);
        private static byte[] D4 => Key(Chr.D4, Matrix.D4, DIK.DIK_4);
        private static byte[] D5 => Key(Chr.D5, Matrix.D5, DIK.DIK_5);
        private static byte[] D6 => Key(Chr.D6, Matrix.D6, DIK.DIK_6);
        private static byte[] D7 => Key(Chr.D7, Matrix.D7, DIK.DIK_7);
        private static byte[] D8 => Key(Chr.D8, Matrix.D8, DIK.DIK_8);
        private static byte[] D9 => Key(Chr.D9, Matrix.D9, DIK.DIK_9);

        private static byte[] Exclamation => Key(Chr.Exclamation, Matrix.Exclamation, DIK.DIK_1, SHIFT);
        private static byte[] AtSign => Key(Chr.AtSign, Matrix.AtSign, DIK.DIK_2, SHIFT);
        private static byte[] NumberSign => Key(Chr.NumberSign, Matrix.NumberSign, DIK.DIK_3, SHIFT);
        private static byte[] DollarSign => Key(Chr.DollarSign, Matrix.DollarSign, DIK.DIK_4, SHIFT);
        private static byte[] Percent => Key(Chr.Percent, Matrix.Percent, DIK.DIK_5, SHIFT);
        private static byte[] Caret => Key(Chr.Caret, Matrix.Caret, DIK.DIK_6, SHIFT);
        private static byte[] Ampersand => Key(Chr.Ampersand, Matrix.Ampersand, DIK.DIK_7, SHIFT);
        private static byte[] Multiply => Key(Chr.Multiply, Matrix.Multiply, DIK.DIK_8, SHIFT);
        private static byte[] LeftParenthesis => Key(Chr.LeftParenthesis, Matrix.LeftParenthesis, DIK.DIK_9, SHIFT);
        private static byte[] RightParenthesis => Key(Chr.RightParenthesis, Matrix.RightParenthesis, DIK.DIK_0, SHIFT);

        private static byte[] SemiColon => Key(Chr.Semicolon, Matrix.Colon, DIK.DIK_SEMICOLON);
        private static byte[] Colon => Key(Chr.Colon, Matrix.SemiColon, DIK.DIK_SEMICOLON, SHIFT);
        private static byte[] SingleQuote => Key(Chr.SingleQuote, Matrix.SingleQuote, DIK.DIK_APOSTROPHE);
        private static byte[] DoubleQuotes => Key(Chr.DoubleQuotes, Matrix.DoubleQuotes, DIK.DIK_APOSTROPHE, SHIFT);

        private static byte[] Comma => Key(Chr.Comma, Matrix.Comma, DIK.DIK_COMMA);
        private static byte[] Period => Key(Chr.Period, Matrix.Period, DIK.DIK_PERIOD);
        private static byte[] QuestionMark => Key(Chr.Question, Matrix.QuestionMark, DIK.DIK_SLASH, SHIFT);
        private static byte[] ForwardSlash => Key(Chr.Slash, Matrix.ForwardSlash, DIK.DIK_SLASH);
        private static byte[] Plus => Key(Chr.Plus, Matrix.Plus, DIK.DIK_EQUALS, SHIFT);
        private static byte[] Equal => Key(Chr.Equal, Matrix.Equal, DIK.DIK_EQUALS);
        private static byte[] Minus => Key(Chr.Minus, Matrix.Minus, DIK.DIK_MINUS);

        private static byte[] Space => Key(Chr.Space, Matrix.Space, DIK.DIK_SPACE);

        #endregion

        #region Other Character Keys

        private static byte[] ArrowUp => Key(Chr.Up, Matrix.Up, DIK.DIK_UPARROW);
        private static byte[] ArrowDown => Key(Chr.Down, Matrix.Down, DIK.DIK_DOWNARROW);
        private static byte[] ArrowLeft => Key(Chr.Left, Matrix.Left, DIK.DIK_LEFTARROW);
        private static byte[] ArrowRight => Key(Chr.Right, Matrix.Right, DIK.DIK_RIGHTARROW);

        private static byte[] Tab => Key(Chr.Tab, Matrix.Clear, DIK.DIK_TAB);
        private static byte[] Enter => Key(Chr.Return, Matrix.Enter, DIK.DIK_RETURN); //--Enter/Return

        #endregion

        #region Command Keys

        private static byte[] Escape => Key(Matrix.Break, DIK.DIK_ESCAPE); //--BREAK

        private static byte[] Home => Key(Matrix.Clear, DIK.DIK_HOME); //--CLEAR
        private static byte[] End => Key(Matrix.ShiftRight, DIK.DIK_END); //--END OF LINE
        private static byte[] PageUp => Key(Matrix.ShiftUp, DIK.DIK_PGUP); //--PAGE-UP
        private static byte[] PageDown => Key(Matrix.ShiftDown, DIK.DIK_PGDN); //--PAGE-DOWN

        private static byte[] F1 => Key(Matrix.F1, DIK.DIK_F1); //--F1 - CoCo3
        private static byte[] F2 => Key(Matrix.F2, DIK.DIK_F2); //--F2 - CoCo3

        private static byte[] BackSpace => Key(Matrix.Backspace, DIK.DIK_BACK); //--CoCo left arrow

        private static byte[] CapsLock => Key(Matrix.CapsLock, DIK.DIK_CAPSLOCK); //--(CoCo CTRL 0 for OS-9)

        private static byte[] Alt => Key(Matrix.Alt, DIK.DIK_LALT);
        private static byte[] Control => Key(Matrix.Control, DIK.DIK_LCONTROL);
        private static byte[] Shift => Key(Matrix.Shift, DIK.DIK_LSHIFT);

        #endregion

        #endregion

        public static readonly byte[][] KeyTranslations =
        {
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
            D0, D1, D2, D3, D4, D5, D6, D7, D8, D9,
            Exclamation, AtSign, NumberSign, DollarSign, Percent, Caret, Ampersand, Multiply, LeftParenthesis, RightParenthesis,
            SemiColon, Colon, SingleQuote, DoubleQuotes,
            Comma, Period, QuestionMark, ForwardSlash, Plus, Equal, Minus,
            ArrowUp, ArrowDown, ArrowLeft, ArrowRight, 
            Space, Enter, Tab, Home, Escape, End, PageUp, PageDown,
            F1, F2, BackSpace,
            CapsLock, Alt, Control, Shift
        };
    }

    private static KeyTranslationEntry[] GetKeyTranslationsNatural()
    {
        return _keyTranslationsNatural ??= ToArray(PC.KeyTranslations);
    }
}