using System;
using System.Windows.Input;

namespace VCCSharp.Menu
{
    public class MenuCommand : RoutedCommand
    {
        public Action Action { get; set; }
        public Key Key { get; set; }

        public MenuCommand(string name) : base(name, typeof(MainWindow)) { }
    }
}
