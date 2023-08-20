using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

internal class JoystickConfiguration : IJoystickConfiguration
{
    public int DeviceIndex { get; set; } = -1;

    public IRangeSelect<JoystickDevices> InputSource { get; } = new RangeSelect<JoystickDevices> { Value = JoystickDevices.Joystick };

    public IRangeSelect<JoystickEmulations> Type { get; } = new RangeSelect<JoystickEmulations> { Value = JoystickEmulations.Standard };

    public IJoystickKeyMappingConfiguration KeyMap { get; } = new JoystickKeyMappingConfiguration();
}
