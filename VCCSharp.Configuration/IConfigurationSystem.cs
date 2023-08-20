namespace VCCSharp.Configuration
{
    /// <summary>
    /// Configuration details to be supplied by the calling system.
    /// </summary>
    public interface IConfigurationSystem
    {
        string GetExecPath();
    }
}