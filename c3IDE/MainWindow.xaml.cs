using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using c3IDE.Compiler;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Windows;
using c3IDE.Windows.Interfaces;
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
                this.Title = $"C3IDE - {s}";
                AddonLoadDelegate();
            });
            OptionsManager.OptionChangedCallback = OptionChanged;
            WindowManager.OpenFindAndReplace = OpenFindAndReplace;
            WindowManager.SetWindowChangeCallback(NavigateToWindow);
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
                ProcessHelper.Insatnce.StartProcess("chrome.exe","https://www.construct.net/en/make-games/manuals/addon-sdk");
                return;
            }

            //call exit on curretn window
            WindowManager.CurrentWindow.OnExit();


            if (AddonManager.CurrentAddon != null)
            {
                //update file index for search
                Searcher.Insatnce.UpdateFileIndex(AddonManager.CurrentAddon, WindowManager.CurrentWindow);
                Searcher.Insatnce.ParseAddon(AddonManager.CurrentAddon);
                switch (clickedLabel)
                {
                    case "Dashboard":
                        WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
                        break;
                    case "Addon":
                        if (AddonManager.CurrentAddon.Type == PluginType.Effect)
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.FxAddonWindow);
                        }
                        else
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.AddonWindow);
                        }
                        break;
                    case "Plugin":
                        WindowManager.ChangeWindow(ApplicationWindows.PluginWindow);
                        break;
                    case "Type":
                       WindowManager.ChangeWindow(ApplicationWindows.TypeWindow);
                        break;
                    case "Instance":
                        WindowManager.ChangeWindow(ApplicationWindows.InstanceWindow);
                        break;
                    case "Actions":
                        WindowManager.ChangeWindow(ApplicationWindows.ActionWindow);
                        break;
                    case "Conditions":
                        WindowManager.ChangeWindow(ApplicationWindows.ConditionWindow);
                        break;
                    case "Expressions":
                        WindowManager.ChangeWindow(ApplicationWindows.ExpressionWindow);
                        break;
                    case "Effect":
                        WindowManager.ChangeWindow(ApplicationWindows.FxCodeWindow);
                        break;
                    case "Language":
                        if (AddonManager.CurrentAddon.Type == PluginType.Effect)
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.FxLanguageWindow);
                        }
                        else
                        {
                            WindowManager.ChangeWindow(ApplicationWindows.LanguageWindow);
                        }
                        break;
                    case "Test":
                        WindowManager.ChangeWindow(ApplicationWindows.TestWidnow);
                        break;
                    case "Options":
                        WindowManager.ChangeWindow(ApplicationWindows.OptionsWindow);
                        break;
                    default:
                        WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
                        break;
                }
            }
            else
            {
                NotificationManager.PublishErrorNotification("No addon loaded. load an addon from the dashboard");
                return;
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
            ActiveItemEffect.Content = window;
            window.OnEnter();
        }

    }
}
