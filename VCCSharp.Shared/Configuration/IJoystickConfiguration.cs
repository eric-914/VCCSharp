using VCCSharp.Shared.Enums;

namespace VCCSharp.Shared.Configuration;

public interface IJoystickConfiguration
{
    int DeviceIndex { get; set; }
    IRangeSelect<JoystickDevices> InputSource { get; }
    IRangeSelect<JoystickEmulations> Type { get; }
    IJoystickKeyMapping KeyMap { get; }
}

public interface ILeftJoystickConfiguration : IJoystickConfiguration { }
public interface IRightJoystickConfiguration : IJoystickConfiguration { }

public class NullJoystickConfiguration : ILeftJoystickConfiguration, IRightJoystickConfiguration
{
    public int DeviceIndex { get; set; }
    public IRangeSelect<JoystickDevices> InputSource { get; } = new NullRangeSelect<JoystickDevices>();
    public IRangeSelect<JoystickEmulations> Type { get; } = new NullRangeSelect<JoystickEmulations>();
    public IJoystickKeyMapping KeyMap { get; } = new NullJoystickKeyMapping();
}
