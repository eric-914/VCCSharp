using VCCSharp.Configuration.Models;

namespace VCCSharp.Shared.Configuration;

public class NullJoystickKeyMapping : IJoystickKeyMapping
{
    public IKeySelect Left { get; } = new NullJoystickKeySelect();
    public IKeySelect Right { get; }= new NullJoystickKeySelect();
    public IKeySelect Up { get; }= new NullJoystickKeySelect();
    public IKeySelect Down { get; }= new NullJoystickKeySelect();
    public IJoystickButtons Buttons { get; } = new NullJoystickButtons();
    public IEnumerable<string> KeyNames { get; } = new List<string>();
}
