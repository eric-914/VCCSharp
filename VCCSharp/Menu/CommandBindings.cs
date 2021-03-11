using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace VCCSharp.Menu
{
    public class CommandBindings : List<CommandBinding>
    {
        protected void Add(MenuCommand command)
        {
            Add(new CommandBinding(command, Execute, CanExecute));
        }

        protected void AddRange(IEnumerable<MenuCommand> commands)
        {
            foreach (var command in commands)
            {
                Add(command);
            }
        }

        private static void Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command is MenuCommand command)
            {
                command.Action();
            }
        }

        private static void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Source is Control;
        }
    }
}
