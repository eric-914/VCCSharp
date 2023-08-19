using System.Diagnostics;
using System.IO;
using VCCSharp.Configuration;

namespace VCCSharp.Modules;

public enum SynchDirection
{
    ConfigurationChanged,
    SaveConfiguration
}

public delegate void ConfigurationSynch(SynchDirection direction, IConfiguration model);

public class ConfigurationManager : IConfigurationManager
{
    public event ConfigurationSynch? OnConfigurationSynch;

    private readonly IConfigurationPersistenceManager _persistenceManager;

    private string? _filePath;

    public IConfiguration Model { get; private set; } = default!;

    public ConfigurationManager(IConfigurationPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;
    }

    public string? GetFilePath() => _filePath;

    public void Load(string filePath)
    {
        Debug.WriteLine("Load(...)");

        _filePath = GetConfigurationFilePath(filePath);  //--Use default if needed

        Model = _persistenceManager.Load(_filePath);

        OnConfigurationSynch?.Invoke(SynchDirection.ConfigurationChanged, Model);

        if (_persistenceManager.IsNew(_filePath))
        {
            Save();
        }
    }

    // LoadFrom allows user to browse for an ini file and reloads the configuration from it.
    public void LoadFrom() => _persistenceManager.LoadFrom(_filePath, LoadFrom);

    private void LoadFrom(string filePath)
    {
        Save();
        Load(filePath);
    }

    public void SaveAs()
    {
        _persistenceManager.SaveAs(_filePath, SaveAs);
    }

    private void SaveAs(string filePath)
    {
        _filePath = filePath;
        Save();
    }

    public void Save()
    {
        OnConfigurationSynch?.Invoke(SynchDirection.SaveConfiguration, Model);

        _persistenceManager.Save(_filePath, Model);
    }

    private static string GetConfigurationFilePath(string argIniFile)
    {
        if (!string.IsNullOrEmpty(argIniFile))
        {
            return argIniFile;
        }

        const string iniFileName = "coco.json";

        string appDataPath = GetApplicationFolder();

        return Path.Combine(appDataPath, iniFileName);
    }

    private static string GetApplicationFolder()
    {
        const string vccFolder = "VCCSharp";

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        //==> C:\Users\erich\AppData\Roaming\VCC
        appDataPath = Path.Combine(appDataPath, vccFolder);

        if (!Directory.Exists(appDataPath))
        {
            try
            {
                Directory.CreateDirectory(appDataPath);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Unable to create application data folder: {appDataPath}");
                //TODO: And still use appDataPath?
            }
        }

        return appDataPath;
    }
}
