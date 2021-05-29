using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp.BitBanger
{
    public class BitBangerViewModel : INotifyPropertyChanged
    {
        private const string NO_FILE = "No Capture File";

        //TODO: Remove STATIC once safe
        private static IConfig _config;

        private string _serialCaptureFile = NO_FILE;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public IConfig Config
        {
            get => _config;
            set
            {
                if (_config != null) return;

                _config = value;
            }
        }

        public string SerialCaptureFile
        {
            get
            {
                if (Config == null) return string.Empty;

                string file = Config.SerialCaptureFile ;

                _serialCaptureFile = string.IsNullOrEmpty(file) ? NO_FILE : file;

                return _serialCaptureFile;
            }
            set
            {
                if (value == _serialCaptureFile) return;

                _serialCaptureFile = value;

                Config.SerialCaptureFile = value;

                OnPropertyChanged();
            }
        }

        public bool AddLineFeed
        {
            get => Config != null && Config.TextMode != Define.FALSE;
            set
            {
                if (value == (Config.TextMode != Define.FALSE)) return;

                Config.TextMode = (value ? Define.TRUE : Define.FALSE);
                OnPropertyChanged();
            }
        }

        public bool Print
        {
            get => Config is {PrintMonitorWindow: Define.TRUE};
            set
            {
                if (value == (Config.PrintMonitorWindow == Define.TRUE)) return;

                Config.PrintMonitorWindow = (value ? Define.TRUE : Define.FALSE);
                OnPropertyChanged();
            }
        }
    }
}
