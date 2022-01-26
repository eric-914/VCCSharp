using Ninject;
using System.Collections.ObjectModel;

namespace VCCSharp.Menu
{
    public interface IMainMenu
    {
        public MenuItemViewModel Plugins { get; }
    }

    public class MainMenu : MenuItems, IMainMenu
    {
        public static readonly MenuItemViewModel ______________ = new SeparatorItemViewModel(); //--Separator

        public MenuItemViewModel Plugins { get; }

        //--Here for XAML template purposes only
        public MainMenu() { }

        [Inject]
        public MainMenu(Actions actions)
        {
            //--The "plug-ins" menu is dynamic that plug-ins can customize it
            Plugins = Cartridge();

            Add(File(actions));
            Add(Edit(actions));
            Add(Configuration(actions));
            Add(Plugins);
            Add(Help(actions));
        }

        private static MenuItemViewModel File(Actions actions) => new MenuItemViewModel
        {
            Header = "_File",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel { Header = "Run", Action = actions.Run },
                new MenuItemViewModel { Header = "Save Config", Action = actions.SaveConfiguration },
                new MenuItemViewModel { Header = "Load Config", Action = actions.LoadConfiguration }, 
                ______________,
                new MenuItemViewModel { Header = "[F9] Hard Reset", Action = actions.HardReset },
                new MenuItemViewModel { Header = "[F5] Soft Reset", Action = actions.SoftReset }, 
                ______________,
                new MenuItemViewModel { Header = "E_xit", Action = actions.ApplicationExit }
            }
        };

        private static MenuItemViewModel Edit(Actions actions) => new MenuItemViewModel
        {
            Header = "_Edit",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel {Header = "Copy Text", Action = actions.CopyText},
                new MenuItemViewModel {Header = "Paste Text", Action = actions.PasteText},
                new MenuItemViewModel {Header = "Paste BASIC Code (Merge)", Action = actions.PasteBasicCodeMerge},
                new MenuItemViewModel {Header = "Paste BASIC Code (with NEW)", Action = actions.PasteBasicCodeNew}
            }
        };

        private static MenuItemViewModel Configuration(Actions actions) => new MenuItemViewModel
        {
            Header = "_Options",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel { Header = "Flip Artifact Colors", Action = actions.FlipArtifactColors },
                new MenuItemViewModel { Header = "Tape Recorder", Action = actions.TapeRecorder }, 
                ______________,
                new MenuItemViewModel { Header = "Configuration", Action = actions.OpenConfiguration }, 
                ______________,
                new MenuItemViewModel { Header = "Bit Banger", Action = actions.BitBanger }
            }
        };

        private static MenuItemViewModel Cartridge() => new MenuItemViewModel
        {
            Header = "_Cartridge",
            MenuItems = new ObservableCollection<MenuItemViewModel>()
        };

        private static MenuItemViewModel Help(Actions actions) => new MenuItemViewModel
        {
            Header = "_Help",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel {Header = "About Vcc", Action = actions.AboutVcc}
            }
        };
    }
}
