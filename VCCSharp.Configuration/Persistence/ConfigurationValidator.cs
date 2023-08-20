using System.IO;

namespace VCCSharp.Configuration.Persistence;

public class ConfigurationValidator : IConfigurationValidator
{
    private readonly IConfigurationSystem _system;

    public ConfigurationValidator(IConfigurationSystem system)
    {
        _system = system;
    }

    public void Validate(IConfiguration model)
    {
        string? exePath = Path.GetDirectoryName(_system.GetExecPath());

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
}
