using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;

namespace c3IDE.Managers
{
    public static class MenuManager
    {
        public static List<HamburgerMenuIconItem> MainMenu { get; set; } = new List<HamburgerMenuIconItem>();
        public static List<HamburgerMenuIconItem> EffectMenu { get; set; } = new List<HamburgerMenuIconItem>();

        public static void SetupMainMenu()
        {
            //main menu
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Home }, Label = "Dashboard", Tag = "Dashboard" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.PlusBox }, Label = "Addon", Tag = "Addon" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.PowerPlug }, Label = "Plugin", Tag = "Plugin" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.FileReplace }, Label = "Type", Tag = "Type" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.FileDocument }, Label = "Instance", Tag = "Instance" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.RunFast }, Label = "Actions", Tag = "Actions" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Equal }, Label = "Conditions", Tag = "Conditions" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Function }, Label = "Expressions", Tag = "Expressions" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.GlobeAmericasSolid}, Label = "Language", Tag = "Language" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.CodeSolid }, Label = "C2 Runtime", Tag = "C2Runtime" });
            MainMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconOcticons { Kind = PackIconOcticonsKind.Beaker }, Label = "Test", Tag = "Test" });

            //effect menu
            EffectMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Home }, Label = "Dashboard", Tag = "Dashboard" });
            EffectMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.BookSolid }, Label = "Properties", Tag = "Properties" });
            EffectMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconMaterial { Kind = PackIconMaterialKind.FunctionVariant }, Label = "Parameters", Tag = "Parameters" });
            EffectMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.CodeSolid }, Label = "Code", Tag = "Code" });
            EffectMenu.Add(new HamburgerMenuIconItem { Icon = new PackIconOcticons { Kind = PackIconOcticonsKind.Beaker }, Label = "Test", Tag = "Test" });
        }
    }
}
