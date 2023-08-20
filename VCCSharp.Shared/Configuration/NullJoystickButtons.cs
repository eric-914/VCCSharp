using VCCSharp.Configuration.Models;

namespace VCCSharp.Shared.Configuration;

public class NullJoystickButtons : IJoystickButtonsConfiguration
{
    public IKeySelectConfiguration this[int index] => new NullJoystickKeySelect();
}
