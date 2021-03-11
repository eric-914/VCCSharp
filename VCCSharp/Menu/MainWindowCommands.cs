using System.Linq;
using System.Windows.Input;

namespace VCCSharp.Menu
{
    public class MainWindowCommands
    {
        public MenuItems MenuItems { get; }
        public CommandBindings CommandBindings { get; }
        public InputBindings InputBindings { get; } = new InputBindings();

        public MainWindowCommands(MainMenu menuItems, MainCommands commandBindings)
        {
            MenuItems = menuItems;
            CommandBindings = commandBindings;

            var menuCommands = from each in commandBindings
                let command = each.Command as MenuCommand
                where command != null && command.Key != Key.None
                select command;

            foreach (var each in menuCommands)
            {
                InputBindings.Add(each);
            }
        }
    }
}
