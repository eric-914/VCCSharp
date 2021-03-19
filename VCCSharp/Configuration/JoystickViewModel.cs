using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Enums;
using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public class JoystickViewModel : INotifyPropertyChanged
    {
        public JoystickModel Model { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public string SideText => Side == JoystickSides.Left ? "Left" : "Right";

        public JoystickSides Side { get; set; }

        public JoystickEmulations Emulation { get; set; }

        public bool UseMouse { get; set; } = true;
        public bool DiDevice { get; set; } = false;
        public bool HiRes { get; set; } = true;

        public char Up { get; set; } = 'U';
        public char Down { get; set; } = 'D';
        public char Left { get; set; } = 'L';
        public char Right { get; set; } = 'R';
        public char Fire1 { get; set; } = '1';
        public char Fire2 { get; set; } = '2';
    }
}
