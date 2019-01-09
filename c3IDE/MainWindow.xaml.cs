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
using MahApps.Metro.Controls.Dialogs;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly DashboardWindow _dashboardWindow = new DashboardWindow();
        private readonly AddonWindow _addonWindow = new AddonWindow();
        private readonly PluginWindow _pluginWindow = new PluginWindow();
        private readonly TypeWindow _typeWindow = new TypeWindow();
        private readonly InstanceWindow _instanceWindow = new InstanceWindow();
        private readonly ActionWindow _actionWindow = new ActionWindow();
        private readonly ConditionWindow _conditionWindow = new ConditionWindow();
        private readonly ExpressionWindow _expressionWindow = new ExpressionWindow();
        private readonly LanguageWindow _languageWindow = new LanguageWindow();
        private readonly OptionsWindow _optionsWindow = new OptionsWindow();

        private string _currentActiveWindow;

        public MainWindow()
        {
            InitializeComponent();

            //load data
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AppData.Insatnce.CurrentAddon = AppData.Insatnce.AddonList.Any() ? AppData.Insatnce.AddonList.OrderBy(x => x.LastModified).FirstOrDefault() : null;
            AppData.Insatnce.ShowDialog = ShowDialogBox;
            AppData.Insatnce.ShowInputDialog = ShowInputDialogBox;

            //setup default view
            ActiveItem.Content = _dashboardWindow;
            _dashboardWindow.OnEnter();
            _currentActiveWindow = _dashboardWindow.DisplayName;
        }

        private void HambugerMenuItem_Click(object sender, ItemClickEventArgs e)
        {
            var clickedLabel = ((HamburgerMenuIconItem)e.ClickedItem).Label;
            //short circut saving and shutdown
            if (clickedLabel == "Save")
            {
                Save();
                MainMenu.IsPaneOpen = false;
                return;
            }

            //execute on exit
            switch (_currentActiveWindow)
            {
                case "Dashboard":
                    _dashboardWindow.OnExit();
                    break;
                case "Addon":
                    _addonWindow.OnExit();
                    break;
                case "Plugin":
                    _pluginWindow.OnExit();
                    break;
                case "Type":
                    _typeWindow.OnExit();
                    break;
                case "Instance":
                    _instanceWindow.OnExit();
                    break;
                case "Actions":
                    _actionWindow.OnExit();
                    break;
                case "Conditions":
                    _conditionWindow.OnExit();
                    break;
                case "Expressions":
                    _expressionWindow.OnExit();
                    break;
                case "Language":
                    _languageWindow.OnExit();
                   break;
                 case "Options":
                    _optionsWindow.OnExit();
                    break;
            }

            switch (clickedLabel)
            {
                case "Dashboard":
                    ActiveItem.Content = _dashboardWindow;
                    _dashboardWindow.OnEnter();
                    break;
                case "Addon":
                    ActiveItem.Content = _addonWindow;
                    _addonWindow.OnEnter();
                    break;
                case "Plugin":
                    ActiveItem.Content = _pluginWindow;
                    _pluginWindow.OnEnter();
                    break;
                case "Type":
                    ActiveItem.Content = _typeWindow;
                    _typeWindow.OnEnter();
                    break;
                case "Instance":
                    ActiveItem.Content = _instanceWindow;
                    _instanceWindow.OnEnter();
                    break;
                case "Actions":
                    ActiveItem.Content = _actionWindow;
                    _actionWindow.OnEnter();
                    break;
                case "Conditions":
                    ActiveItem.Content = _conditionWindow;
                    _conditionWindow.OnEnter();
                    break;
                case "Expressions":
                    ActiveItem.Content = _expressionWindow;
                    _expressionWindow.OnEnter();
                    break;
                case "Language":
                    ActiveItem.Content = _languageWindow;
                    _languageWindow.OnEnter();
                    break;
                case "Options":
                    ActiveItem.Content = _optionsWindow;
                    _optionsWindow.OnEnter();
                    break;
                default:
                    ActiveItem.Content = _dashboardWindow;
                    break;
            }

            //set the current active window
            _currentActiveWindow = clickedLabel;
            //close menu pane
            MainMenu.IsPaneOpen = false;
        }

        public void Save()
        {
            //make sure we save current window
            switch (_currentActiveWindow)
            {
                case "Dashboard":
                    _dashboardWindow.OnExit();
                    break;
                case "Addon":
                    _addonWindow.OnExit();
                    break;
                case "Plugin":
                    _pluginWindow.OnExit();
                    break;
                case "Type":
                    _typeWindow.OnExit();
                    break;
                case "Instance":
                    _instanceWindow.OnExit();
                    break;
                case "Actions":
                    _actionWindow.OnExit();
                    break;
                case "Conditions":
                    _conditionWindow.OnExit();
                    break;
                case "Expressions":
                    _expressionWindow.OnExit();
                    break;
                case "Language":
                    _languageWindow.OnExit();
                    break;
                case "Options":
                    _optionsWindow.OnExit();
                    break;
            }

            if (AppData.Insatnce.CurrentAddon != null)
            {
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
            //todo: add flyout for notifications
        }

        public async Task<bool> ShowDialogBox(string title, string message)
        {
            var result = await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
            return result == MessageDialogResult.Affirmative ? true : false;
        }

        public async Task<string> ShowInputDialogBox(string title, string message, string deafultText)
        {
            var value = await this.ShowInputAsync(title, message, new MetroDialogSettings {DefaultText = deafultText });
            return value;
        }
    }
}
