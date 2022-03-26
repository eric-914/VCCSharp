using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules;

public interface IQuickLoad
{
    int QuickStart(string binFileName);
}

public class QuickLoad : IQuickLoad
{
    private readonly IModules _modules;
    private readonly IConfiguration _configuration;

    public QuickLoad(IModules modules, IConfiguration configuration)
    {
        _modules = modules;
        _configuration = configuration;
    }

    public int QuickStart(string binFileName)
    {
        if (string.IsNullOrEmpty(binFileName))
        {
            return (int)QuickStartStatuses.NoAction;
        }

        if (!File.Exists(binFileName))
        {
            MessageBox.Show($"Cannot find file: {binFileName}");

            return (int)QuickStartStatuses.FileNotFound;
        }

        try
        {
            File.OpenRead(binFileName);
        }
        catch (Exception)
        {
            MessageBox.Show($"Cannot open file: {binFileName}");

            return (int)QuickStartStatuses.CannotOpenFile;
        }

        string extension = Path.GetExtension(binFileName).ToLower();

        var modules = new List<string> { ".rom", ".ccc", ".pak" };

        if (modules.Contains(extension))
        {
            _configuration.Accessories.ModulePath = binFileName;
            _modules.PAKInterface.InsertModule();
        }

        if (extension == ".bin")
        {
            return LoadBinFile(binFileName);
        }

        return (int)QuickStartStatuses.Unknown;
    }

    private int LoadBinFile(string binFileName)
    {
        byte[] memImage;

        try
        {
            memImage = File.ReadAllBytes(binFileName);
        }
        catch (Exception)
        {
            MessageBox.Show("Can't allocate ram", "Error");

            return (int)QuickStartStatuses.OutOfMemory;
        }

        while (true)
        {
            //--Looks like first 5 bytes are special
            byte fileType = memImage[0];
            ushort fileLength = (ushort)(memImage[1] << 8 + memImage[2]);
            ushort startAddress = (ushort)(memImage[3] << 8 + memImage[4]);

            if (fileType != 0x00 && fileType != 0xFF)
            {
                MessageBox.Show(".Bin file is corrupt or invalid", "Error");

                return (int)QuickStartStatuses.InvalidFileType;
            }

            if (fileType == 0)
            {
                for (ushort memIndex = 0; memIndex < fileLength; memIndex++)
                { //Kluge!!!
                    _modules.TC1014.MemWrite8(memImage[memIndex], (ushort)(startAddress + memIndex));
                }
            }
            else
            {
                if (startAddress == 0 || startAddress > 32767 || fileLength != 0)
                {
                    MessageBox.Show(".Bin file is corrupt or invalid Transfer Address", "Error");

                    return (int)QuickStartStatuses.InvalidTransfer;
                }

                _modules.CPU.ForcePC(startAddress);
            }
        }
    }
}