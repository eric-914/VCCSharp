using VCCSharp.Configuration.Models;

namespace VCCSharp.Shared.Configuration;

public class NullJoystickKeyMapping : IJoystickKeyMappingConfiguration
{
    public IKeySelectConfiguration Left { get; } = new NullJoystickKeySelect();
    public IKeySelectConfiguration Right { get; }= new NullJoystickKeySelect();
    public IKeySelectConfiguration Up { get; }= new NullJoystickKeySelect();
    public IKeySelectConfiguration Down { get; }= new NullJoystickKeySelect();
    public IJoystickButtonsConfiguration Buttons { get; } = new NullJoystickButtons();
    public IEnumerable<string> KeyNames { get; } = new List<string>();
}
