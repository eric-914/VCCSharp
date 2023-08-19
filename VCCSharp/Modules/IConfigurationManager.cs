using VCCSharp.Configuration;

namespace VCCSharp.Modules;

public interface IConfigurationManager
{
    event ConfigurationSynch OnConfigurationSynch;

    IConfiguration Model { get; }

    string? GetFilePath();
    void Load(string filePath);
    void LoadFrom();
    void Save();
    void SaveAs();
}
