namespace VCCSharp.Modules;

public interface IModule
{
    /// <summary>
    /// All modules will need to implement what it wants to do on "Reset"
    /// Called when a configuration is loaded or a Hard Reset occurs.
    /// </summary>
    public void ModuleReset();
}