using System.IO;
using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.BitBanger
{
    public class BitBangerManager : IBitBanger
    {
        private readonly IModules _modules;

        private readonly BitBangerViewModel _viewModel = new BitBangerViewModel();
        private BitBangerWindow _view;

        public BitBangerManager(IModules modules)
        {
            _modules = modules;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "AddLineFeed":
                        _modules.MC6821.SetSerialParams(_modules.Config.TextMode);
                        break;

                    case "Print":
                        _modules.MC6821.SetMonState(_modules.Config.PrintMonitorWindow);
                        break;
                }
            };
        }

        public void ShowDialog()
        {
            _viewModel.Config = _modules.Config;

            _view ??= new BitBangerWindow(_viewModel, this);

            _view.Show();
        }

        public void Open()
        {
            string szFileName = _modules.Config.SerialCaptureFile;
            string appPath = Path.GetDirectoryName(szFileName) ?? "C:\\";

            var openFileDlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = szFileName,
                DefaultExt = ".txt",
                Filter = "Text files (.txt)|*.txt",
                InitialDirectory = appPath,
                CheckFileExists = true,
                ShowReadOnly = false,
                Title = "Open print capture file"
            };

            if (openFileDlg.ShowDialog() == true)
            {
                string serialCaptureFile = openFileDlg.FileName;
                if (_modules.MC6821.OpenPrintFile(serialCaptureFile) == 0)
                {
                    MessageBox.Show($"Can't Open File {serialCaptureFile}", "Can't open the file specified.");
                }

                _viewModel.SerialCaptureFile = serialCaptureFile;

                serialCaptureFile = Path.GetFileName(openFileDlg.FileName);

                _modules.Config.SerialCaptureFile = serialCaptureFile;
            }
        }

        public void Close()
        {
            _modules.MC6821.ClosePrintFile();

            _viewModel.SerialCaptureFile = null;

            _modules.Config.PrintMonitorWindow = Define.FALSE;

            _modules.MC6821.SetMonState(_modules.Config.PrintMonitorWindow);

            _viewModel.Print = false;
        }
    }
}
