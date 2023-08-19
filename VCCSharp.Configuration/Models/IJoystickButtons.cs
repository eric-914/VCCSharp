namespace VCCSharp.Configuration.Models;

public interface IJoystickButtons
{
    IKeySelect this[int index] { get; }
}
