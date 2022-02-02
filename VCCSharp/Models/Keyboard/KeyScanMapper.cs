using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using VCCSharp.Models.Keyboard.Definitions;
using VCCSharp.Models.Keyboard.Mappings;

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

        public static readonly Dictionary<Key, string> KeyNamesMap =
            KeyDefinitions.Instance.Where(x => x.IsMappable).ToDictionary(x => x.Key, x => x.Text);

        private readonly Dictionary<Key, byte> _mapKeyDIK = 
            new Dictionary<Key, byte>
        {
            { Key.A, DIK.DIK_A },
            { Key.B, DIK.DIK_B },
            { Key.C, DIK.DIK_C },
            { Key.D, DIK.DIK_D },
            { Key.E, DIK.DIK_E },
            { Key.F, DIK.DIK_F },
            { Key.G, DIK.DIK_G },
            { Key.H, DIK.DIK_H },
            { Key.I, DIK.DIK_I },
            { Key.J, DIK.DIK_J },
            { Key.K, DIK.DIK_K },
            { Key.L, DIK.DIK_L },
            { Key.M, DIK.DIK_M },
            { Key.N, DIK.DIK_N },
            { Key.O, DIK.DIK_O },
            { Key.P, DIK.DIK_P },
            { Key.Q, DIK.DIK_Q },
            { Key.R, DIK.DIK_R },
            { Key.S, DIK.DIK_S },
            { Key.T, DIK.DIK_T },
            { Key.U, DIK.DIK_U },
            { Key.V, DIK.DIK_V },
            { Key.W, DIK.DIK_W },
            { Key.X, DIK.DIK_X },
            { Key.Y, DIK.DIK_Y },
            { Key.Z, DIK.DIK_Z },
            { Key.D1, DIK.DIK_1 },
            { Key.D2, DIK.DIK_2 },
            { Key.D3, DIK.DIK_3 },
            { Key.D4, DIK.DIK_4 },
            { Key.D5, DIK.DIK_5 },
            { Key.D6, DIK.DIK_6 },
            { Key.D7, DIK.DIK_7 },
            { Key.D8, DIK.DIK_8 },
            { Key.D9, DIK.DIK_9 },
            { Key.D0, DIK.DIK_0 },
            { Key.OemMinus, DIK.DIK_MINUS },
            { Key.OemPlus, DIK.DIK_EQUALS },
            { Key.OemComma, DIK.DIK_COMMA },
            { Key.OemPeriod, DIK.DIK_PERIOD },
            { Key.OemQuestion, DIK.DIK_SLASH },
            { Key.OemSemicolon, DIK.DIK_SEMICOLON },
            { Key.Space, DIK.DIK_SPACE },
            { Key.LeftShift, DIK.DIK_LSHIFT },
            { Key.RightShift, DIK.DIK_LSHIFT }, //--Intentional
            { Key.Return, DIK.DIK_RETURN }, //--Enter key
            { Key.Back, DIK.DIK_LEFTARROW },
            { Key.Left, DIK.DIK_LEFTARROW },
            { Key.Right, DIK.DIK_RIGHTARROW },
            { Key.Up, DIK.DIK_UPARROW },
            { Key.Down, DIK.DIK_DOWNARROW },
            { Key.Home, DIK.DIK_HOME },  //--Clear button
            { Key.CapsLock, DIK.DIK_CAPSLOCK }, //--Alternate for SHIFT-0

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
