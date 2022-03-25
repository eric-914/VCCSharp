using VCCSharp.Modules;

namespace VCCSharp.Configuration;

public interface IConfigurationWindowManager
{
    void ShowDialog(IConfigurationManager manager);
}
