// ReSharper disable CommentTypo
// ReSharper disable InvalidXmlDocComment

using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard
{
    /**
      Original VCC key translation table for DECB

      VCC BASIC Keyboard:

      +--------------------------------------------------------------------------------+
      [  ][F1][F2][  ][  ][Rst][RGB][  ][Thr][Pwr][StB][FSc][  ]   [    ][   ][    ]   |
      |                                                                                |
      | [ ][1!][2"][3#][4$][5%][6&][7'][8(][9)][0 ][:*][-=][BkSpc]   [    ][Clr][    ] |
      | [    ][Qq][Ww][Ee][Rr][Tt][Yy][Uu][Ii][Oo][Pp][[{][]}][\|]   [    ][Esc][    ] |
      | [ Caps][Aa][Ss][Dd][Ff][Gg][Hh][Jj][Kk][Ll][;:][  ][Enter]                     |
      | [ Shift ][Zz][Xx][Cc][Vv][Bb][Nn][Mm][,<][.>][/?][ Shift ]         [UpA]       |
      | [Cntl][   ][Alt][       Space       ][ @ ][   ][   ][Cntl]   [LftA][DnA][RgtA] |
      +--------------------------------------------------------------------------------+
     */

    public partial class KeyboardLayout
    {
        private static KeyTranslationEntry[] _keyTranslationsCoCo;

        //--Character isn't used, but included for debugging purposes
        private static byte[] Key(byte scanCode1, byte scanCode2, byte row1, byte col1, byte row2, byte col2, char character) 
            => new[] { scanCode1, scanCode2, row1, col1, row2, col2, (byte)character };

        #region Key Definitions

        private static byte[] A => Key(DIK.DIK_A, 0, 1, 1, 0, 0, 'A');
        private static byte[] B => Key(DIK.DIK_B, 0, 1, 2, 0, 0, 'B');
        private static byte[] C => Key(DIK.DIK_C, 0, 1, 3, 0, 0, 'C');
        private static byte[] D => Key(DIK.DIK_D, 0, 1, 4, 0, 0, 'D');
        private static byte[] E => Key(DIK.DIK_E, 0, 1, 5, 0, 0, 'E');
        private static byte[] F => Key(DIK.DIK_F, 0, 1, 6, 0, 0, 'F');
        private static byte[] G => Key(DIK.DIK_G, 0, 1, 7, 0, 0, 'G');
        private static byte[] H => Key(DIK.DIK_H, 0, 2, 0, 0, 0, 'H');
        private static byte[] I => Key(DIK.DIK_I, 0, 2, 1, 0, 0, 'I');
        private static byte[] J => Key(DIK.DIK_J, 0, 2, 2, 0, 0, 'J');
        private static byte[] K => Key(DIK.DIK_K, 0, 2, 3, 0, 0, 'K');
        private static byte[] L => Key(DIK.DIK_L, 0, 2, 4, 0, 0, 'L');
        private static byte[] M => Key(DIK.DIK_M, 0, 2, 5, 0, 0, 'M');
        private static byte[] N => Key(DIK.DIK_N, 0, 2, 6, 0, 0, 'N');
        private static byte[] O => Key(DIK.DIK_O, 0, 2, 7, 0, 0, 'O');
        private static byte[] P => Key(DIK.DIK_P, 0, 4, 0, 0, 0, 'P');
        private static byte[] Q => Key(DIK.DIK_Q, 0, 4, 1, 0, 0, 'Q');
        private static byte[] R => Key(DIK.DIK_R, 0, 4, 2, 0, 0, 'R');
        private static byte[] S => Key(DIK.DIK_S, 0, 4, 3, 0, 0, 'S');
        private static byte[] T => Key(DIK.DIK_T, 0, 4, 4, 0, 0, 'T');
        private static byte[] U => Key(DIK.DIK_U, 0, 4, 5, 0, 0, 'U');
        private static byte[] V => Key(DIK.DIK_V, 0, 4, 6, 0, 0, 'V');
        private static byte[] W => Key(DIK.DIK_W, 0, 4, 7, 0, 0, 'W');
        private static byte[] X => Key(DIK.DIK_X, 0, 8, 0, 0, 0, 'X');
        private static byte[] Y => Key(DIK.DIK_Y, 0, 8, 1, 0, 0, 'Y');
        private static byte[] Z => Key(DIK.DIK_Z, 0, 8, 2, 0, 0, 'Z');

        private static byte[] _0 => Key(DIK.DIK_0, 0, 16, 0, 0, 0, '0');
        private static byte[] _1 => Key(DIK.DIK_1, 0, 16, 1, 0, 0, '1');
        private static byte[] _2 => Key(DIK.DIK_2, 0, 16, 2, 0, 0, '2');
        private static byte[] _3 => Key(DIK.DIK_3, 0, 16, 3, 0, 0, '3');
        private static byte[] _4 => Key(DIK.DIK_4, 0, 16, 4, 0, 0, '4');
        private static byte[] _5 => Key(DIK.DIK_5, 0, 16, 5, 0, 0, '5');
        private static byte[] _6 => Key(DIK.DIK_6, 0, 16, 6, 0, 0, '6');
        private static byte[] _7 => Key(DIK.DIK_7, 0, 16, 7, 0, 0, '7');
        private static byte[] _8 => Key(DIK.DIK_8, 0, 32, 0, 0, 0, '8');
        private static byte[] _9 => Key(DIK.DIK_9, 0, 32, 1, 0, 0, '9');

        private static byte[] Exclamation => Key(DIK.DIK_1, DIK.DIK_LSHIFT, 16, 1, 64, 7, '!');
        private static byte[] DoubleQuote => Key(DIK.DIK_2, DIK.DIK_LSHIFT, 16, 2, 64, 7, '"');
        private static byte[] NumberSign => Key(DIK.DIK_3, DIK.DIK_LSHIFT, 16, 3, 64, 7, '#');
        private static byte[] DollarSign => Key(DIK.DIK_4, DIK.DIK_LSHIFT, 16, 4, 64, 7, '$');
        private static byte[] Percent => Key(DIK.DIK_5, DIK.DIK_LSHIFT, 16, 5, 64, 7, '%');
        private static byte[] Ampersand => Key(DIK.DIK_6, DIK.DIK_LSHIFT, 16, 6, 64, 7, '&');
        private static byte[] SingleQuote => Key(DIK.DIK_7, DIK.DIK_LSHIFT, 16, 7, 64, 7, '\'');
        private static byte[] LeftParenthesis => Key(DIK.DIK_8, DIK.DIK_LSHIFT, 32, 0, 64, 7, '(');
        private static byte[] RightParenthesis => Key(DIK.DIK_9, DIK.DIK_LSHIFT, 32, 1, 64, 7, ')');

        private static byte[] Comma => Key(DIK.DIK_COMMA, 0, 32, 4, 0, 0, ',');
        private static byte[] Period => Key(DIK.DIK_PERIOD, 0, 32, 6, 0, 0, '.');
        private static byte[] QuestionMark => Key(DIK.DIK_SLASH, DIK.DIK_LSHIFT, 32, 7, 64, 7, '?');
        private static byte[] ForwardSlash => Key(DIK.DIK_SLASH, 0, 32, 7, 0, 0, '/');
        private static byte[] Asterisk => Key(DIK.DIK_MINUS, DIK.DIK_LSHIFT, 32, 2, 64, 7, '*');
        private static byte[] Colon => Key(DIK.DIK_MINUS, 0, 32, 2, 0, 0, ':');
        private static byte[] Plus => Key(DIK.DIK_SEMICOLON, DIK.DIK_LSHIFT, 32, 3, 64, 7, '+');
        private static byte[] SemiColon => Key(DIK.DIK_SEMICOLON, 0, 32, 3, 0, 0, ';');
        private static byte[] Equal => Key(DIK.DIK_EQUALS, DIK.DIK_LSHIFT, 32, 5, 64, 7, '=');
        private static byte[] Dash => Key(DIK.DIK_EQUALS, 0, 32, 5, 0, 0, '-');

        private static byte[] ArrowUp => Key(DIK.DIK_UPARROW, 0, 8, 3, 0, 0, '↑');
        private static byte[] ArrowDown => Key(DIK.DIK_DOWNARROW, 0, 8, 4, 0, 0, '↓');
        private static byte[] ArrowLeft => Key(DIK.DIK_LEFTARROW, 0, 8, 5, 0, 0, '←');
        private static byte[] ArrowRight => Key(DIK.DIK_RIGHTARROW, 0, 8, 6, 0, 0, '→');

        private static byte[] Space => Key(DIK.DIK_SPACE, 0, 8, 7, 0, 0, ' ');

        //BACKSPACE -> CoCo left arrow
        private static byte[] BackSpace => Key(DIK.DIK_BACK, 0, 8, 5, 0, 0, '◄');

        //--No need for RSHIFT, code changes it to LSHIFT
        private static byte[] Shift => Key(DIK.DIK_LSHIFT, 0, 64, 7, 0, 0, '▲');
        private static byte[] Control => Key(DIK.DIK_LCONTROL, 0, 64, 4, 0, 0, '©');
        private static byte[] Alt => Key(DIK.DIK_LALT, 0, 64, 3, 0, 0, 'ª');

        //ENTER/Return-Key
        private static byte[] Enter => Key(DIK.DIK_RETURN, 0, 64, 0, 0, 0, '¤');
        //CLEAR/Home-Key
        private static byte[] Clear => Key(DIK.DIK_HOME, 0, 64, 1, 0, 0, '○');
        //BREAK/End-Key
        private static byte[] Break => Key(DIK.DIK_NUMPAD1, 0, 64, 2, 0, 0, '×');

        //CAPS LOCK (CoCo SHIFT-0 for DECB)
        private static byte[] CapsLock => Key(DIK.DIK_CAPSLOCK, 0, 64, 7, 16, 0, '⌂');
        // CAPS LOCK (DECB SHIFT-0, OS-9 CTRL-0 does not need to be emulated specifically)
        private static byte[] ShiftZero => Key(DIK.DIK_0, DIK.DIK_LSHIFT, 16, 0, 64, 7, '⌂');

        #endregion

#pragma warning disable IDE1006 // Naming Styles
        private static readonly byte[][] KeyTranslationsCoCoRaw =
        {
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
            _0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
            Exclamation, DoubleQuote, NumberSign, DollarSign, Percent, Ampersand, SingleQuote, // ! " # $ % & '
            LeftParenthesis, RightParenthesis, // ( )
            Space, BackSpace,
            Comma, Period, QuestionMark, ForwardSlash, Asterisk, Colon, Plus, SemiColon, Equal, Dash,   // , . ? / * : + ; = -
            ArrowUp, ArrowDown, ArrowLeft, ArrowRight, // ↑ ↓ ← →
            Enter, Clear, Break,
            Alt, Control, Shift,
            CapsLock, ShiftZero, //--Shift-0

            //new byte[] { Define.DIK_NUMPAD8, 0, 8, 3, 0, 0 }, //   UP
            //new byte[] { Define.DIK_NUMPAD2, 0, 8, 4, 0, 0 }, //   DOWN
            //new byte[] { Define.DIK_NUMPAD4, 0, 8, 5, 0, 0 }, //   LEFT
            //new byte[] { Define.DIK_NUMPAD6, 0, 8, 6, 0, 0 }, //   RIGHT

            //new byte[] { Define.DIK_F1, 0, 64, 5, 0, 0 }, //   F1
            //new byte[] { Define.DIK_F2, 0, 64, 6, 0, 0 }, //   F2

            // added from OS-9 layout
            //new byte[] { Define.DIK_LBRACKET, 0, 64, 4, 32, 0 }, //   [ (CoCo CTRL 8)
            //new byte[] { Define.DIK_RBRACKET, 0, 64, 4, 32, 1 }, //   ] (CoCo CTRL 9)
            //new byte[] { Define.DIK_LBRACKET, Define.DIK_LSHIFT, 64, 4, 32, 4 }, //   { (CoCo CTRL ,)
            //new byte[] { Define.DIK_RBRACKET, Define.DIK_LSHIFT, 64, 4, 32, 6 }, //   } (CoCo CTRL .)

            //new byte[] { Define.DIK_RALT, 0, 1, 0, 0, 0 }, //   @
        };

        private static KeyTranslationEntry[] GetKeyTranslationsCoCo()
        {
            return _keyTranslationsCoCo ??= ToArray(KeyTranslationsCoCoRaw);
        }
    }
}
