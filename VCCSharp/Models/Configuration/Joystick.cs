using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Shared.Configuration;
using VCCSharp.Shared.Enums;

namespace VCCSharp.Models.Configuration;

public class Joystick 
{
    public int DeviceIndex { get; set; } = -1;

    public RangeSelect<JoystickDevices> InputSource { get; } = new() { Value = JoystickDevices.Joystick };

    public RangeSelect<JoystickEmulations> Type { get; } = new() { Value = JoystickEmulations.Standard };

    public IJoystickKeyMapping KeyMap { get; } = new JoystickKeyMapping();
}
