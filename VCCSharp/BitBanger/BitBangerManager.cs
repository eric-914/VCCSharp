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

        private static unsafe ConfigState* _configState;

        public BitBangerManager(IModules modules)
        {
            _modules = modules;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "AddLineFeed":
                        _modules.MC6821.MC6821_SetSerialParams(_modules.Config.TextMode);
                        break;

                    case "Print":
                        _modules.MC6821.MC6821_SetMonState(_modules.Config.PrintMonitorWindow);
                        break;
                }
            };
        }

        public unsafe void ShowDialog(ConfigState* state)
        {
            _configState = state;
            _viewModel.Config = _modules.Config;
            _viewModel.State = state;

            _view ??= new BitBangerWindow(_viewModel, this);

            _view.Show();
        }

        public unsafe void Open()
        {
            string szFileName = Converter.ToString(_configState->SerialCaptureFile);
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
                if (_modules.MC6821.MC6821_OpenPrintFile(serialCaptureFile) == 0)
                {
                    MessageBox.Show($"Can't Open File {serialCaptureFile}", "Can't open the file specified.");
                }

                _viewModel.SerialCaptureFile = serialCaptureFile;

                serialCaptureFile = Path.GetFileName(openFileDlg.FileName);

                Converter.ToByteArray(serialCaptureFile, _configState->SerialCaptureFile);
            }
        }

        public void Close()
        {
            _modules.MC6821.MC6821_ClosePrintFile();

            _viewModel.SerialCaptureFile = null;

            _modules.Config.PrintMonitorWindow = Define.FALSE;

            _modules.MC6821.MC6821_SetMonState(_modules.Config.PrintMonitorWindow);

            _viewModel.Print = false;
        }
    }
}
