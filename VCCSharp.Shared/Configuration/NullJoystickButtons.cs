using VCCSharp.Configuration.Models;

namespace VCCSharp.Shared.Configuration;

public class NullJoystickButtons : IJoystickButtons
{
    public IKeySelect this[int index] => new NullJoystickKeySelect();
}
