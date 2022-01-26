using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Menu;
using VCCSharp.Modules;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp
{
    public class MainWindowViewModel
    {
        public MenuItems MenuItems { get; set; }
        public IStatus Status { get; set; }
    }

    public partial class MainWindow
    {
        private MainWindowViewModel ViewModel { get; } = new MainWindowViewModel();

        private readonly IFactory _factory = Factory.Instance;

        public MainWindow()
        {
            InitializeComponent();

            var modules = _factory.Get<IModules>();
            IEvents events = modules.Events;
            IKeyboard keyboard = modules.Keyboard;
            IJoystick joystick = modules.Joystick;

            MainWindowCommands bindings = _factory.MainWindowCommands;

            ViewModel.MenuItems = (MenuItems)bindings.MenuItems;
            CommandBindings.AddRange(bindings.CommandBindings);
            InputBindings.AddRange(bindings.InputBindings);

            ViewModel.Status = _factory.Get<IStatus>();

            DataContext = ViewModel;

            Closing += (sender, e) => events.EmuExit();
            KeyDown += (sender, e) => keyboard.KeyboardHandleKey(e.Key, KeyStates.Down);
            KeyUp += (sender, e) => keyboard.KeyboardHandleKey(e.Key, KeyStates.Up);
            Deactivated += (sender, e) => keyboard.SendSavedKeyEvents(); ;
            MouseLeftButtonUp += (sender, e) => joystick.SetButtonStatus(MouseButtonStates.LeftUp);
            MouseLeftButtonDown += (sender, e) => joystick.SetButtonStatus(MouseButtonStates.LeftDown);
            MouseRightButtonUp += (sender, e) => joystick.SetButtonStatus(MouseButtonStates.RightUp);
            MouseRightButtonDown += (sender, e) => joystick.SetButtonStatus(MouseButtonStates.RightDown);
            MouseMove += (sender, e) => joystick.SetJoystick(RenderSize, Mouse.GetPosition(this));

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
    }
}
