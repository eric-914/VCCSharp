using System.Windows;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Menu;
using VCCSharp.Modules;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp
{
    public interface IWindowEvents
    {
        void Bind(Window window, MainWindowCommands commands);
    }

    public class WindowEvents : IWindowEvents
    {
        private readonly IEvents _events;
        private readonly IKeyboard _keyboard;
        private readonly IJoystick _joystick;

        public WindowEvents(IModules modules)
        {
            _events = modules.Events;
            _keyboard = modules.Keyboard;
            _joystick = modules.Joystick;
        }

        /// <summary>
        /// Bind a bunch of of the main window's events
        /// </summary>
        public void Bind(Window window, MainWindowCommands commands)
        {
            window.CommandBindings.AddRange(commands.CommandBindings);
            window.InputBindings.AddRange(commands.InputBindings);

            window.Closing += (o, e) => _events.EmuExit();

            window.KeyDown += (o, e) => _keyboard.KeyboardHandleKey(e.Key, KeyStates.Down);
            window.KeyUp += (o, e) => _keyboard.KeyboardHandleKey(e.Key, KeyStates.Up);
            window.Deactivated += (o, e) => _keyboard.SendSavedKeyEvents();

            window.MouseLeftButtonUp += (o, e) => _joystick.SetButtonStatus(MouseButtonStates.LeftUp);
            window.MouseLeftButtonDown += (o, e) => _joystick.SetButtonStatus(MouseButtonStates.LeftDown);
            window.MouseRightButtonUp += (o, e) => _joystick.SetButtonStatus(MouseButtonStates.RightUp);
            window.MouseRightButtonDown += (o, e) => _joystick.SetButtonStatus(MouseButtonStates.RightDown);
            window.MouseMove += (o, e) => _joystick.SetJoystick(window.RenderSize, Mouse.GetPosition(window));
        }
    }
}
