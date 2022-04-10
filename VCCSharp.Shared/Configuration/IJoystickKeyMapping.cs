namespace VCCSharp.Shared.Configuration;

public interface IJoystickKeyMapping
{
    IKeySelect Left { get; }
    IKeySelect Right { get; }
    IKeySelect Up { get; }
    IKeySelect Down { get; }
    IJoystickButtons Buttons { get; }

    IEnumerable<string> KeyNames { get; }
}

public class NullJoystickKeyMapping : IJoystickKeyMapping
{
    public IKeySelect Left { get; } = new NullJoystickKeySelect();
    public IKeySelect Right { get; }= new NullJoystickKeySelect();
    public IKeySelect Up { get; }= new NullJoystickKeySelect();
    public IKeySelect Down { get; }= new NullJoystickKeySelect();
    public IJoystickButtons Buttons { get; } = new NullJoystickButtons();
    public IEnumerable<string> KeyNames { get; } = new List<string>();
}
