namespace DX8.Tester.Model
{
    public interface IJoystickStateViewModel
    {
        IDxJoystickState Joystick { get; }
    }

    public class JoystickStateViewModel : NotifyViewModel, IJoystickStateViewModel
    {
        private IDxJoystickState _joystick = new NullDxJoystickState();

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
    }
}
