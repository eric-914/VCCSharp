using VCCSharp.Modules;

namespace VCCSharp.Configuration
{
    public interface IConfigurationWindow
    {
        void ShowDialog(IConfigurationModule configurationModule);
    }
}
