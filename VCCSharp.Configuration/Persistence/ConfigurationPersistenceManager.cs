using Microsoft.Win32;
using System.IO;

namespace VCCSharp.Configuration.Persistence;

public interface IConfigurationPersistenceManager
{
    IConfiguration Load(string? filePath);
    void LoadFrom(string? filePath, Action<string> onContinue);

    void Save(string? filePath, IConfiguration model);
    void SaveAs(string? filePath, Action<string> onContinue);

    bool IsNew(string? filePath);
}

public class ConfigurationPersistenceManager : IConfigurationPersistenceManager
{
    private readonly IConfigurationPersistence _persistence;
    private readonly IConfigurationValidator _validator;

    public ConfigurationPersistenceManager(IConfigurationPersistence persistence, IConfigurationValidator validator)
    {
        _persistence = persistence;
        _validator = validator;
    }

    public bool IsNew(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        // Try to open the configuration file.  
        if (File.Exists(filePath))
        {
            return false;
        }

        // Create it if necessary. Abort if failure.
        try
        {
            File.WriteAllText(filePath, "");
        }
        catch (Exception e)
        {
            throw new Exception($"Could not write configuration to: {filePath}", e);
        }

        return true;
    }

    public IConfiguration Load(string? filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        var model = _persistence.Load(filePath);

        _validator.Validate(model);

        return model;
    }

    public void LoadFrom(string? filePath, Action<string> onContinue)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        Dialog(filePath, LoadFrom, onContinue);
    }

    public void Save(string? filePath, IConfiguration model)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        _validator.Validate(model);

        _persistence.Save(filePath, model);
    }

    public void SaveAs(string? filePath, Action<string> onContinue)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        Dialog(filePath, SaveAs, onContinue);
    }

    private static FileDialog LoadFrom(string filePath)
    {
        return new OpenFileDialog
        {
            FileName = filePath,
            DefaultExt = ".ini",
            Filter = "JSON files (.json)|*.json",
            InitialDirectory = Path.GetDirectoryName(filePath) ?? "C:\\",
            CheckFileExists = true,
            ShowReadOnly = false,
            Title = "Load Vcc Configuration File"
        };
    }

    private static FileDialog SaveAs(string filePath)
    {
        return new SaveFileDialog
        {
            FileName = filePath,
            DefaultExt = ".json",
            Filter = "JSON files (.json)|*.json",
            FilterIndex = 1,
            InitialDirectory = filePath,
            CheckPathExists = true,
            Title = "Save Vcc Configuration File",
            AddExtension = true
        };
    }

    private static void Dialog(string filePath, Func<string, FileDialog> fn, Action<string> onContinue)
    {
        var dialog = fn(filePath);

        if (dialog.ShowDialog() == true)
        {
            onContinue(dialog.FileName);
        }
    }
}
