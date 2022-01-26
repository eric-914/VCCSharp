using System;
using System.Windows;
using VCCSharp.Menu;

namespace VCCSharp.IoC
{
    public interface IViewModelFactory
    {
        MainWindowViewModel CreateMainWindowViewModel(Window window);
        CommandViewModel CreateCommandViewModel(Action action = null);
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IFactory _factory;

        public ViewModelFactory(IFactory factory)
        {
            _factory = factory;
        }

        public MainWindowViewModel CreateMainWindowViewModel(Window window)
        {
            return new MainWindowViewModel
            {
                MenuItems = _factory.Get<IWindowEvents>().Bind(window),
                Status = _factory.Get<IStatus>()
            };
        }

        public CommandViewModel CreateCommandViewModel(Action action = null)
        {
            return new CommandViewModel(action ?? (() => { }));
        }
    }
}
