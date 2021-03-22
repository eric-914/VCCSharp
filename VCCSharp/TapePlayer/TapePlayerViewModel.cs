using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;

namespace VCCSharp.TapePlayer
{
    public class TapePlayerViewModel : INotifyPropertyChanged
    {
        private string _filePath = "Sample Browse File Text";
        private string _mode = "STOP";
        private int _counter;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (value == _filePath) return;

                _filePath = value;
                OnPropertyChanged();
            }
        }

        public string Mode
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
                OnPropertyChanged();
            }
        }
    }
}
