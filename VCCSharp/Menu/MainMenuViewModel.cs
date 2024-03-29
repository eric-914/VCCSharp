﻿using System.Collections.ObjectModel;
using VCCSharp.IoC;

namespace VCCSharp.Menu;

public interface IMainMenu
{
    public MenuItemViewModel Plugins { get; }
}

/// <summary>
/// This is in itself a MenuItems(VM) containing child MenuItems
/// </summary>
public abstract class MainMenuViewModelBase : MenuItemsViewModel, IMainMenu
{
    /// <summary>
    /// Menu Separator
    /// </summary>
    public static readonly MenuItemViewModel ______________ = new SeparatorItemViewModel();

    public MenuItemViewModel Plugins { get; } = new();

    private readonly Func<string, Action, MenuItemViewModel> _menu;

    //--Here for XAML template purposes only
    protected MainMenuViewModelBase()
    {
        _menu = (_,_) => new MenuItemViewModel();
    }

    protected MainMenuViewModelBase(IViewModelFactory factory, Actions actions)
    {
        _menu = factory.CreateMenuItemViewModel;

        //--The "plug-ins" menu is dynamic that plug-ins can customize it
        Plugins = Cartridge();

        Add(File(actions));
        Add(Edit(actions));
        Add(Configuration(actions));
        Add(Plugins);
        Add(Help(actions));
    }

    private MenuItemViewModel Menu(string header, Action action) => _menu(header, action);

    private MenuItemViewModel File(Actions actions) => new()
    {
        Header = "_File",
        MenuItems = new MenuItemsViewModel
        {
            Menu("Run", actions.Run),
            Menu("Save Configuration", actions.SaveConfiguration),
            Menu("Load Configuration", actions.LoadConfiguration),
            ______________,
            Menu("[F9] Hard Reset", actions.HardReset),
            Menu("[F5] Soft Reset", actions.SoftReset),
            ______________,
            Menu("E_xit", actions.ApplicationExit)
        }
    };

    private MenuItemViewModel Edit(Actions actions) => new()
    {
        Header = "_Edit",
        MenuItems = new MenuItemsViewModel
        {
            Menu("Copy Text", actions.CopyText),
            Menu("Paste Text", actions.PasteText),
            Menu("Paste BASIC Code (Merge)", actions.PasteBasicCodeMerge),
            Menu("Paste BASIC Code (with NEW)", actions.PasteBasicCodeNew)
        }
    };

    private MenuItemViewModel Configuration(Actions actions) => new()
    {
        Header = "_Options",
        MenuItems = new MenuItemsViewModel
        {
            Menu("Flip Artifact Colors", actions.FlipArtifactColors),
            Menu("Tape Recorder", actions.TapeRecorder),
            ______________,
            Menu("Configuration", actions.OpenConfiguration),
            ______________,
            Menu("Bit Banger", actions.BitBanger)
        }
    };

    private static MenuItemViewModel Cartridge() => new()
    {
        Header = "_Cartridge",
        MenuItems = new ObservableCollection<MenuItemViewModel>()
    };

    private MenuItemViewModel Help(Actions actions) => new()
    {
        Header = "_Help",
        MenuItems = new MenuItemsViewModel
        {
            Menu("About Vcc", actions.AboutVcc)
        }
    };
}

public class MainMenuViewModelStub : MainMenuViewModelBase
{
    public MainMenuViewModelStub() : base() { }
}

public class MainMenuViewModel : MainMenuViewModelBase
{
    public MainMenuViewModel(Actions actions) : base(Factory.Instance.Get<IViewModelFactory>(), actions) { }
}