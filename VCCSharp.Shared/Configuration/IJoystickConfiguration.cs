using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Configuration;

public interface IJoystickConfiguration
{
    int DeviceIndex { get; set; }
    IRangeSelect<JoystickDevices> InputSource { get; }
    IRangeSelect<JoystickEmulations> Type { get; }
    IJoystickKeyMapping KeyMap { get; }
}
