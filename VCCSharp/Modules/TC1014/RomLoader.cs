using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules.TC1014;

public interface IRomLoader
{
    void CopyRom(BytePointer irb);
}

public class RomLoader : IRomLoader
{
    private readonly IConfiguration _configuration;
    private readonly IVcc _vcc;

    public RomLoader(IConfiguration configuration, IVcc vcc)
    {
        _configuration = configuration;
        _vcc = vcc;
    }

    public void CopyRom(BytePointer irb)
    {
        const string rom = "coco3.rom";

        //--Try loading from Vcc.ini >> CoCoRomPath
        string cocoRomPath = _configuration.FilePaths.Rom;

        string path = Path.Combine(_configuration.FilePaths.Rom, rom);

        if (LoadInternalRom(path, irb))
        {
            Debug.WriteLine($"Found {rom} in CoCoRomPath");
            return;
        }

        //--Try loading from Vcc.ini >> ExternalBasicImage
        string externalBasicImage = _configuration.Memory.ExternalBasicImage;

        if (!string.IsNullOrEmpty(externalBasicImage) && LoadInternalRom(externalBasicImage, irb))
        {
            Debug.WriteLine($"Found {rom} in ExternalBasicImage");
            return;
        }

        //--Try loading from current executable folder
        string? exePath = Path.GetDirectoryName(_vcc.GetExecPath());
        if (exePath == null)
        {
            throw new Exception("Missing .EXE path?");
        }

        string exeFile = Path.Combine(exePath, rom);

        if (LoadInternalRom(exeFile, irb))
        {
            Debug.WriteLine($"Found {rom} in executable folder");
            return;
        }

        //--Give up...
        string message = @$"
Could not locate {rom} in any of these locations:
* Vcc.ini >> CoCoRomPath=""{cocoRomPath}""
* Vcc.ini >> ExternalBasicImage=""{externalBasicImage}""
* In the same folder as the executable: ""{exePath}""
";

        MessageBox.Show(message, "Error");

        Environment.Exit(0);
    }

    private static bool LoadInternalRom(string filename, BytePointer irb)
    {
        Debug.WriteLine($"LoadInternalRom: {filename}");

        if (!File.Exists(filename)) return false;

        byte[] bytes = File.ReadAllBytes(filename);

        for (ushort index = 0; index < bytes.Length; index++)
        {
            irb[index] = bytes[index];
        }

        return true; //(ushort)bytes.Length;
    }
}
