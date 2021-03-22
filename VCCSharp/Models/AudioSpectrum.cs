using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;

namespace VCCSharp.Models
{
    public class AudioSpectrum : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private int _left = 500;
        private int _right = 600;

        public int LeftSpeaker
        {
            get => _left;
            set
            {
                _left = value;
                OnPropertyChanged();
            }
        }

        public int RightSpeaker
        {
            get => _right;
            set
            {
                _right = value;
                OnPropertyChanged();
            }
        }

        public void UpdateSoundBar(ushort left, ushort right)
        {
            LeftSpeaker = left;
            RightSpeaker = right;
            //OnPropertyChanged("LeftSpeaker");
            //OnPropertyChanged("RightSpeaker");
        }
    }
}
