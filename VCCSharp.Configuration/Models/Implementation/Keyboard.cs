using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

public class Keyboard : IKeyboardConfiguration
{
    public RangeSelect<KeyboardLayouts> Layout { get; } = new();
}
