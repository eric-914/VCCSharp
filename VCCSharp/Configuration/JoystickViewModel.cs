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

        public JoystickModel Model
        {
            get => _parent == null ? null : Side == JoystickSides.Left ? _parent.LeftModel : _parent.RightModel;
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

        public int UseMouse
        {
            get => Model?.UseMouse ?? 0;
            set => Model.UseMouse = (byte) value;
        }

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
            get => Model?.HiRes ?? 0;
            set => Model.HiRes = (byte)value;
        }

        // Index of which Joystick is selected
        public int DiDevice { get; set; } = 0;

        public byte Up
        {
            get => Model?.Up ?? 0;
            set
            {
                if (Model.Up == value) return;

                Model.Up = value;
                OnPropertyChanged();
            }
        }

        public byte Down
        {
            get => Model?.Down ?? 0;
            set
            {
                if (Model.Down == value) return;

                Model.Down = value;
                OnPropertyChanged();
            }
        }

        public byte Left
        {
            get => Model?.Left ?? 0;
            set
            {
                if (Model.Left == value) return;

                Model.Left = value;
                OnPropertyChanged();
            }
        }

        public byte Right
        {
            get => Model?.Right ?? 0;
            set
            {
                if (Model.Right == value) return;

                Model.Right = value;
                OnPropertyChanged();
            }
        }

        public byte Fire1
        {
            get => Model?.Fire1 ?? 0;
            set
            {
                if (Model.Fire1 == value) return;

                Model.Fire1 = value;
                OnPropertyChanged();
            }
        }

        public byte Fire2
        {
            get => Model?.Fire2 ?? 0;
            set
            {
                if (Model.Fire2 == value) return;

                Model.Fire2 = value;
                OnPropertyChanged();
            }
        }

    }
}
