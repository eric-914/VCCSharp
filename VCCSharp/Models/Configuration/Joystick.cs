using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class Joystick
    {
        public int DeviceIndex { get; set; } = -1;

        public RangeSelect<JoystickDevices> InputSource { get; } = new() { Value = JoystickDevices.Mouse };

        public RangeSelect<JoystickEmulations> Type { get; } = new() { Value = JoystickEmulations.Standard };

        public JoystickKeyMapping KeyMap { get; } = new();
    }
}
