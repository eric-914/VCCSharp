using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface IJoystickKeyMappingConfiguration : ILeft<IJoystickKeyMappingConfiguration>, IRight<IJoystickKeyMappingConfiguration>
{
    IKeySelectConfiguration Left { get; }
    IKeySelectConfiguration Right { get; }
    IKeySelectConfiguration Up { get; }
    IKeySelectConfiguration Down { get; }
    IJoystickButtonsConfiguration Buttons { get; }
}
