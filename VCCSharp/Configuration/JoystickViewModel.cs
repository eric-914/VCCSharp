using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Enums;
using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public class JoystickViewModel : INotifyPropertyChanged
    {
        private readonly ConfigurationViewModel _parent;

        public JoystickViewModel() { }

        //--TODO: Holding a local copy of the correct JoystickModel* ends up with bad pointers for reason unknown
        public JoystickViewModel(JoystickSides side, ConfigurationViewModel parent) : this()
        {
            Side = side;
            _parent = parent;
        }

        public unsafe JoystickModel* Model
        {
            get => _parent == null ? null : Side == JoystickSides.Left ? _parent.Model->Left : _parent.Model->Right;
        }

        #region Constants

        //for displaying key name
        private static readonly string[] _keyNames = { "", "ESC", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "BackSp", "Tab", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "[", "]", "Bkslash", ";", "'", "Comma", ".", "/", "CapsLk", "Shift", "Ctrl", "Alt", "Space", "Enter", "Insert", "Delete", "Home", "End", "PgUp", "PgDown", "Left", "Right", "Up", "Down", "F1", "F2" };

        public string[] KeyNames => _keyNames;

        #endregion

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

        public JoystickDevices? Device
        {
            get => (JoystickDevices)UseMouse;
            set
            {
                if (value.HasValue)
                {
                    UseMouse = (int)value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public int UseMouse { get; set; }

        public JoystickEmulations? Emulation
        {
            get => (JoystickEmulations)HiRes;
            set
            {
                if (value.HasValue)
                {
                    HiRes = (int)value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public int HiRes
        {
            get
            {
                unsafe
                {
                    return Model == null ? 0 : Model->HiRes;
                }
            }
            set
            {
                unsafe
                {
                    Model->HiRes = (byte)value;
                }
            }
        }

        // Index of which Joystick is selected
        public int DiDevice { get; set; } = 0;

        public char Up { get; set; } = 'U';
        public char Down { get; set; } = 'D';
        public char Left { get; set; } = 'L';
        public char Right { get; set; } = 'R';
        public char Fire1 { get; set; } = '1';
        public char Fire2 { get; set; } = '2';
    }
}
