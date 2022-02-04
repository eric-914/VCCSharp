using Microsoft.Win32;
using System;
using System.IO;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Modules
{
    public interface IConfigPersistence
    {
        IConfiguration Load(string filePath);
        void LoadFrom(string filePath, Action<string> onContinue);

        void Save(string filePath, IConfiguration model);
        void SaveAs(string filePath, Action<string> onContinue);

        bool IsNew(string filePath);
    }

    /// <summary>
    /// Handles the File I/O of Config
    /// </summary>
    public class ConfigPersistence : IConfigPersistence
    {
        private readonly IModules _modules;
        private readonly IConfigurationPersistence _persistence;

        public ConfigPersistence(IModules modules, IConfigurationPersistence persistence)
        {
            _modules = modules;
            _persistence = persistence;
        }

        public IConfiguration Load(string filePath)
        {
            var model = _persistence.Load(filePath);

            ValidateModel(model);

            return model;
        }

        public void LoadFrom(string filePath, Action<string> onContinue)
        {
            Dialog(filePath, LoadFrom, onContinue);
        }

        public void Save(string filePath, IConfiguration model)
        {
            ValidateModel(model);

            _persistence.Save(filePath, model);
        }

        public void SaveAs(string filePath, Action<string> onContinue)
        {
            Dialog(filePath, SaveAs, onContinue);
        }

        public bool IsNew(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            //  Try to open the config file.  Create it if necessary.  Abort if failure.
            if (File.Exists(filePath))
            {
                return false;
            }

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

        public FileDialog LoadFrom(string filePath)
        {
            return new OpenFileDialog
            {
                FileName = filePath,
                DefaultExt = ".ini",
                Filter = "INI files (.ini)|*.ini",
                InitialDirectory = Path.GetDirectoryName(filePath) ?? "C:\\",
                CheckFileExists = true,
                ShowReadOnly = false,
                Title = "Load Vcc Config File"
            };
        }

        public FileDialog SaveAs(string filePath)
        {
            return new SaveFileDialog
            {
                FileName = filePath,
                DefaultExt = ".ini",
                Filter = "INI files (.ini)|*.ini",
                FilterIndex = 1,
                InitialDirectory = filePath,
                CheckPathExists = true,
                Title = "Save Vcc Config File",
                AddExtension = true
            };
        }

        public void ValidateModel(IConfiguration model)
        {
            string exePath = Path.GetDirectoryName(_modules.Vcc.GetExecPath());

            if (string.IsNullOrEmpty(exePath))
            {
                throw new Exception("Invalid exePath");
            }

            string modulePath = model.Accessories.ModulePath;
            string externalBasicImage = model.Memory.ExternalBasicImage;

            //--If module is in same location as .exe, strip off path portion, leaving only module name

            //--If relative to EXE path, simplify
            if (!string.IsNullOrEmpty(modulePath) && modulePath.StartsWith(exePath))
            {
                modulePath = modulePath[exePath.Length..];
            }

            //--If relative to EXE path, simplify
            if (!string.IsNullOrEmpty(externalBasicImage) && externalBasicImage.StartsWith(exePath))
            {
                externalBasicImage = externalBasicImage[exePath.Length..];
            }

            if (!string.IsNullOrEmpty(modulePath))
            {
                model.Accessories.ModulePath = modulePath;
            }

            model.Memory.SetExternalBasicImage(externalBasicImage);
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
}
