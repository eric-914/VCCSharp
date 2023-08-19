using VCCSharp.Configuration;

namespace VCCSharp.Modules;

public interface IConfigurationPersistenceManager
{
    IConfiguration Load(string? filePath);
    void LoadFrom(string? filePath, Action<string> onContinue);

    void Save(string? filePath, IConfiguration model);
    void SaveAs(string? filePath, Action<string> onContinue);

    bool IsNew(string? filePath);
}
