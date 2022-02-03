// ReSharper disable CommentTypo
// ReSharper disable InvalidXmlDocComment

using System.Linq;
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Layouts
{
    /*
      Color Computer (3) OS-9 Keyboard
      ┌────────────────────────────────────────────────────────────────────────────────────────┐
      | [Esc]   [F1][F2][  ][  ][  ][  ][  ][  ][  ][   ][   ][   ]          [ ][ ][ ]         |
      |                                                                                        |
      | [  ][1!][2"][3#][4$][5%][6&][7'][8(][9)][0 ][:*][-=][  ←  ]        [ ][Clear][ ]       |
      | [     ][Qq][Ww][Ee][Rr][Tt][Yy][Uu][Ii][Oo][Pp][  ][  ][@ ]        [ ][Break][ ]       |
      | [  Caps][Aa][Ss][Dd][Ff][Gg][Hh][Jj][Kk][Ll][;+][  ][Enter]                            |
      | [  Shift ][Zz][Xx][Cc][Vv][Bb][Nn][Mm][,<][.>][/?][ Shift ]             [↑]            |
      | [Ctrl][   ][Alt][        Space       ][Alt][   ][   ][Ctrl]          [←][↓][→]         |
      └────────────────────────────────────────────────────────────────────────────────────────┘
      (Pretty much the same as the CoCo 1/2 keyboard with new keys: Esc, Alt, Ctrl, F1, F2)
     */
    public partial class KeyboardLayout
    {
        private static KeyTranslationEntry[] _keyTranslationsCoCo3;

        public class ColorComputer3
        {
            #region Key Definitions

            private static byte[] Escape => Key(DIK.DIK_ESCAPE, DIK.DIK_LSHIFT, Matrix.Break);

            private static byte[] Control => Key(DIK.DIK_LCONTROL, 0, Matrix.Control);
            private static byte[] Alt => Key(DIK.DIK_LALT, 0, Matrix.Alt);

            private static byte[] F1 => Key(DIK.DIK_F1, 0, Matrix.F1);
            private static byte[] F2 => Key(DIK.DIK_F2, 0, Matrix.F2);

            #endregion

            public static readonly byte[][] KeyTranslations =
            {
                Escape, Alt, Control, F1, F2
            };
        }

        private static KeyTranslationEntry[] GetKeyTranslationsCoCo3()
        {
            return _keyTranslationsCoCo3 ??= ToArray(ColorComputer.KeyTranslations.Concat(ColorComputer3.KeyTranslations).ToArray());
        }
    }
}
