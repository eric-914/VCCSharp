using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

public interface IKeyboardConfiguration
{
    RangeSelect<KeyboardLayouts> Layout { get; }
}

public class Keyboard : IKeyboardConfiguration
{
    public RangeSelect<KeyboardLayouts> Layout { get; } = new();
}
