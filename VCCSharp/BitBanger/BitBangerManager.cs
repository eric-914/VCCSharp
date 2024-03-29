﻿using System.IO;
using System.Windows;
using VCCSharp.Configuration;
using VCCSharp.IoC;
using VCCSharp.Modules;

namespace VCCSharp.BitBanger;

public class BitBangerManager : IBitBanger
{
    private readonly IModules _modules;
    private readonly IConfigurationManager _configurationManager;
    private readonly BitBangerViewModel _viewModel = new();
    private BitBangerWindow? _view;

    private IConfiguration Model => _modules.Configuration;

    public BitBangerManager(IModules modules, IConfigurationManager configurationManager)
    {
        _modules = modules;
        _configurationManager = configurationManager;

        _viewModel.PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case "AddLineFeed":
                    _modules.MC6821.SetSerialParams(_configurationManager.Model.SerialPort.TextMode);
                    break;

                case "Print":
                    _modules.MC6821.SetMonState(_configurationManager.Model.SerialPort.PrintMonitorWindow);
                    break;
            }
        };
    }

    public void ShowDialog()
    {
        _viewModel.ConfigurationManager = _configurationManager;

        _view ??= new BitBangerWindow(_viewModel, this);

        _view.Show();
    }

    public void Open()
    {
        string? filename = Model.SerialPort.SerialCaptureFile;
        string appPath = Path.GetDirectoryName(filename) ?? "C:\\";

        var openFileDlg = new Microsoft.Win32.OpenFileDialog
        {
            FileName = filename,
            DefaultExt = ".txt",
            Filter = "Text files (.txt)|*.txt",
            InitialDirectory = appPath,
            CheckFileExists = true,
            ShowReadOnly = false,
            Title = "Open print capture file"
        };

        if (openFileDlg.ShowDialog() == true)
        {
            string? serialCaptureFile = openFileDlg.FileName;

            if (serialCaptureFile == null)
            {
                throw new Exception("Invalid serial capture file name");
            }

            if (_modules.MC6821.OpenPrintFile(serialCaptureFile) == 0)
            {
                MessageBox.Show($"Can't Open File {serialCaptureFile}", "Can't open the file specified.");
            }

            _viewModel.SerialCaptureFile = serialCaptureFile;

            serialCaptureFile = Path.GetFileName(openFileDlg.FileName);

            Model.SerialPort.SerialCaptureFile = serialCaptureFile;
        }
    }

    public void Close()
    {
        _modules.MC6821.ClosePrintFile();

        _viewModel.SerialCaptureFile = null;

        Model.SerialPort.PrintMonitorWindow = false;

        _modules.MC6821.SetMonState(Model.SerialPort.PrintMonitorWindow);

        _viewModel.Print = false;
    }
}
