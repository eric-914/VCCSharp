using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

internal class KeyboardConfiguration : IKeyboardConfiguration
{
    public RangeSelect<KeyboardLayouts> Layout { get; } = new();
}
