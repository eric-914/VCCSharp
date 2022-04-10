namespace VCCSharp.Shared.Configuration;

public interface IJoystickButtons
{
    IKeySelect this[int index] { get; }
}
