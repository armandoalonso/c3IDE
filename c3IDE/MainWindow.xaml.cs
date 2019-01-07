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
using c3IDE.Windows;
using MahApps.Metro.Controls;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public DashboardWindow dashboardWindow = new DashboardWindow();
        public AddonWindow addonWindow = new AddonWindow();
        private string _currentActiveWindow;

        public MainWindow()
        {
            InitializeComponent();

            //load data
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AppData.Insatnce.CurrentAddon = AppData.Insatnce.AddonList.Any() ? AppData.Insatnce.AddonList.OrderBy(x => x.LastModified).FirstOrDefault() : null;

            //setup default view
            ActiveItem.Content = dashboardWindow;
            dashboardWindow.OnEnter();
            _currentActiveWindow = dashboardWindow.DisplayName;
        }

        private void HambugerMenuItem_Click(object sender, ItemClickEventArgs e)
        {

            //execute on exit
            switch (_currentActiveWindow)
            {
                case "Dashboard":
                    dashboardWindow.OnExit();
                    break;
                case "Addon":
                    addonWindow.OnExit();
                    break;
                //case "Plugin":
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
            }

            var clickedLabel = ((HamburgerMenuIconItem)e.ClickedItem).Label;
            switch (clickedLabel)
            {
                case "Dashboard":
                    ActiveItem.Content = dashboardWindow;
                    dashboardWindow.OnEnter();
                    break;
                case "Addon":
                    ActiveItem.Content = addonWindow;
                    addonWindow.OnEnter();
                    break;
                //case "Plugin":
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
                    ActiveItem.Content = dashboardWindow;
                    break;
            }

            //close menu pane
            MainMenu.IsPaneOpen = false;
        }
    }
}
