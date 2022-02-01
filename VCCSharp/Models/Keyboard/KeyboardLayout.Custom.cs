using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard
{
    /**
        Original VCC key translation table for Custom layout

        PC Keyboard:
        +---------------------------------------------------------------------------------+
        | [Esc]   [F1][F2][F3][F4][F5][F6][F7][F8][F9][F10][F11][F12]   [Prnt][Scr][Paus] |
        |                                                                                 |
        | [`~][1!][2@][3#][4$][5%][6^][7&][8*][9(][0]][-_][=+][BkSpc]   [Inst][Hom][PgUp] |
        | [  Tab][Qq][Ww][Ee][Rr][Tt][Yy][Uu][Ii][Oo][Pp][[{][]}][\|]   [Dlet][End][PgDn] |
        | [  Caps][Aa][Ss][Dd][Ff][Gg][Hh][Jj][Kk][Ll][;:]['"][Enter]                     |
        | [  Shift ][Zz][Xx][Cc][Vv][Bb][Nn][Mm][,<][.>][/?][ Shift ]         [UpA]       |
        | [Cntl][Win][Alt][        Space       ][Alt][Win][Prp][Cntl]   [LftA][DnA][RgtA] |
        +---------------------------------------------------------------------------------+

        VCC Custom Keyboard

        +---------------------------------------------------------------------------------+
        | [Esc][F1][F2][  ][  ][Rst][RGB][  ][Thr][Pwr][StB][FSc][  ]  [    ][   ][    ]  |
        |                                                                                 |
        | [`][1!][2@][3#][4$][5%][6^][7&][8*][9(][0]][-_][=+][BkSpc]   [INST][Clr][PgUp]  |
        | [    ][Qq][Ww][Ee][Rr][Tt][Yy][Uu][Ii][Oo][Pp][[{][]}][\|]   [DEL ][EOL][PgDn]  |
        | [ Caps][Aa][Ss][Dd][Ff][Gg][Hh][Jj][Kk][Ll][;:]['"][Enter]                      |
        | [ Shift ][Zz][Xx][Cc][Vv][Bb][Nn][Mm][,<][.>][/?][ Shift ]         [UpA]        |
        | [Cntl][   ][Alt][       Space       ][Alt][   ][   ][Cntl]   [LftA][DnA][RgtA]  |
        +---------------------------------------------------------------------------------+
     */

    public partial class KeyboardLayout
    {
        private static KeyTranslationEntry[] _keyTranslationsCustom;

#pragma warning disable IDE1006 // Naming Styles
        private static readonly byte[][] KeyTranslationsCustomRaw =
        {
            // ScanCode1,     ScanCode2,      Row1,  Col1,  Row2, Col2    Char  
            new byte[] {DIK.DIK_A, 0, 1, 1, 0, 0}, //   A
            new byte[] {DIK.DIK_B, 0, 1, 2, 0, 0}, //   B
            new byte[] {DIK.DIK_C, 0, 1, 3, 0, 0}, //   C
            new byte[] {DIK.DIK_D, 0, 1, 4, 0, 0}, //   D
            new byte[] {DIK.DIK_E, 0, 1, 5, 0, 0}, //   E
            new byte[] {DIK.DIK_F, 0, 1, 6, 0, 0}, //   F
            new byte[] {DIK.DIK_G, 0, 1, 7, 0, 0}, //   G
            new byte[] {DIK.DIK_H, 0, 2, 0, 0, 0}, //   H
            new byte[] {DIK.DIK_I, 0, 2, 1, 0, 0}, //   I
            new byte[] {DIK.DIK_J, 0, 2, 2, 0, 0}, //   J
            new byte[] {DIK.DIK_K, 0, 2, 3, 0, 0}, //   K
            new byte[] {DIK.DIK_L, 0, 2, 4, 0, 0}, //   L
            new byte[] {DIK.DIK_M, 0, 2, 5, 0, 0}, //   M
            new byte[] {DIK.DIK_N, 0, 2, 6, 0, 0}, //   N
            new byte[] {DIK.DIK_O, 0, 2, 7, 0, 0}, //   O
            new byte[] {DIK.DIK_P, 0, 4, 0, 0, 0}, //   P
            new byte[] {DIK.DIK_Q, 0, 4, 1, 0, 0}, //   Q
            new byte[] {DIK.DIK_R, 0, 4, 2, 0, 0}, //   R
            new byte[] {DIK.DIK_S, 0, 4, 3, 0, 0}, //   S
            new byte[] {DIK.DIK_T, 0, 4, 4, 0, 0}, //   T
            new byte[] {DIK.DIK_U, 0, 4, 5, 0, 0}, //   U
            new byte[] {DIK.DIK_V, 0, 4, 6, 0, 0}, //   V
            new byte[] {DIK.DIK_W, 0, 4, 7, 0, 0}, //   W
            new byte[] {DIK.DIK_X, 0, 8, 0, 0, 0}, //   X
            new byte[] {DIK.DIK_Y, 0, 8, 1, 0, 0}, //   Y
            new byte[] {DIK.DIK_Z, 0, 8, 2, 0, 0}, //   Z
            new byte[] {DIK.DIK_0, 0, 16, 0, 0, 0}, //   0
            new byte[] {DIK.DIK_1, 0, 16, 1, 0, 0}, //   1
            new byte[] {DIK.DIK_2, 0, 16, 2, 0, 0}, //   2
            new byte[] {DIK.DIK_3, 0, 16, 3, 0, 0}, //   3
            new byte[] {DIK.DIK_4, 0, 16, 4, 0, 0}, //   4
            new byte[] {DIK.DIK_5, 0, 16, 5, 0, 0}, //   5
            new byte[] {DIK.DIK_6, 0, 16, 6, 0, 0}, //   6
            new byte[] {DIK.DIK_7, 0, 16, 7, 0, 0}, //   7
            new byte[] {DIK.DIK_8, 0, 32, 0, 0, 0}, //   8
            new byte[] {DIK.DIK_9, 0, 32, 1, 0, 0}, //   9
            new byte[] {DIK.DIK_1, DIK.DIK_LSHIFT, 16, 1, 64, 7}, //   !
            new byte[] {DIK.DIK_2, DIK.DIK_LSHIFT, 1, 0, 0, 0}, //   @
            new byte[] {DIK.DIK_3, DIK.DIK_LSHIFT, 16, 3, 64, 7}, //   #
            new byte[] {DIK.DIK_4, DIK.DIK_LSHIFT, 16, 4, 64, 7}, //   $
            new byte[] {DIK.DIK_5, DIK.DIK_LSHIFT, 16, 5, 64, 7}, //   %
            new byte[] {DIK.DIK_6, DIK.DIK_LSHIFT, 16, 7, 64, 4}, //   ^ (CoCo CTRL 7)
            new byte[] {DIK.DIK_7, DIK.DIK_LSHIFT, 16, 6, 64, 7}, //   &
            new byte[] {DIK.DIK_8, DIK.DIK_LSHIFT, 32, 2, 64, 7}, //   *
            new byte[] {DIK.DIK_9, DIK.DIK_LSHIFT, 32, 0, 64, 7}, //   (
            new byte[] {DIK.DIK_0, DIK.DIK_LSHIFT, 32, 1, 64, 7}, //   )

            new byte[] {DIK.DIK_SEMICOLON, 0, 32, 3, 0, 0}, //   ;
            new byte[] {DIK.DIK_SEMICOLON, DIK.DIK_LSHIFT, 32, 2, 0, 0}, //   :

            new byte[] {DIK.DIK_APOSTROPHE, 0, 16, 7, 64, 7}, //   '
            new byte[] {DIK.DIK_APOSTROPHE, DIK.DIK_LSHIFT, 16, 2, 64, 7}, //   "

            new byte[] {DIK.DIK_COMMA, 0, 32, 4, 0, 0}, //   ,
            new byte[] {DIK.DIK_PERIOD, 0, 32, 6, 0, 0}, //   .
            new byte[] {DIK.DIK_SLASH, DIK.DIK_LSHIFT, 32, 7, 64, 7}, //   ?
            new byte[] {DIK.DIK_SLASH, 0, 32, 7, 0, 0}, //   /
            new byte[] {DIK.DIK_EQUALS, DIK.DIK_LSHIFT, 32, 3, 64, 7}, //   +
            new byte[] {DIK.DIK_EQUALS, 0, 32, 5, 64, 7}, //   =
            new byte[] {DIK.DIK_MINUS, 0, 32, 5, 0, 0}, //   -
            new byte[] {DIK.DIK_MINUS, DIK.DIK_LSHIFT, 32, 5, 64, 4}, //   _ (underscore) (CoCo CTRL -)

            // added
            new byte[] {DIK.DIK_UPARROW, 0, 8, 3, 0, 0}, //   UP
            new byte[] {DIK.DIK_DOWNARROW, 0, 8, 4, 0, 0}, //   DOWN
            new byte[] {DIK.DIK_LEFTARROW, 0, 8, 5, 0, 0}, //   LEFT
            new byte[] {DIK.DIK_RIGHTARROW, 0, 8, 6, 0, 0}, //   RIGHT

            new byte[] {DIK.DIK_NUMPAD8, 0, 8, 3, 0, 0}, //   UP
            new byte[] {DIK.DIK_NUMPAD2, 0, 8, 4, 0, 0}, //   DOWN
            new byte[] {DIK.DIK_NUMPAD4, 0, 8, 5, 0, 0}, //   LEFT
            new byte[] {DIK.DIK_NUMPAD6, 0, 8, 6, 0, 0}, //   RIGHT
            new byte[] {DIK.DIK_SPACE, 0, 8, 7, 0, 0}, //   SPACE

            new byte[] {DIK.DIK_RETURN, 0, 64, 0, 0, 0}, //   ENTER
            new byte[] {DIK.DIK_NUMPAD7, 0, 64, 1, 0, 0}, //   HOME (CLEAR)
            new byte[] {DIK.DIK_ESCAPE, 0, 64, 2, 0, 0}, //   ESCAPE (BREAK)
            new byte[] {DIK.DIK_NUMPAD1, 0, 64, 7, 8, 6}, //   END OF LINE (SHIFT)(RIGHT)
            new byte[] {DIK.DIK_NUMPADPERIOD, 0, 64, 4, 8, 5}, //   DELETE (CTRL)(LEFT)
            new byte[] {DIK.DIK_NUMPAD0, 0, 64, 4, 8, 6}, //   INSERT (CTRL)(RIGHT)
            new byte[] {DIK.DIK_NUMPAD9, 0, 64, 7, 8, 3}, //   PAGEUP (SHFT)(UP)
            new byte[] {DIK.DIK_NUMPAD3, 0, 64, 7, 8, 4}, //   PAGEDOWN (SHFT)(DOWN)
            new byte[] {DIK.DIK_F1, 0, 64, 5, 0, 0}, //   F1
            new byte[] {DIK.DIK_F2, 0, 64, 6, 0, 0}, //   F2
            new byte[] {DIK.DIK_BACK, 0, 8, 5, 0, 0}, //   BACKSPACE -> CoCo left arrow

            new byte[] {DIK.DIK_LBRACKET, 0, 64, 4, 32, 0}, //   [ (CoCo CTRL 8)
            new byte[] {DIK.DIK_RBRACKET, 0, 64, 4, 32, 1}, //   ] (CoCo CTRL 9)
            new byte[] {DIK.DIK_LBRACKET, DIK.DIK_LSHIFT, 64, 4, 32, 4}, //   new byte[] { (CoCo CTRL ,)
            new byte[] {DIK.DIK_RBRACKET, DIK.DIK_LSHIFT, 64, 4, 32, 6}, //   } (CoCo CTRL .)

            new byte[] {DIK.DIK_BACKSLASH, 0, 32, 7, 64, 4}, //   '\' (Back slash) (CoCo CTRL /)
            new byte[] {DIK.DIK_BACKSLASH, DIK.DIK_LSHIFT, 16, 1, 64, 4}, //   | (Pipe) (CoCo CTRL 1)
            new byte[] {DIK.DIK_GRAVE, DIK.DIK_LSHIFT, 16, 3, 64, 4}, //   ~ (tilde) (CoCo CTRL 3)

            new byte[] {DIK.DIK_CAPSLOCK, 0, 64, 4, 16, 0}, //   CAPS LOCK (CoCo CTRL 0 for OS-9)

            new byte[] {DIK.DIK_LALT, 0, 64, 3, 0, 0}, //   ALT
            new byte[] {DIK.DIK_LCONTROL, 0, 64, 4, 0, 0}, //   CTRL
            //   SHIFT (the code converts Define.DIK_RSHIFT to Define.DIK_LSHIFT)
            new byte[] {DIK.DIK_LSHIFT, 0, 64, 7, 0, 0}, 
        };

        public static KeyTranslationEntry[] GetKeyTranslationsCustom()
        {
            return _keyTranslationsCustom ??= ToArray(KeyTranslationsCustomRaw);
        }
    }
}
