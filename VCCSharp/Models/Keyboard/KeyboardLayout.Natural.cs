using System.Linq;

namespace VCCSharp.Models.Keyboard
{

    /**
      Original VCC key translation table for OS-9

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

      VCC OS-9 Keyboard

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
        private static KeyTranslationEntry[] _keyTranslationsNatural;

        private static readonly byte[][] KeyTranslationsNaturalRaw =
        {
            // ScanCode1,     ScanCode2,      Row1,  Col1,  Row2, Col2    Char  
            new byte[] {Define.DIK_A, 0, 1, 1, 0, 0}, //   A
            new byte[] {Define.DIK_B, 0, 1, 2, 0, 0}, //   B
            new byte[] {Define.DIK_C, 0, 1, 3, 0, 0}, //   C
            new byte[] {Define.DIK_D, 0, 1, 4, 0, 0}, //   D
            new byte[] {Define.DIK_E, 0, 1, 5, 0, 0}, //   E
            new byte[] {Define.DIK_F, 0, 1, 6, 0, 0}, //   F
            new byte[] {Define.DIK_G, 0, 1, 7, 0, 0}, //   G
            new byte[] {Define.DIK_H, 0, 2, 0, 0, 0}, //   H
            new byte[] {Define.DIK_I, 0, 2, 1, 0, 0}, //   I
            new byte[] {Define.DIK_J, 0, 2, 2, 0, 0}, //   J
            new byte[] {Define.DIK_K, 0, 2, 3, 0, 0}, //   K
            new byte[] {Define.DIK_L, 0, 2, 4, 0, 0}, //   L
            new byte[] {Define.DIK_M, 0, 2, 5, 0, 0}, //   M
            new byte[] {Define.DIK_N, 0, 2, 6, 0, 0}, //   N
            new byte[] {Define.DIK_O, 0, 2, 7, 0, 0}, //   O
            new byte[] {Define.DIK_P, 0, 4, 0, 0, 0}, //   P
            new byte[] {Define.DIK_Q, 0, 4, 1, 0, 0}, //   Q
            new byte[] {Define.DIK_R, 0, 4, 2, 0, 0}, //   R
            new byte[] {Define.DIK_S, 0, 4, 3, 0, 0}, //   S
            new byte[] {Define.DIK_T, 0, 4, 4, 0, 0}, //   T
            new byte[] {Define.DIK_U, 0, 4, 5, 0, 0}, //   U
            new byte[] {Define.DIK_V, 0, 4, 6, 0, 0}, //   V
            new byte[] {Define.DIK_W, 0, 4, 7, 0, 0}, //   W
            new byte[] {Define.DIK_X, 0, 8, 0, 0, 0}, //   X
            new byte[] {Define.DIK_Y, 0, 8, 1, 0, 0}, //   Y
            new byte[] {Define.DIK_Z, 0, 8, 2, 0, 0}, //   Z
            new byte[] {Define.DIK_0, 0, 16, 0, 0, 0}, //   0
            new byte[] {Define.DIK_1, 0, 16, 1, 0, 0}, //   1
            new byte[] {Define.DIK_2, 0, 16, 2, 0, 0}, //   2
            new byte[] {Define.DIK_3, 0, 16, 3, 0, 0}, //   3
            new byte[] {Define.DIK_4, 0, 16, 4, 0, 0}, //   4
            new byte[] {Define.DIK_5, 0, 16, 5, 0, 0}, //   5
            new byte[] {Define.DIK_6, 0, 16, 6, 0, 0}, //   6
            new byte[] {Define.DIK_7, 0, 16, 7, 0, 0}, //   7
            new byte[] {Define.DIK_8, 0, 32, 0, 0, 0}, //   8
            new byte[] {Define.DIK_9, 0, 32, 1, 0, 0}, //   9
            new byte[] {Define.DIK_1, Define.DIK_LSHIFT, 16, 1, 64, 7}, //   !
            new byte[] {Define.DIK_2, Define.DIK_LSHIFT, 1, 0, 0, 0}, //   @
            new byte[] {Define.DIK_3, Define.DIK_LSHIFT, 16, 3, 64, 7}, //   #
            new byte[] {Define.DIK_4, Define.DIK_LSHIFT, 16, 4, 64, 7}, //   $
            new byte[] {Define.DIK_5, Define.DIK_LSHIFT, 16, 5, 64, 7}, //   %
            new byte[] {Define.DIK_6, Define.DIK_LSHIFT, 16, 7, 64, 4}, //   ^ (CoCo CTRL 7)
            new byte[] {Define.DIK_7, Define.DIK_LSHIFT, 16, 6, 64, 7}, //   &
            new byte[] {Define.DIK_8, Define.DIK_LSHIFT, 32, 2, 64, 7}, //   *
            new byte[] {Define.DIK_9, Define.DIK_LSHIFT, 32, 0, 64, 7}, //   (
            new byte[] {Define.DIK_0, Define.DIK_LSHIFT, 32, 1, 64, 7}, //   )

            new byte[] {Define.DIK_SEMICOLON, 0, 32, 3, 0, 0}, //   ;
            new byte[] {Define.DIK_SEMICOLON, Define.DIK_LSHIFT, 32, 2, 0, 0}, //   :

            new byte[] {Define.DIK_APOSTROPHE, 0, 16, 7, 64, 7}, //   '
            new byte[] {Define.DIK_APOSTROPHE, Define.DIK_LSHIFT, 16, 2, 64, 7}, //   "

            new byte[] {Define.DIK_COMMA, 0, 32, 4, 0, 0}, //   ,
            new byte[] {Define.DIK_PERIOD, 0, 32, 6, 0, 0}, //   .
            new byte[] {Define.DIK_SLASH, Define.DIK_LSHIFT, 32, 7, 64, 7}, //   ?
            new byte[] {Define.DIK_SLASH, 0, 32, 7, 0, 0}, //   /
            new byte[] {Define.DIK_EQUALS, Define.DIK_LSHIFT, 32, 3, 64, 7}, //   +
            new byte[] {Define.DIK_EQUALS, 0, 32, 5, 64, 7}, //   =
            new byte[] {Define.DIK_MINUS, 0, 32, 5, 0, 0}, //   -
            new byte[] {Define.DIK_MINUS, Define.DIK_LSHIFT, 32, 5, 64, 4}, //   _ (underscore) (CoCo CTRL -)

            // added
            new byte[] {Define.DIK_UPARROW, 0, 8, 3, 0, 0}, //   UP
            new byte[] {Define.DIK_DOWNARROW, 0, 8, 4, 0, 0}, //   DOWN
            new byte[] {Define.DIK_LEFTARROW, 0, 8, 5, 0, 0}, //   LEFT
            new byte[] {Define.DIK_RIGHTARROW, 0, 8, 6, 0, 0}, //   RIGHT

            new byte[] {Define.DIK_NUMPAD8, 0, 8, 3, 0, 0}, //   UP
            new byte[] {Define.DIK_NUMPAD2, 0, 8, 4, 0, 0}, //   DOWN
            new byte[] {Define.DIK_NUMPAD4, 0, 8, 5, 0, 0}, //   LEFT
            new byte[] {Define.DIK_NUMPAD6, 0, 8, 6, 0, 0}, //   RIGHT
            new byte[] {Define.DIK_SPACE, 0, 8, 7, 0, 0}, //   SPACE

            new byte[] {Define.DIK_RETURN, 0, 64, 0, 0, 0}, //   ENTER
            new byte[] {Define.DIK_NUMPAD7, 0, 64, 1, 0, 0}, //   HOME (CLEAR)
            new byte[] {Define.DIK_ESCAPE, 0, 64, 2, 0, 0}, //   ESCAPE (BREAK)
            new byte[] {Define.DIK_NUMPAD1, 0, 64, 7, 8, 6}, //   END OF LINE (SHIFT)(RIGHT)
            new byte[] {Define.DIK_NUMPADPERIOD, 0, 64, 4, 8, 5}, //   DELETE (CTRL)(LEFT)
            new byte[] {Define.DIK_NUMPAD0, 0, 64, 4, 8, 6}, //   INSERT (CTRL)(RIGHT)
            new byte[] {Define.DIK_NUMPAD9, 0, 64, 7, 8, 3}, //   PAGEUP (SHFT)(UP)
            new byte[] {Define.DIK_NUMPAD3, 0, 64, 7, 8, 4}, //   PAGEDOWN (SHFT)(DOWN)
            new byte[] {Define.DIK_F1, 0, 64, 5, 0, 0}, //   F1
            new byte[] {Define.DIK_F2, 0, 64, 6, 0, 0}, //   F2
            new byte[] {Define.DIK_BACK, 0, 8, 5, 0, 0}, //   BACKSPACE -> CoCo left arrow

            new byte[] {Define.DIK_LBRACKET, 0, 64, 4, 32, 0}, //   [ (CoCo CTRL 8)
            new byte[] {Define.DIK_RBRACKET, 0, 64, 4, 32, 1}, //   ] (CoCo CTRL 9)
            new byte[] {Define.DIK_LBRACKET, Define.DIK_LSHIFT, 64, 4, 32, 4}, //   { (CoCo CTRL ,)
            new byte[] {Define.DIK_RBRACKET, Define.DIK_LSHIFT, 64, 4, 32, 6}, //   } (CoCo CTRL .)

            new byte[] {Define.DIK_BACKSLASH, 0, 32, 7, 64, 4}, //   '\' (Back slash) (CoCo CTRL /)
            new byte[] {Define.DIK_BACKSLASH, Define.DIK_LSHIFT, 16, 1, 64, 4}, //   | (Pipe) (CoCo CTRL 1)
            new byte[] {Define.DIK_GRAVE, Define.DIK_LSHIFT, 16, 3, 64, 4}, //   ~ (tilde) (CoCo CTRL 3)

            new byte[] {Define.DIK_CAPSLOCK, 0, 64, 4, 16, 0}, //   CAPS LOCK (CoCo CTRL 0 for OS-9)

            new byte[] {Define.DIK_LALT, 0, 64, 3, 0, 0}, //   ALT
            new byte[] {Define.DIK_LCONTROL, 0, 64, 4, 0, 0}, //   CTRL
            //   SHIFT (the code converts Define.DIK_RSHIFT to Define.DIK_LSHIFT)
            new byte[] {Define.DIK_LSHIFT, 0, 64, 7, 0, 0}
        };

        public static KeyTranslationEntry[] GetKeyTranslationsNatural()
        {
            return _keyTranslationsNatural ??= ToArray(KeyTranslationsNaturalRaw);
        }
    }
}
