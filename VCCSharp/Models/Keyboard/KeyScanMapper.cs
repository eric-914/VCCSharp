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
    KeyDefinitions keyDefinitions = new();
    
    public byte ToScanCode(Key key) => keyDefinitions.ByKey(key).DIK;
}
