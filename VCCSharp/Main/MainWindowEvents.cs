using System.Windows;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Menu;
using VCCSharp.Modules;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp.Main
{
    public interface IMainWindowEvents
    {
        void Bind(IMainWindow window, MainWindowCommands commands);
    }

    public class MainWindowEvents : IMainWindowEvents
    {
        private readonly IEvents _events;
        private readonly IKeyboard _keyboard;
        private readonly IJoysticks _joysticks;

        public MainWindowEvents(IModules modules, IEvents events)
        {
            _events = events;
            _keyboard = modules.Keyboard;
            _joysticks = modules.Joysticks;
        }

        /// <summary>
        /// Bind a bunch of of the main window's events
        /// </summary>
        public void Bind(IMainWindow main, MainWindowCommands commands)
        {
            Bind(main.Window, commands);
            Bind(main.View, main.ViewModel);
        }

        private void Bind(Window window, MainWindowCommands commands)
        {
            window.CommandBindings.AddRange(commands.CommandBindings);
            window.InputBindings.AddRange(commands.InputBindings);

            window.Closing += (o, e) => _events.EmuExit();

            window.KeyDown += (o, e) => _keyboard.KeyboardHandleKey(e.Key, KeyStates.Down);
            window.KeyUp += (o, e) => _keyboard.KeyboardHandleKey(e.Key, KeyStates.Up);
            window.Deactivated += (o, e) => _keyboard.SendSavedKeyEvents();

            window.MouseLeftButtonUp += (o, e) => _joysticks.SetButtonStatus(MouseButtonStates.LeftUp);
            window.MouseLeftButtonDown += (o, e) => _joysticks.SetButtonStatus(MouseButtonStates.LeftDown);
            window.MouseRightButtonUp += (o, e) => _joysticks.SetButtonStatus(MouseButtonStates.RightUp);
            window.MouseRightButtonDown += (o, e) => _joysticks.SetButtonStatus(MouseButtonStates.RightDown);
            window.MouseMove += (o, e) => _joysticks.SetJoystick(window.RenderSize, Mouse.GetPosition(window));
        }

        private static void Bind(FrameworkElement view, MainWindowViewModel viewModel)
        {
            view.SizeChanged += (o, e) => viewModel.SurfaceSize = e.NewSize;
        }
    }
}
