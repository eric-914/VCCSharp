using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules;

public interface IConfigurationManager
{
    IConfiguration Model { get; }

    string? GetFilePath();
    void Load(string filePath);
    void LoadFrom();
    void Save();
    void SaveAs();
}
