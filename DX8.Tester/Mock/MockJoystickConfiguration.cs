using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;
using VCCSharp.Shared.Configuration;

namespace DX8.Tester.Mock;

internal class MockJoystickConfiguration : IJoystickConfiguration
{
    public int DeviceIndex { get; set; }
    public IRangeSelect<JoystickDevices> InputSource { get; } = new NullRangeSelect<JoystickDevices>();
    public IRangeSelect<JoystickEmulations> Type { get; } = new NullRangeSelect<JoystickEmulations>();
    public IJoystickKeyMappingConfiguration KeyMap { get; } = new NullJoystickKeyMapping();
}
