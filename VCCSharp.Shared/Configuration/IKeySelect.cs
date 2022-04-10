using System.Windows.Input;

namespace VCCSharp.Shared.Configuration;

public interface IKeySelect
{
    Key Value { get; set; }
    string Selected { get; set; }
}

public class NullJoystickKeySelect : IKeySelect
{
    public Key Value { get; set; } = Key.None;
    public string Selected { get; set; } = string.Empty;
}