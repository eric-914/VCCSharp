using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class Keyboard
    {
        public RangeSelect<KeyboardLayouts> Layout { get; } = new RangeSelect<KeyboardLayouts>();
    }
}
