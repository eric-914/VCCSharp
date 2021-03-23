using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Models;

namespace VCCSharp.BitBanger
{
    public class BitBangerViewModel : INotifyPropertyChanged
    {
        private const string NO_FILE = "No Capture File";

        //TODO: Remove STATIC once safe
        private static unsafe ConfigState* _state;

        private string _serialCaptureFile = NO_FILE;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public unsafe ConfigState* State
        {
            get => _state;
            set
            {
                if (_state != null) return;

                _state = value;
            }
        }

        public string SerialCaptureFile
        {
            get
            {
                unsafe
                {
                    if (State == null) return string.Empty;

                    string file = Converter.ToString(State->SerialCaptureFile, Define.MAX_PATH);

                    _serialCaptureFile = string.IsNullOrEmpty(file) ? NO_FILE : file;

                    return _serialCaptureFile;
                }
            }
            set
            {
                if (value == _serialCaptureFile) return;

                _serialCaptureFile = value;

                unsafe
                {
                    Converter.ToByteArray(value, State->SerialCaptureFile);
                }

                OnPropertyChanged();
            }
        }

        public bool AddLineFeed
        {
            get
            {
                unsafe
                {
                    return State != null && State->TextMode != Define.FALSE;
                }
            }
            set
            {
                unsafe
                {
                    if (value == (State->TextMode != Define.FALSE)) return;

                    State->TextMode = (value ? Define.TRUE : Define.FALSE);
                    OnPropertyChanged();
                }
            }
        }

        public bool Print
        {
            get
            {
                unsafe
                {
                    return State != null && State->PrintMonitorWindow == Define.TRUE;
                }
            }
            set
            {
                unsafe
                {
                    if (value == (State->PrintMonitorWindow == Define.TRUE)) return;

                    State->PrintMonitorWindow = (value ? Define.TRUE : Define.FALSE);
                    OnPropertyChanged();
                }
            }
        }
    }
}
