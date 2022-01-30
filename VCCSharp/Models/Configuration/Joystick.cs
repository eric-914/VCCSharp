using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class Joystick
    {
        public string Device { get; set; } = "";

        public RangeSelect<JoystickDevices> InputSource { get; } = new RangeSelect<JoystickDevices> { Value = JoystickDevices.Mouse };

        public RangeSelect<JoystickEmulations> Type { get; } = new RangeSelect<JoystickEmulations> { Value = JoystickEmulations.Standard };

        public JoystickKeyMapping KeyMap { get; } = new JoystickKeyMapping();
    }
}
