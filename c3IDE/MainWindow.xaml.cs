using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.Compiler;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Eventing;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Windows;
using c3IDE.Windows.Interfaces;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Theme = c3IDE.Utilities.ThemeEngine.Theme;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public readonly DashboardWindow _dashboardWindow;
        public readonly AddonWindow _addonWindow = new AddonWindow();
        public readonly PluginWindow _pluginWindow = new PluginWindow();
        public readonly TypeWindow _typeWindow = new TypeWindow();
        public readonly InstanceWindow _instanceWindow = new InstanceWindow();
        public readonly ActionWindow _actionWindow = new ActionWindow();
        public readonly ConditionWindow _conditionWindow = new ConditionWindow();
        public readonly ExpressionWindow _expressionWindow = new ExpressionWindow();
        public readonly LanguageWindow _languageWindow = new LanguageWindow();
        public readonly TestWindow _testWidnow = new TestWindow();
        public readonly OptionsWindow _optionsWindow = new OptionsWindow();
        public readonly EffectAddonWindow _fxAddonWindow = new EffectAddonWindow();
        public readonly EffectCodeWindow _fxCodeWindow = new EffectCodeWindow();
        public readonly EffectLanguageWindow _fxLanguageWindow = new EffectLanguageWindow();
        public PopoutCompileWindow _popoutCompileWindow;
        public readonly SearchAndReplaceWindow _findAndReplaceWindow = new SearchAndReplaceWindow();

        private IWindow _currentActiveWindow;

        public MainWindow()
        {
            InitializeComponent();

            AppData.Insatnce.MainWindow = this;
            AppData.Insatnce.NavigateToWindow = NavigateToWindow;

            //apply default theme
            AppData.Insatnce.SetupTheme();

            //bind notification container
            AppData.Insatnce.InfoMessage = OpenNotification;
            AppData.Insatnce.ErrorMessage = OpenErrorNotification;
            AppData.Insatnce.LoadAddon = s => { this.Title = $"C3IDE - {s}"; };
            AppData.Insatnce.GlobalSave = Save;
            AppData.Insatnce.OptionChanged = OptionChanged;
            AppData.Insatnce.OpenFindAndReplace = OpenFindAndReplace;
          
            //load data
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AppData.Insatnce.ShowDialog = ShowDialogBox;
            AppData.Insatnce.ShowInputDialog = ShowInputDialogBox;

            //setup load action callback
            _dashboardWindow = new DashboardWindow { AddonLoaded = AddonLoadDelegate };

            //apply pinned menu
            OptionChanged(AppData.Insatnce.Options);

            //setup default view
            ActiveItem.Content = _dashboardWindow;
            _dashboardWindow.OnEnter();
            _currentActiveWindow = _dashboardWindow;
        }

        private void OpenFindAndReplace(IEnumerable<SearchResult> records, IWindow window)
        {
            //execute on exit
            switch (_currentActiveWindow.DisplayName)
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
                case "Test":
                    _testWidnow.OnExit();
                    break;
                case "Options":
                    _optionsWindow.OnExit();
                    break;
            }

            ActiveItem.Content = _findAndReplaceWindow;
            _findAndReplaceWindow.PopulateSearchData(records);
            _findAndReplaceWindow.RestoreWindow = () =>
            {
                ActiveItem.Content = window;
                window.OnEnter();
            };
        }

        private void OptionChanged(Options obj)
        {
            if (obj.PinMenu)
            {
                DefaultMainMenu.DisplayMode = SplitViewDisplayMode.Inline;
                DefaultMainMenu.IsPaneOpen = true;
                DefaultMainMenu.HamburgerVisibility = Visibility.Collapsed;
                DefaultMainMenu.HamburgerHeight = 0;

                EffectMainMenu.DisplayMode = SplitViewDisplayMode.Inline;
                EffectMainMenu.IsPaneOpen = true;
                EffectMainMenu.HamburgerVisibility = Visibility.Collapsed;
                EffectMainMenu.HamburgerHeight = 0;
            }
            else
            {
                DefaultMainMenu.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                DefaultMainMenu.IsPaneOpen = false;
                DefaultMainMenu.HamburgerVisibility = Visibility.Visible;
                DefaultMainMenu.HamburgerHeight = 48;

                EffectMainMenu.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                EffectMainMenu.IsPaneOpen = false;
                EffectMainMenu.HamburgerVisibility = Visibility.Visible;
                EffectMainMenu.HamburgerHeight = 48;
            }
        }

        private void HambugerMenuItem_Click(object sender, ItemClickEventArgs e)
        {
            var clickedLabel = ((HamburgerMenuIconItem)e.ClickedItem).Label;
            //short circut saving and shutdown
            if (clickedLabel == "Save")
            {
                Save();
                DefaultMainMenu.IsPaneOpen = AppData.Insatnce.Options.PinMenu;
                return;
            }

            if (clickedLabel == "Exit")
            {
                Save();
                Close();
            }

            if (clickedLabel == "SDK Help")
            {
                ProcessHelper.Insatnce.StartProcess("chrome.exe","https://www.construct.net/en/make-games/manuals/addon-sdk");
                return;
            }

            //execute on exit
            switch (_currentActiveWindow.DisplayName)
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
                case "Test":
                    _testWidnow.OnExit();
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
                    _currentActiveWindow = _dashboardWindow;
                    break;
                case "Addon":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _addonWindow;
                    _addonWindow.OnEnter();
                    _currentActiveWindow = _addonWindow;
                    break;
                case "Plugin":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _pluginWindow;
                    _pluginWindow.OnEnter();
                    _currentActiveWindow = _pluginWindow;
                    break;
                case "Type":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _typeWindow;
                    _typeWindow.OnEnter();
                    _currentActiveWindow = _typeWindow;
                    break;
                case "Instance":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _instanceWindow;
                    _instanceWindow.OnEnter();
                    _currentActiveWindow = _instanceWindow;
                    break;
                case "Actions":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _actionWindow;
                    _actionWindow.OnEnter();
                    _currentActiveWindow = _actionWindow;
                    break;
                case "Conditions":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _conditionWindow;
                    _conditionWindow.OnEnter();
                    _currentActiveWindow = _conditionWindow;
                    break;
                case "Expressions":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _expressionWindow;
                    _expressionWindow.OnEnter();
                    _currentActiveWindow = _expressionWindow;
                    break;
                case "Language":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _languageWindow;
                    _languageWindow.OnEnter();
                    _currentActiveWindow = _languageWindow;
                    break;
                case "Test":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _testWidnow;
                    _testWidnow.OnEnter();
                    _currentActiveWindow = _testWidnow;
                    break;
                case "Options":
                    ActiveItem.Content = _optionsWindow;
                    _optionsWindow.OnEnter();
                    _currentActiveWindow = _optionsWindow;
                    break;
                default:
                    ActiveItem.Content = _dashboardWindow;
                    _currentActiveWindow = _dashboardWindow;
                    break;
            }

            //close menu pane
            DefaultMainMenu.IsPaneOpen = AppData.Insatnce.Options.PinMenu;
        }

        public void Save()
        {
            //make sure we save current window
            switch (_currentActiveWindow.DisplayName)
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
                case "Test":
                    _testWidnow.OnExit();
                    break;
                case "Options":
                    _optionsWindow.OnExit();
                    break;
            }

            if (AppData.Insatnce.CurrentAddon != null)
            {
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
                OpenNotification($"Saved {AppData.Insatnce.CurrentAddon.Name} successfully.");

                if (AppData.Insatnce.Options.CompileOnSave)
                {
                    if (!ControlHelper.Insatnce.IsWindowOpen<PopoutCompileWindow>())
                    {
                        //open popout compile window
                        _popoutCompileWindow = new PopoutCompileWindow();
                        _popoutCompileWindow.Show();
                    }

                    _popoutCompileWindow.LogText.Text = string.Empty;
                    _popoutCompileWindow.LogText.Text = "starting addon compliation...";
                    Task.Run(() => AddonCompiler.Insatnce.CompileAddon(AppData.Insatnce.CurrentAddon, false));
                }
            }    
        }

        public async Task<bool> ShowDialogBox(string title, string message)
        {
            var result = await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
            return result == MessageDialogResult.Affirmative ? true : false;
        }

        public async Task<string> ShowInputDialogBox(string title, string message, string deafultText)
        {
            var value = await this.ShowInputAsync(title, message, new MetroDialogSettings { DefaultText = deafultText });
            return value;
        }

        public void OpenNotification(string text)
        {
            SuccessNotificationFlyOut.IsOpen = true;
            SuccessNotification.Text = text;
        }

        public void OpenErrorNotification(string text)
        {
            ErrorNotificationFlyOut.IsOpen = true;
            ErrorNotification.Text = text;
        }

        private bool _checkForLoadedAddon()
        {
            if (AppData.Insatnce.CurrentAddon == null)
            {
                OpenErrorNotification($"No addon loaded. Please load addon from dashboard.");
                return false;
            }

            return true;
        }

        public void AddonLoadDelegate()
        {
            //clear out all addons
            foreach (IWindow window in new List<IWindow> {_addonWindow, _actionWindow, _conditionWindow, _expressionWindow, _pluginWindow, _typeWindow, _instanceWindow, _fxAddonWindow, _fxCodeWindow, _fxLanguageWindow})
            {
                window.Clear();
            }

            var addon = AppData.Insatnce.CurrentAddon;

            if (addon.Type == PluginType.Effect)
            {
                DefaultMainMenu.Visibility = Visibility.Collapsed;
                EffectMainMenu.Visibility = Visibility.Visible;
                ActiveItemEffect.Content = _dashboardWindow;

                //todo: do other effect stuff here
            }
            else
            {
                DefaultMainMenu.Visibility = Visibility.Visible;
                EffectMainMenu.Visibility = Visibility.Collapsed;
                ActiveItem.Content = _dashboardWindow;

                CodeCompletionFactory.Insatnce.ParseAddon(addon);
                Searcher.Insatnce.ParseAddon(addon);
            }
        }

        //effect menu
        private void HambugerMenuItemEffect_Click(object sender, ItemClickEventArgs e)
        {
            var clickedLabel = ((HamburgerMenuIconItem)e.ClickedItem).Label;
            //short circut saving and shutdown
            if (clickedLabel == "Save")
            {
                Save();
                EffectMainMenu.IsPaneOpen = AppData.Insatnce.Options.PinMenu;
                return;
            }

            if (clickedLabel == "Exit")
            {
                Save();
                Close();
            }

            if (clickedLabel == "SDK Help")
            {
                ProcessHelper.Insatnce.StartProcess("chrome.exe", "https://www.construct.net/en/make-games/manuals/addon-sdk");
                return;
            }

            //execute on exit
            switch (_currentActiveWindow.DisplayName)
            {
                case "Dashboard":
                    _dashboardWindow.OnExit();
                    break;
                case "Addon":
                    _fxAddonWindow.OnExit();
                    break;
                case "Effect":
                    _fxCodeWindow.OnExit();
                    break;
                case "Language":
                    _fxLanguageWindow.OnExit();
                    break;
                case "Test":
                    _testWidnow.OnExit();
                    break;
                case "Options":
                    _optionsWindow.OnExit();
                    break;
            }

            switch (clickedLabel)
            {
                case "Dashboard":
                    ActiveItemEffect.Content = _dashboardWindow;
                    _dashboardWindow.OnEnter();
                    _currentActiveWindow = _dashboardWindow;
                    break;
                case "Addon":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _fxAddonWindow;
                    _fxAddonWindow.OnEnter();
                    _currentActiveWindow = _fxAddonWindow;
                    break;
                case "Effect":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _fxCodeWindow;
                    _fxCodeWindow.OnEnter();
                    _currentActiveWindow = _fxCodeWindow;
                    break;
                case "Language":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _fxLanguageWindow;
                    _fxLanguageWindow.OnEnter();
                    _currentActiveWindow = _fxLanguageWindow;
                    break;
                case "Test":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _testWidnow;
                    _testWidnow.OnEnter();
                    _currentActiveWindow = _testWidnow;
                    break;
                case "Options":
                    ActiveItemEffect.Content = _optionsWindow;
                    _optionsWindow.OnEnter();
                    _currentActiveWindow = _optionsWindow;
                    break;
                default:
                    ActiveItemEffect.Content = _dashboardWindow;
                    _currentActiveWindow = _dashboardWindow;
                    break;
            }
            //close menu pane
            EffectMainMenu.IsPaneOpen = AppData.Insatnce.Options.PinMenu;
        }

        public void NavigateToWindow(IWindow window)
        {
            _currentActiveWindow.OnExit();
            ActiveItemEffect.Content = window;
            window.OnEnter();
            _currentActiveWindow = window;
        }
    }
}
