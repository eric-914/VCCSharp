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
