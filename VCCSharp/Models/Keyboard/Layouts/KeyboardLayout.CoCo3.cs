// ReSharper disable CommentTypo
// ReSharper disable InvalidXmlDocComment

using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Layouts;

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
    private static KeyTranslationEntry[]? _keyTranslationsCoCo3;

    public class ColorComputer3
    {
        #region Key Definitions

        private static byte[] Escape => Key(Matrix.Break, DIK.DIK_ESCAPE, SHIFT);

        private static byte[] Control => Key(Matrix.Control, DIK.DIK_LCONTROL);
        private static byte[] Alt => Key(Matrix.Alt, DIK.DIK_LALT);

        private static byte[] F1 => Key(Matrix.F1, DIK.DIK_F1);
        private static byte[] F2 => Key(Matrix.F2, DIK.DIK_F2);

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