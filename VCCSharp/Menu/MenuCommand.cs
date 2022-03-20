using System;
using System.Windows.Input;
using VCCSharp.Main;

namespace VCCSharp.Menu;

public class MenuCommand : RoutedCommand
{
    public Action Action { get; set; } = () => throw new NotImplementedException();
    public Key Key { get; set; }
    public ModifierKeys Modifier { get; set; } = ModifierKeys.None;

    public MenuCommand(string name) : base(name, typeof(MainWindow)) { }
}
