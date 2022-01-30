using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace VCCSharp.Models.Keyboard
{
    public interface IKeyScanMapper
    {
        byte ToScanCode(Key key);
    }

    /// <summary>
    /// For now, a way to map the Input.Key to the Define.DIK_(scan-code)
    /// </summary>
    public class KeyScanMapper : IKeyScanMapper
    {
        //for displaying key name
        public static IEnumerable<string> KeyNames => KeyNamesMap.Select(x => x.Value);
        public static IEnumerable<Key> KeyIndexes => KeyNamesMap.Select(x => x.Key);

        public static readonly Dictionary<Key, string> KeyNamesMap = new Dictionary<Key, string>
        {
            { Key.None, "None" },
            { Key.Escape, "Esc"},
            { Key.D1, "1"},
            { Key.D2, "2"},
            { Key.D3, "3"},
            { Key.D4, "4"},
            { Key.D5, "5"},
            { Key.D6, "6"},
            { Key.D7, "7"},
            { Key.D8, "8"},
            { Key.D9, "9"},
            { Key.D0, "0"},
            { Key.OemMinus, "-"},
            { Key.OemPlus, "="}, //--Yeah, really
            { Key.Back, "Backspace"},
            { Key.Tab, "Tab"},
            { Key.A, "A"},
            { Key.B, "B"},
            { Key.C, "C"},
            { Key.D, "D"},
            { Key.E, "E"},
            { Key.F, "F"},
            { Key.G, "G"},
            { Key.H, "H"},
            { Key.I, "I"},
            { Key.J, "J"},
            { Key.K, "K"},
            { Key.L, "L"},
            { Key.M, "M"},
            { Key.N, "N"},
            { Key.O, "O"},
            { Key.P, "P"},
            { Key.Q, "Q"},
            { Key.R, "R"},
            { Key.S, "S"},
            { Key.T, "T"},
            { Key.U, "U"},
            { Key.V, "V"},
            { Key.W, "W"},
            { Key.X, "X"},
            { Key.Y, "Y"},
            { Key.Z, "Z"},
            { Key.OemOpenBrackets, "["},
            { Key.OemCloseBrackets, "]"},
            { Key.OemBackslash, "\\"},
            { Key.OemSemicolon, ";"},
            { Key.OemQuotes, "'"}, //--Another weird one
            { Key.OemComma, ","},
            { Key.Decimal, "."},
            { Key.Divide, "/"},
            { Key.CapsLock, "Caps Lock"},
            { Key.LeftShift, "Shift"},  //--Processing will consider left/right shift same
            //{ Key.RightShift, "Shift"},  
            //{ Key.LeftAlt, "Alt"},
            //{ Key.RightAlt, "Alt"},
            //{ Key.LeftCtrl, "Ctrl"},
            //{ Key.RightCtrl, "Ctrl"},
            { Key.Space, "Space"},
            { Key.Enter, "Enter"},
            { Key.Insert, "Insert"},
            { Key.Delete, "Delete"},
            { Key.Home, "Home"},
            { Key.End, "End"},
            { Key.PageUp, "PgUp"},
            { Key.PageDown, "PgDn"},
            { Key.Left, "Left"},
            { Key.Right, "Right"},
            { Key.Up, "Up"},
            { Key.Down, "Down"}
            //{ Key.F1, "F1"},
            //{ Key.F2, "F2"},
        };

        private readonly Dictionary<Key, byte> _mapKeyDIK = new Dictionary<Key, byte>
        {
            { Key.A, Define.DIK_A },
            { Key.B, Define.DIK_B },
            { Key.C, Define.DIK_C },
            { Key.D, Define.DIK_D },
            { Key.E, Define.DIK_E },
            { Key.F, Define.DIK_F },
            { Key.G, Define.DIK_G },
            { Key.H, Define.DIK_H },
            { Key.I, Define.DIK_I },
            { Key.J, Define.DIK_J },
            { Key.K, Define.DIK_K },
            { Key.L, Define.DIK_L },
            { Key.M, Define.DIK_M },
            { Key.N, Define.DIK_N },
            { Key.O, Define.DIK_O },
            { Key.P, Define.DIK_P },
            { Key.Q, Define.DIK_Q },
            { Key.R, Define.DIK_R },
            { Key.S, Define.DIK_S },
            { Key.T, Define.DIK_T },
            { Key.U, Define.DIK_U },
            { Key.V, Define.DIK_V },
            { Key.W, Define.DIK_W },
            { Key.X, Define.DIK_X },
            { Key.Y, Define.DIK_Y },
            { Key.Z, Define.DIK_Z },
            { Key.D1, Define.DIK_1 },
            { Key.D2, Define.DIK_2 },
            { Key.D3, Define.DIK_3 },
            { Key.D4, Define.DIK_4 },
            { Key.D5, Define.DIK_5 },
            { Key.D6, Define.DIK_6 },
            { Key.D7, Define.DIK_7 },
            { Key.D8, Define.DIK_8 },
            { Key.D9, Define.DIK_9 },
            { Key.D0, Define.DIK_0 },
            { Key.OemMinus, Define.DIK_MINUS },
            { Key.OemPlus, Define.DIK_EQUALS },
            { Key.OemComma, Define.DIK_COMMA },
            { Key.OemPeriod, Define.DIK_PERIOD },
            { Key.OemQuestion, Define.DIK_SLASH },
            { Key.OemSemicolon, Define.DIK_SEMICOLON },
            { Key.Space, Define.DIK_SPACE },
            { Key.LeftShift, Define.DIK_LSHIFT },
            { Key.RightShift, Define.DIK_LSHIFT }, //--Intentional
            { Key.Return, Define.DIK_RETURN }, //--Enter key
            { Key.Back, Define.DIK_LEFTARROW }, 
            { Key.Left, Define.DIK_LEFTARROW }, 
            { Key.Right, Define.DIK_RIGHTARROW }, 
            { Key.Up, Define.DIK_UPARROW }, 
            { Key.Down, Define.DIK_DOWNARROW }, 
            { Key.Home, Define.DIK_HOME },  //--Clear button
            { Key.CapsLock, Define.DIK_CAPSLOCK }, //--Alternate for SHIFT-0

            //{ Key.Oem5, 0 }  //--Substituting the backslash (\) for @
        };

        public byte ToScanCode(Key key)
        {
            //System.Diagnostics.Debug.WriteLine($"key={key}");

            if (_mapKeyDIK.ContainsKey(key))
            {
                return _mapKeyDIK[key]; 
            }
            
            return 0;
        }

        public static string ToText(Key key)
        {
            if (KeyNamesMap.ContainsKey(key))
            {
                return KeyNamesMap[key];
            }

            return "???";
        }
    }
}
