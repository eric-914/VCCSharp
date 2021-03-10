namespace VCCSharp.Menu
{
    public class MainMenu : MenuItems
    {
        private static readonly MenuItemViewModel Separator = new SeparatorItemViewModel();

        public MainMenu(Actions actions)
        {
            Add(File(actions));
            Add(Edit(actions));
            Add(Configuration(actions));
            Add(Cartridge(actions));
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
            Header = "Configuration",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel {Header = "Flip Artifact Colors", Action = actions.FlipArtifactColors},
                new MenuItemViewModel {Header = "Config", Action = actions.OpenConfiguration}
            }
        };

        private static MenuItemViewModel Cartridge(Actions actions) => new MenuItemViewModel
        {
            Header = "Cartridge",
            MenuItems = new MenuItems
            {
                new MenuItemViewModel
                {
                    Header = "Cartridge",
                    MenuItems = new MenuItems
                    {
                        new MenuItemViewModel {Header = "Load Cart", Action = actions.LoadCartridge},
                        new MenuItemViewModel {Header = "Eject Cart: Mega-Bug (1982) (26-3076) (Tandy).ccc", Action = actions.EjectCartridge}
                    }
                }
            }
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
