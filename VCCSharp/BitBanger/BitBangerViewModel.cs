using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;

namespace VCCSharp.BitBanger
{
    public class BitBangerViewModel: INotifyPropertyChanged
    {
        private string _filePath = "Sample Browse File Text";
        private bool _addLineFeed;
        private bool _print;

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

        public bool AddLineFeed
        {
            get => _addLineFeed;
            set
            {
                if (value == _addLineFeed) return;
                _addLineFeed = value;
                OnPropertyChanged();
            }
        }

        public bool Print
        {
            get => _print;
            set
            {
                if (value == _print) return;
                _print = value;
                OnPropertyChanged();
            }
        }
    }
}
