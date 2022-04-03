using System;
using System.Windows.Input;

namespace DX8.Tester.Model;

public class ActionCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private readonly Action _action;

    public ActionCommand(Action action)
    {
        _action = action;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty); //--Such a stupid way to avoid CS0067 warning
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
