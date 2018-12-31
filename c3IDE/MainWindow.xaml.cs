using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Windows;
using MahApps.Metro.Controls;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Pages
        public DashboardWindow DashboardWin = new DashboardWindow();
        public PluginWindow PluginWin = new PluginWindow();
        #endregion

        public ApplicationOptions Options { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Options = DataAccessFacade.Insatnce.Options.GetAll().First() ?? new ApplicationOptions();
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedLabel = ((HamburgerMenuIconItem)e.ClickedItem).Label;
            switch (clickedLabel)
            {
                case "Dashboard":
                    ActiveItem.Content = DashboardWin;
                    break;
                case "Plugin":
                    ActiveItem.Content = PluginWin;
                    break;
                //case "Properties":
                //    break;
                //case "EditTime JS": 
                //    break;
                //case "RunTime JS":
                //    break;
                //case "Actions":
                //    break;
                //case "Conditions":
                //    break;
                //case "Expressions":
                //    break;
                //case "Language":
                //    break;
                //case "Test":
                //    break;
                //case "Export":
                //    break;
                default:
                    ActiveItem.Content = DashboardWin;
                    break;
            }

            //close menu pane
            MainMenu.IsPaneOpen = false;
        }
    }
}
