using System;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules;

public interface IConfigurationModulePersistence
{
    IConfigurationRoot Load(string? filePath);
    void LoadFrom(string? filePath, Action<string> onContinue);

    void Save(string? filePath, IConfigurationRoot model);
    void SaveAs(string? filePath, Action<string> onContinue);

    bool IsNew(string? filePath);
}