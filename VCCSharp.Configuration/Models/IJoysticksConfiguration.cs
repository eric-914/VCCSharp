namespace VCCSharp.Configuration.Models;

public interface IJoysticksConfiguration : IDeviceIndex, IInterval
{
    IJoystickConfiguration Left { get; }
    IJoystickConfiguration Right { get; }
}
