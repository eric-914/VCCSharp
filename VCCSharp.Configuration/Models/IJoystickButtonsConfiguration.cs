namespace VCCSharp.Configuration.Models;

public interface IJoystickButtonsConfiguration
{
    IKeySelectConfiguration this[int index] { get; }
}
