using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using VCCSharp.IoC;
using VCCSharp.Libraries.Models;
using VCCSharp.Menu;
using VCCSharp.Models.Keyboard;
using VCCSharp.Modules;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp
{
    public class MainWindowViewModel
    {
        public MenuItems MenuItems { get; set; }
        public IStatus Status { get; set; }
        public ICommand Cancel { get; set; }
    }

    public partial class MainWindow
    {
        private MainWindowViewModel ViewModel { get; } = new MainWindowViewModel();

        private readonly IFactory _factory = Factory.Instance;
        private readonly IEvents _events;
        private readonly IJoystick _joystick;
        private readonly IKeyboard _keyboard;
        private readonly IClipboard _clipboard;
        private readonly IKeyboardScanCodes _keyboardScanCodes;

        public MainWindow()
        {
            InitializeComponent();

            var modules = _factory.Get<IModules>();
            _events = modules.Events;
            _joystick = modules.Joystick;
            _keyboard = modules.Keyboard;
            _clipboard = modules.Clipboard;

            _keyboardScanCodes = _factory.Get<IKeyboardScanCodes>();

            var bindings = _factory.MainWindowCommands;

            ViewModel.MenuItems = (MenuItems)bindings.MenuItems;
            CommandBindings.AddRange(bindings.CommandBindings);
            InputBindings.AddRange(bindings.InputBindings);

            ViewModel.Status = _factory.Get<IStatus>();

            DataContext = ViewModel;

            //TODO: Seems to get parent window, not surface container
            Window window = GetWindow(Surface);

            if (window == null)
            {
                throw new Exception("Failed to get window object?");
            }

            IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle();

            IVccThread thread = _factory.Get<IVccThread>();

            Task.Run(() => thread.Run(hWnd));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _events.EmuExit();
        }

        private void MainWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _joystick.SetButtonStatus(0, 0);
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _joystick.SetButtonStatus(0, 1);
        }

        private void MainWindow_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _joystick.SetButtonStatus(1, 0);
        }

        private void MainWindow_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _joystick.SetButtonStatus(1, 1);
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            Point mouse = Mouse.GetPosition(this);

            var point = new System.Drawing.Point((int)mouse.X, (int)mouse.Y);

            var clientSize = new RECT
            {
                top = (int)Top,
                left = (int)Left,
                right = (int)Left + (int)Width,
                bottom = (int)Top + (int)Height
            };

            _joystick.SetJoystick(clientSize, point);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            _keyboard.KeyboardHandleKey(e.Key, KeyStates.kEventKeyDown);
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            _keyboard.KeyboardHandleKey(e.Key, KeyStates.kEventKeyUp);
        }
    }
}
