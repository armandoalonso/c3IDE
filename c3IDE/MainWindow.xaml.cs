using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.Compiler;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Windows;
using c3IDE.Windows.Interfaces;
using ControlzEx.Standard;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace c3IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// main window constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            WindowManager.MainWindow = this;
            ThemeManager.SetupTheme();

            //set application callbacks
            AddonManager.AddLoadedCallback((s) =>
            {
                this.Title = $"C3IDE - {s.Name}";
                AddonLoadDelegate();
            });
            OptionsManager.OptionChangedCallback = OptionChanged;
            WindowManager.OpenFindAndReplace = OpenFindAndReplace;
            WindowManager.SetWindowChangeCallback(NavigateToWindow);
            WindowManager.ShowDialog = ShowDialogBox;
            WindowManager.ShowInputDialog = ShowInputDialogBox;
            NotificationManager.SetInfoCallback(OpenNotification);
            NotificationManager.SetErrorCallback(OpenErrorNotification);

            //load data
            AddonManager.LoadAllAddons();

            //setup default view
            ActiveItem.Content = ApplicationWindows.DashboardWindow;
            ApplicationWindows.DashboardWindow.OnEnter();
            WindowManager.CurrentWindow = ApplicationWindows.DashboardWindow;
        }

        /// <summary>
        /// opens find and replace window callback
        /// </summary>
        /// <param name="records"></param>
        /// <param name="window"></param>
        private void OpenFindAndReplace(IEnumerable<SearchResult> records, IWindow window)
        {
            WindowManager.CurrentWindow.OnExit();
            ActiveItem.Content = ApplicationWindows.FindAndReplaceWindow;
            ApplicationWindows.FindAndReplaceWindow.PopulateSearchData(records);
            ApplicationWindows.FindAndReplaceWindow.RestoreWindow = () =>
            {
                ActiveItem.Content = window;
                window.OnEnter();
            };
        }

        /// <summary>
        /// options changed callback handles pinning the menu
        /// </summary>
        /// <param name="obj"></param>
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

        /// <summary>
        /// click on any link in menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HambugerMenuItem_Click(object sender, ItemClickEventArgs e)
        {
            var clickedLabel = ((HamburgerMenuIconItem)e.ClickedItem).Label;

            //short circut saving and shutdown
            if (clickedLabel == "Save")
            {
                Save();
                DefaultMainMenu.IsPaneOpen = OptionsManager.CurrentOptions.PinMenu;
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

            //call exit on current window
            WindowManager.CurrentWindow.OnExit();

            if (AddonManager.CurrentAddon != null)
            {
                //update file index for search
                Searcher.Insatnce.UpdateFileIndex(AddonManager.CurrentAddon, WindowManager.CurrentWindow);
                Searcher.Insatnce.ParseAddon(AddonManager.CurrentAddon);
            }

            switch (clickedLabel)
            {
                case "Dashboard":
                    WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
                    break;
                case "Addon":
                    if (CheckIfAddonLoaded())
                    {
                        if (AddonManager.CurrentAddon.Type == PluginType.Effect)
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.FxAddonWindow);
                        }
                        else
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.AddonWindow);
                        }
                    }
                    break;
                case "Plugin":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.PluginWindow);
                    }
                    
                    break;
                case "Type":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.TypeWindow);
                    }
                    break;
                case "Instance":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.InstanceWindow);
                    }
                    break;
                case "Actions":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.ActionWindow);
                    }
                    break;
                case "Conditions":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.ConditionWindow);
                    }
                    break;
                case "Expressions":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.ExpressionWindow);
                    }
                    break;
                case "Effect":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.FxCodeWindow);
                    }
                    break;
                case "Language":
                    if (CheckIfAddonLoaded())
                    {
                        if (AddonManager.CurrentAddon.Type == PluginType.Effect)
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.FxLanguageWindow);
                        }
                        else
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.LanguageWindow);
                        }
                    }
                    break;
                case "Test":
                    if (CheckIfAddonLoaded())
                    {
                        WindowManager.ChangeWindow(ApplicationWindows.TestWidnow);
                    }
                    break;
                case "Options":
                    WindowManager.ChangeWindow(ApplicationWindows.OptionsWindow);
                    break;
                default:
                    WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
                    break;
            }

            //close menu pane
            DefaultMainMenu.IsPaneOpen = OptionsManager.CurrentOptions.PinMenu;
        }

        /// <summary>
        /// global save
        /// </summary>
        /// <param name="compile"></param>
        public void Save(bool compile = true)
        {
            WindowManager.CurrentWindow.OnExit();

            if (AddonManager.CurrentAddon != null)
            {
                Searcher.Insatnce.UpdateFileIndex(AddonManager.CurrentAddon, WindowManager.CurrentWindow);
                AddonManager.SaveCurrentAddon();
                OpenNotification($"Saved {AddonManager.CurrentAddon.Name} successfully.");

                //parse all addons for new code completions
                CodeCompletionFactory.Insatnce.ParseAddon(AddonManager.CurrentAddon);

                if (OptionsManager.CurrentOptions.CompileOnSave && compile)
                {
                    if (!ControlHelper.Insatnce.IsWindowOpen<PopoutCompileWindow>())
                    {
                        //open popout compile window
                        ApplicationWindows.PopoutCompileWindow = new PopoutCompileWindow();
                        ApplicationWindows.PopoutCompileWindow.Show();
                    }

                    ApplicationWindows.PopoutCompileWindow.LogText.Text = string.Empty;
                    ApplicationWindows.PopoutCompileWindow.LogText.Text = "starting addon compilation...";
                    Task.Run(() => AddonCompiler.Insatnce.CompileAddon(AddonManager.CurrentAddon, false));
                }
            }
        }

        /// <summary>
        /// open information message flyout
        /// </summary>
        /// <param name="text"></param>
        public void OpenNotification(string text)
        {
            SuccessNotificationFlyOut.IsOpen = true;
            SuccessNotification.Text = text;
        }

        /// <summary>
        /// open error message fly out
        /// </summary>
        /// <param name="text"></param>
        public void OpenErrorNotification(string text)
        {
            ErrorNotificationFlyOut.IsOpen = true;
            ErrorNotification.Text = text;
        }

        //todo: parse addon when type is effect
        /// <summary>
        /// callback when an addon is loaded
        /// </summary>
        public void AddonLoadDelegate()
        {
            WindowManager.ClearAllWindows();
            var addon = AddonManager.CurrentAddon;

            if (addon.Type == PluginType.Effect)
            {
                DefaultMainMenu.Visibility = Visibility.Collapsed;
                EffectMainMenu.Visibility = Visibility.Visible;
                ActiveItemEffect.Content = ApplicationWindows.DashboardWindow;
            }
            else
            {
                DefaultMainMenu.Visibility = Visibility.Visible;
                EffectMainMenu.Visibility = Visibility.Collapsed;
                ActiveItemEffect.Content = ApplicationWindows.DashboardWindow;

                CodeCompletionFactory.Insatnce.ParseAddon(addon);
                Searcher.Insatnce.ParseAddon(addon);
            }
        }

        /// <summary>
        /// navigate to window callback
        /// </summary>
        /// <param name="window"></param>
        public void NavigateToWindow(IWindow window)
        {
            var activeItem = AddonManager.CurrentAddon.Type == PluginType.Effect ? ActiveItemEffect : ActiveItem;
            activeItem.Content = window;
            //window.OnEnter();
        }

        /// <summary>
        /// checks if current addon is not null
        /// </summary>
        /// <returns></returns>
        public bool CheckIfAddonLoaded()
        {
            var loaded = AddonManager.CurrentAddon != null;
            if (!loaded)
            {
                NotificationManager.PublishErrorNotification("No addon loaded. load an addon from the dashboard");
            }
            return loaded;
        }

        /// <summary>
        /// when main window is closing close, all childe windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            ApplicationWindows.PopoutCompileWindow?.Close();
        }

        /// <summary>
        /// shows a dialog box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> ShowDialogBox(string title, string message)
        {
            var result = await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
            return result == MessageDialogResult.Affirmative ? true : false;
        }

        /// <summary>
        /// shows a dialog box with an input value
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="deafultText"></param>
        /// <returns></returns>
        public async Task<string> ShowInputDialogBox(string title, string message, string deafultText)
        {
            var value = await this.ShowInputAsync(title, message, new MetroDialogSettings { DefaultText = deafultText });
            return value;
        }
    }
}
