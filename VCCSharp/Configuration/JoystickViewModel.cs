using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Enums;

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

        public Models.Configuration.Joystick Model => _parent == null ? null : Side == JoystickSides.Left ? _parent.LeftModel : _parent.RightModel;

        #region Constants

        //for displaying key name
        private static readonly string[] _keyNames = { "", "ESC", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "BackSp", "Tab", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "[", "]", "Bkslash", ";", "'", "Comma", ".", "/", "CapsLk", "Shift", "Ctrl", "Alt", "Space", "Enter", "Insert", "Delete", "Home", "End", "PgUp", "PgDown", "Left", "Right", "Up", "Down", "F1", "F2" };

        public string[] KeyNames => _keyNames;

        public List<string> JoystickNames { get; set; } = new List<string> { "A", "B", "C" };

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
                    UseMouse = value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public JoystickDevices UseMouse
        {
            get => Model?.InputSource.Value ?? JoystickDevices.Mouse;
            set => Model.InputSource.Value = value;
        }

        public JoystickEmulations? Emulation
        {
            get => (JoystickEmulations)HiRes;
            set
            {
                if (value.HasValue)
                {
                    HiRes = value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public JoystickEmulations HiRes
        {
            get => Model?.Type.Value ?? JoystickEmulations.Standard;
            set => Model.Type.Value = value;
        }

        // Index of which Joystick is selected
        public int DiDevice { get; set; } = 0;

        public char Up
        {
            get => Model?.KeyMap.Up ?? (char)0;
            set
            {
                if (Model.KeyMap.Up == value) return;

                Model.KeyMap.Up = value;
                OnPropertyChanged();
            }
        }

        public char Down
        {
            get => Model?.KeyMap.Down ?? (char)0;
            set
            {
                if (Model.KeyMap.Down == value) return;

                Model.KeyMap.Down = value;
                OnPropertyChanged();
            }
        }

        public char Left
        {
            get => Model?.KeyMap.Left ?? (char)0;
            set
            {
                if (Model.KeyMap.Left == value) return;

                Model.KeyMap.Left = value;
                OnPropertyChanged();
            }
        }

        public char Right
        {
            get => Model?.KeyMap.Right ?? (char)0;
            set
            {
                if (Model.KeyMap.Right == value) return;

                Model.KeyMap.Right = value;
                OnPropertyChanged();
            }
        }

        public char Fire1
        {
            get => Model?.KeyMap.Buttons[0] ?? (char)0;
            set
            {
                if (Model.KeyMap.Buttons[0] == value) return;

                Model.KeyMap.Buttons[0] = value;
                OnPropertyChanged();
            }
        }

        public char Fire2
        {
            get => Model?.KeyMap.Buttons[1] ?? (char)0;
            set
            {
                if (Model.KeyMap.Buttons[1] == value) return;

                Model.KeyMap.Buttons[1] = value;
                OnPropertyChanged();
            }
        }
    }
}
