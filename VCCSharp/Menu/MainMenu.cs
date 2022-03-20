using Ninject;
using System;
using System.Collections.ObjectModel;
using VCCSharp.IoC;

namespace VCCSharp.Menu;

public interface IMainMenu
{
    public MenuItemViewModel Plugins { get; }
}

public class MainMenu : MenuItems, IMainMenu
{
    /// <summary>
    /// Menu Separator
    /// </summary>
    public static readonly MenuItemViewModel ______________ = new SeparatorItemViewModel();

    public MenuItemViewModel Plugins { get; } = new();

    private readonly IViewModelFactory _factory;

    //--Here for XAML template purposes only
    public MainMenu()
    {
        _factory = Factory.Instance.Get<IViewModelFactory>();
    }

    [Inject]
    public MainMenu(Actions actions) : this()
    {
        //--The "plug-ins" menu is dynamic that plug-ins can customize it
        Plugins = Cartridge();

        Add(File(actions));
        Add(Edit(actions));
        Add(Configuration(actions));
        Add(Plugins);
        Add(Help(actions));
    }

    private MenuItemViewModel Menu(string header, Action action) => _factory.CreateMenuItemViewModel(header, action);

    private MenuItemViewModel File(Actions actions) => new()
    {
        Header = "_File",
        MenuItems = new MenuItems
        {
            Menu("Run", actions.Run),
            Menu("Save Config", actions.SaveConfiguration),
            Menu("Load Config", actions.LoadConfiguration),
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
        MenuItems = new MenuItems
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
        MenuItems = new MenuItems
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
        MenuItems = new MenuItems
        {
            Menu("About Vcc", actions.AboutVcc)
        }
    };
}