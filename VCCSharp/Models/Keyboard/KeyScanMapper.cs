using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace VCCSharp.Models.Keyboard
{
    /// <summary>
    /// For now, a way to map the Input.Key to the Define.DIK_(scan-code)
    /// </summary>
    public class KeyScanMapper
    {
        private readonly Dictionary<Key, byte> _map = new Dictionary<Key, byte>
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
            { Key.Space, Define.DIK_SPACE },
            { Key.LeftShift, Define.DIK_LSHIFT },
            { Key.RightShift, Define.DIK_LSHIFT }, //--Intentional
            { Key.Return, Define.DIK_RETURN }, 
            { Key.Oem5, (byte)((char)3) },  //--Substituting the backslash (\) for @
        };

        public byte ToScanCode(Key key)
        {
            Debug.WriteLine($"key={key}");

            if (_map.ContainsKey(key))
            {
                return _map[key]; 
            }
            
            return 0;
        }
    }
}
