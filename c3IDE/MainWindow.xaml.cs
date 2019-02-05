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
        private readonly DashboardWindow _dashboardWindow;
        private readonly AddonWindow _addonWindow = new AddonWindow();
        private readonly PluginWindow _pluginWindow = new PluginWindow();
        private readonly TypeWindow _typeWindow = new TypeWindow();
        private readonly InstanceWindow _instanceWindow = new InstanceWindow();
        private readonly ActionWindow _actionWindow = new ActionWindow();
        private readonly ConditionWindow _conditionWindow = new ConditionWindow();
        private readonly ExpressionWindow _expressionWindow = new ExpressionWindow();
        private readonly LanguageWindow _languageWindow = new LanguageWindow();
        private readonly TestWindow _testWidnow = new TestWindow();
        private readonly OptionsWindow _optionsWindow = new OptionsWindow();
        private readonly EffectAddonWindow _fxAddonWindow = new EffectAddonWindow();
        private readonly EffectCodeWindow _fxCodeWindow = new EffectCodeWindow();
        private readonly EffectLanguageWindow _fxLanguageWindow = new EffectLanguageWindow();

        private string _currentActiveWindow;

        public MainWindow()
        {
            InitializeComponent();

            //apply default theme
            AppData.Insatnce.SetupTheme();

            //bind notification container
            AppData.Insatnce.InfoMessage = OpenNotification;
            AppData.Insatnce.ErrorMessage = OpenErrorNotification;
            AppData.Insatnce.LoadAddon = s => { this.Title = $"C3IDE - {s}"; };
            AppData.Insatnce.GlobalSave = Save;
            AppData.Insatnce.OptionChanged = OptionChanged;
          

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
            _currentActiveWindow = _dashboardWindow.DisplayName;
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
                    break;
                case "Addon":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _addonWindow;
                    _addonWindow.OnEnter();
                    break;
                case "Plugin":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _pluginWindow;
                    _pluginWindow.OnEnter();
                    break;
                case "Type":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _typeWindow;
                    _typeWindow.OnEnter();
                    break;
                case "Instance":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _instanceWindow;
                    _instanceWindow.OnEnter();
                    break;
                case "Actions":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _actionWindow;
                    _actionWindow.OnEnter();
                    break;
                case "Conditions":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _conditionWindow;
                    _conditionWindow.OnEnter();
                    break;
                case "Expressions":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _expressionWindow;
                    _expressionWindow.OnEnter();
                    break;
                case "Language":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _languageWindow;
                    _languageWindow.OnEnter();
                    break;
                case "Test":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItem.Content = _testWidnow;
                    _testWidnow.OnEnter();
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
            DefaultMainMenu.IsPaneOpen = AppData.Insatnce.Options.PinMenu;
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

                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("addon_json", addon.AddonJson);
                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("runtime_plugin_script", addon.PluginRunTime);
                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("edittime_plugin_script", addon.PluginEditTime);
                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("runtime_type_script", addon.TypeRunTime);
                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("edittime_type_script", addon.TypeEditTime);
                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("runtime_instance_script", addon.InstanceRunTime);
                CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens("edittime_instance_script", addon.InstanceEditTime);

                foreach (var act in addon.Actions)
                {
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{act.Value.Id}_code_script", act.Value.Code);
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{act.Value.Id}_lang_json", act.Value.Language);
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{act.Value.Id}_ace_json", act.Value.Ace);
                }

                foreach (var cnd in addon.Conditions)
                {
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{cnd.Value.Id}_code_script", cnd.Value.Code);
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{cnd.Value.Id}_lang_json", cnd.Value.Language);
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{cnd.Value.Id}_ace_json", cnd.Value.Ace);
                }

                foreach (var exp in addon.Expressions)
                {
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{exp.Value.Id}_code_script", exp.Value.Code);
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{exp.Value.Id}_lang_json", exp.Value.Language);
                    CodeCompletionFactory.Insatnce.PopulateUserDefinedTokens($"{exp.Value.Id}_ace_json", exp.Value.Ace);
                }
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
            switch (_currentActiveWindow)
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
                    break;
                case "Addon":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _fxAddonWindow;
                    _fxAddonWindow.OnEnter();
                    break;
                case "Effect":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _fxCodeWindow;
                    _fxCodeWindow.OnEnter();
                    break;
                case "Language":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _fxLanguageWindow;
                    _fxLanguageWindow.OnEnter();
                    break;
                case "Test":
                    if (!_checkForLoadedAddon()) return;
                    ActiveItemEffect.Content = _testWidnow;
                    _testWidnow.OnEnter();
                    break;
                case "Options":
                    ActiveItemEffect.Content = _optionsWindow;
                    _optionsWindow.OnEnter();
                    break;
                default:
                    ActiveItemEffect.Content = _dashboardWindow;
                    break;
            }

            //set the current active window
            _currentActiveWindow = clickedLabel;
            //close menu pane
            EffectMainMenu.IsPaneOpen = AppData.Insatnce.Options.PinMenu;
        }
    }
}
