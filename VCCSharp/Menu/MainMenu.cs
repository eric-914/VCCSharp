using Ninject;
using VCCSharp.Enums;
using VCCSharp.IoC;

namespace VCCSharp.Menu
{
    public interface IMainMenu
    {
        public MenuItemViewModel Plugins { get; }
    }

    public class MainMenu : MenuItems, IMainMenu
    {
        private static readonly MenuItemViewModel Separator = new SeparatorItemViewModel();

        public MenuItemViewModel Plugins { get; }

        //--Here for XAML template purposes only
        public MainMenu() { }

        [Inject]
        public MainMenu(Actions actions)
        {
            Plugins = Cartridge(actions);

            Add(File(actions));
            Add(Edit(actions));
            Add(Configuration(actions));
            Add(Plugins);
            Add(Help(actions));
        }

        private static MenuItemViewModel File(Actions actions) => new MenuItemViewModel
        {
            Header = "File",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel {Header = "Run", Action = actions.Run},
                new MenuItemViewModel {Header = "Save Config", Action = actions.SaveConfiguration},
                new MenuItemViewModel {Header = "Load Config", Action = actions.LoadConfiguration},
                Separator,
                new MenuItemViewModel {Header = "[F9] Hard Reset", Action = actions.HardReset},
                new MenuItemViewModel {Header = "[F5] Soft Reset", Action = actions.SoftReset},
                Separator,
                new MenuItemViewModel {Header = "Exit", Action = actions.ApplicationExit}
            }
        };

        private static MenuItemViewModel Edit(Actions actions) => new MenuItemViewModel
        {
            Header = "Edit",
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
            Header = "Options",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel {Header = "Flip Artifact Colors", Action = actions.FlipArtifactColors},
                new MenuItemViewModel {Header = "Tape Recorder", Action = actions.TapeRecorder},
                Separator,
                new MenuItemViewModel {Header = "Configuration", Action = actions.OpenConfiguration},
                Separator,
                new MenuItemViewModel {Header = "Bit Banger", Action = actions.BitBanger}
            }
        };

        private static MenuItems CartridgeShared(Actions actions) =>
            new MenuItems
            {
                new MenuItemViewModel
                {
                    Id = MenuActions.Cartridge,
                    Header = "Cartridge",
                    MenuItems = new MenuItems
                    {
                        new MenuItemViewModel
                        {
                            Id=MenuActions.Load,
                            Header = "Load Cart",
                            Action = actions.LoadCartridge
                        },
                        new MenuItemViewModel
                        {
                            Id=MenuActions.Eject,
                            Header = "Eject Cart",
                            Action = actions.EjectCartridge
                        }
                    }
                }
            };

        private static MenuItemViewModel Cartridge(Actions actions) => new MenuItemViewModel
        {
            Header = "Cartridge",
            MenuItems = CartridgeShared(actions)
        };

        private static MenuItemViewModel Help(Actions actions) => new MenuItemViewModel
        {
            Header = "Help",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel {Header = "About Vcc", Action = actions.AboutVcc}
            }
        };
    }
}
