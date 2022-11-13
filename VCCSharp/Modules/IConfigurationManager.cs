using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules;

public interface IConfigurationManager
{
    event ConfigurationChanged OnConfigurationChanged;
    event ConfigurationSave OnConfigurationSave;

    IConfiguration Model { get; }

    string? GetFilePath();
    void Load(string filePath);
    void LoadFrom();
    void Save();
    void SaveAs();
}
