using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
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
        public static IEnumerable<string> KeyText => KeyTextMap.Select(x => x.Value);
        public static IEnumerable<Key> KeyIndexes => KeyTextMap.Select(x => x.Key);

        public static readonly Dictionary<Key, string> KeyTextMap 
            = KeyDefinitions.Instance.Where(x => x.IsMappable).ToDictionary(x => x.Key, x => x.Text);

        public byte ToScanCode(Key key) => KeyDefinitions.Instance.ByKey(key).DIK;
    }
}
