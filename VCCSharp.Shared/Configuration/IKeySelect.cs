using System.Windows.Input;
using VCCSharp.Configuration.Models;

namespace VCCSharp.Shared.Configuration;

public class NullJoystickKeySelect : IKeySelect
{
    public Key Value { get; set; } = Key.None;
    public string Selected { get; set; } = string.Empty;
}