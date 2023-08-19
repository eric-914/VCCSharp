namespace VCCSharp.Configuration.Models;

public interface IJoystickKeyMapping : ILeft<IJoystickKeyMapping>, IRight<IJoystickKeyMapping>
{
    IKeySelect Left { get; }
    IKeySelect Right { get; }
    IKeySelect Up { get; }
    IKeySelect Down { get; }
    IJoystickButtons Buttons { get; }

    IEnumerable<string> KeyNames { get; }
}
