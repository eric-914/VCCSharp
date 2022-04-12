using VCCSharp.Shared.Configuration;

namespace VCCSharp.Shared.Models;

public interface IJoysticksConfiguration : IDeviceIndex, IInterval
{
    IJoystickConfiguration Left { get; }
    IJoystickConfiguration Right { get; }
}
