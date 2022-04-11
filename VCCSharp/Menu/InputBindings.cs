using System.Windows.Input;

namespace VCCSharp.Menu;

public class InputBindings : List<InputBinding>
{
    public void Add(MenuCommand command)
    {
        Add(new KeyBinding(command, command.Key, command.Modifier));
    }
}
