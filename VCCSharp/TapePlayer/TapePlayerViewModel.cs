using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Enums;
using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp.TapePlayer
{
    public class TapePlayerViewModel : INotifyPropertyChanged
    {
        private const string NO_FILE = "EMPTY";

        //TODO: Remove STATIC once safe
        private static unsafe ConfigState* _state;
        private static IConfig _config;

        private string _filePath = "Sample Browse File Text";
        private TapeModes _mode = TapeModes.STOP;
        private int _counter;

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

        public unsafe ConfigState* State
        {
            get => _state;
            set
            {
                if (_state != null) return;

                _state = value;
            }
        }

        public string FilePath
        {
            get
            {
                unsafe
                {
                    if (State == null) return string.Empty;

                    string file = Converter.ToString(State->TapeFileName, Define.MAX_PATH);

                    _filePath = string.IsNullOrEmpty(file) ? NO_FILE : file;

                    return _filePath;
                }
            }
            set
            {
                if (value == _filePath) return;

                _filePath = value;

                unsafe
                {
                    Converter.ToByteArray(value, State->TapeFileName);
                }

                OnPropertyChanged();
            }
        }

        public TapeModes Mode
        {
            get => _mode;
            set
            {
                if (value == _mode) return;

                _mode = value;
                OnPropertyChanged();
            }
        }

        public int Counter
        {
            get => _counter;
            set
            {
                if (value == _counter) return;
                _counter = value;

                Config.TapeCounter = (ushort)value;

                OnPropertyChanged();
            }
        }
    }
}
