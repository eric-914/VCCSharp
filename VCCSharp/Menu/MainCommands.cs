using System;
using System.Windows.Input;

namespace VCCSharp.Menu
{
    public class MainCommands : CommandBindings
    {
        public MainCommands(Actions actions)
        {
            MenuCommand F(Key key, Action action, ModifierKeys modifier = ModifierKeys.None) 
                => new(key.ToString()) { Key = key, Action = action, Modifier = modifier};

            AddRange(new[]
            {
                F(Key.Escape, actions.Cancel),
                //--F1/F2 are reserved for actual function keys on CoCo3 keyboard
                F(Key.F3, actions.SlowDown),
                F(Key.F4, actions.SpeedUp),
                F(Key.F5, actions.SoftReset),
                F(Key.F6, actions.ToggleMonitorType),
                F(Key.F7, actions.FlipArtifactColors),
                F(Key.F8, actions.ToggleThrottle),
                F(Key.F9, actions.HardReset),
                F(Key.F10, actions.ToggleInfoBand),
                F(Key.F11, actions.ToggleFullScreen),
                //--F12 is reserved by Visual Studio --> BREAK (debug mode)

                F(Key.F10, actions.OpenConfiguration, ModifierKeys.Control),
                F(Key.F12, actions.TestIt, ModifierKeys.Control)
            });
        }
    }
}
