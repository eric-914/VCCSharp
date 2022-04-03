using System.Windows.Input;

namespace VCCSharp.Shared.Commands;

public class ActionCommand : ICommand
{
    public event EventHandler? CanExecuteChanged { add { } remove { } }

    private readonly Action _action;

    public ActionCommand(Action action)
    {
        _action = action;
    }

    public void Execute(object? o)
    {
        _action();
    }

    public bool CanExecute(object? o)
    {
        return true;
    }
}
