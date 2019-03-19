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

        public static List<HamburgerMenuIconItem> SetupMainMenu()
        {
            HamburgerMenuIconItem btnDashboard = new HamburgerMenuIconItem{Icon = new PackIconMaterial{Kind = PackIconMaterialKind.Home}, Label = "Dashboard", Tag = "Dashboard"};
            MainMenu.Add(btnDashboard);
            return MainMenu;
        }
    }
}
