using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface IKeyboardConfiguration
{
    RangeSelect<KeyboardLayouts> Layout { get; }
}
