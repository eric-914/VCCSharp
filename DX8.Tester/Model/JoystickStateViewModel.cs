namespace DX8.Tester.Model
{
    public interface IJoystickStateViewModel
    {
        IDxJoystickState Joystick { get; }
        void Refresh();
    }

    public class JoystickStateViewModel : NotifyViewModel, IJoystickStateViewModel
    {
        private readonly IDxInput? _input;
        private readonly int _index;

        private IDxJoystickState _joystick = new NullDxJoystickState();

        public JoystickStateViewModel() { }

        public JoystickStateViewModel(IDxInput input, int index)
        {
            _input = input;
            _index = index;
        }

        public IDxJoystickState Joystick
        {
            get => _joystick;
            set
            {
                if (_joystick == value) return;

                _joystick = value;
                OnPropertyChanged();
            }
        }

        public void Refresh()
        {
            if (_input != null)
            {
                Joystick = _input.JoystickPoll(_index);
            }
        }
    }
}
