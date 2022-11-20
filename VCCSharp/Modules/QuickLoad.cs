using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules;

public interface IQuickLoad
{
    QuickStartStatuses QuickStart(string binFileName);
}

public class QuickLoad : IQuickLoad
{
    private readonly IModules _modules;

    private IConfiguration Configuration => _modules.Configuration;

    public QuickLoad(IModules modules)
    {
        _modules = modules;
    }

    public QuickStartStatuses QuickStart(string binFileName)
    {
        if (string.IsNullOrEmpty(binFileName))
        {
            return QuickStartStatuses.NoAction;
        }

        if (!File.Exists(binFileName))
        {
            MessageBox.Show($"Cannot find file: {binFileName}");

            return QuickStartStatuses.FileNotFound;
        }

        try
        {
            File.OpenRead(binFileName);
        }
        catch (Exception)
        {
            MessageBox.Show($"Cannot open file: {binFileName}");

            return QuickStartStatuses.CannotOpenFile;
        }

        string extension = Path.GetExtension(binFileName).ToLower();

        var modules = new List<string> { ".rom", ".ccc", ".pak" };

        if (modules.Contains(extension))
        {
            _modules.PAKInterface.InsertModule(binFileName);

            return QuickStartStatuses.Ok;
        }

        if (extension == ".bin")
        {
            return LoadBinFile(binFileName);
        }

        return QuickStartStatuses.Unknown;
    }

    private QuickStartStatuses LoadBinFile(string binFileName)
    {
        byte[] memImage;

        try
        {
            memImage = File.ReadAllBytes(binFileName);
        }
        catch (Exception)
        {
            MessageBox.Show("Can't allocate ram", "Error");

            return QuickStartStatuses.OutOfMemory;
        }

        //--Looks like first 5 bytes are special
        byte fileType = memImage[0];
        ushort fileLength = (ushort)(memImage[1] << 8 + memImage[2]);
        ushort startAddress = (ushort)(memImage[3] << 8 + memImage[4]);

        if (fileType != 0x00 && fileType != 0xFF)
        {
            MessageBox.Show(".Bin file is corrupt or invalid", "Error");

            return QuickStartStatuses.InvalidFileType;
        }

        //TODO: Investigate all this.  It feels incomplete.
        throw new Exception("TODO: Investigate this code if you reached here.");

        if (fileType == 0)
        {
            for (ushort memIndex = 0; memIndex < fileLength; memIndex++)
            {
                //Kluge!!!
                _modules.TCC1014.MemWrite8(memImage[memIndex], (ushort)(startAddress + memIndex));
            }
        }
        else
        {
            if (startAddress == 0 || startAddress > 32767 || fileLength != 0)
            {
                MessageBox.Show(".Bin file is corrupt or invalid Transfer Address", "Error");

                return QuickStartStatuses.InvalidTransfer;
            }

            _modules.CPU.ForcePC(startAddress);
        }

        return QuickStartStatuses.Ok;
    }
}
