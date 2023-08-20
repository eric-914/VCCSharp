using System.Windows.Input;
using VCCSharp.Models.Keyboard.Mappings;

namespace VCCSharp.Models.Keyboard;

public interface IKeyScanMapper
{
    byte ToScanCode(Key key);
}

/// <summary>
/// For now, a way to map the Input.Key to the Define.DIK_(scan-code)
/// </summary>
public class KeyScanMapper : IKeyScanMapper
{
    public byte ToScanCode(Key key) => KeyDefinitions.Instance.ByKey(key).DIK;
}
