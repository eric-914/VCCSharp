namespace VCCSharp.Shared.Configuration;

public interface IJoystickButtons
{
    IKeySelect this[int index] { get; }
}

public class NullJoystickButtons : IJoystickButtons
{
    public IKeySelect this[int index] => new NullJoystickKeySelect();
}
