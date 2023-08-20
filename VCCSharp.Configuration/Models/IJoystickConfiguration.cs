using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface IJoystickConfiguration : ILeft<IJoystickConfiguration>, IRight<IJoystickConfiguration>
{
    int DeviceIndex { get; set; }
    IRangeSelect<JoystickDevices> InputSource { get; }
    IRangeSelect<JoystickEmulations> Type { get; }
    IJoystickKeyMappingConfiguration KeyMap { get; }
}
