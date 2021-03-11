using System;
using System.Windows.Input;

namespace VCCSharp.Menu
{
    public class MainCommands : CommandBindings
    {
        public MenuCommand F9;


        public MainCommands(Actions actions)
        {
            MenuCommand F(Key key, Action action) => new MenuCommand(key.ToString()) { Key = key, Action = action };

            AddRange(new[]
            {
                F(Key.F3, actions.SlowDown),
                F(Key.F4, actions.SpeedUp),
                F(Key.F5, actions.SoftReset),
                F(Key.F6, actions.ToggleMonitorType),
                F(Key.F7, actions.FlipArtifactColors),
                F(Key.F8, actions.ToggleThrottle),
                F(Key.F9, actions.HardReset),
                F(Key.F10, actions.ToggleInfoBand),
                F(Key.F11, actions.ToggleFullScreen)
            });
        }
    }
}
