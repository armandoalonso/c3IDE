using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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
using c3IDE.Compiler;
using c3IDE.Server;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using c3IDE.Windows.Interfaces;
using MahApps.Metro.Controls;
using c3IDE.Utilities.ThemeEngine;
using Theme = c3IDE.Utilities.ThemeEngine.Theme;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Test";

        //ctor
        public TestWindow()
        {
            InitializeComponent();
        }

        //button clicks
        private async void TestC3AddonButton_Click(object sender, RoutedEventArgs e)
        {
            //compile the addon
            //LogText.Document.Blocks.Clear();
            LogText.Text = string.Empty;
            AddonCompiler.Insatnce.UpdateLogText = s => Dispatcher.Invoke(() =>
            {
                LogText.AppendText(s);
                LogText.ScrollToLine(LogText.LineCount-1);
            });

            StopWebServerButton.IsEnabled = true;
            StartAndTestButton.IsEnabled = false;
            StartWebServerButton.IsEnabled = false;

            var isValid = await AddonCompiler.Insatnce.CompileAddon(AppData.Insatnce.CurrentAddon);

            //there was an error detected in complication
            if (!isValid)
            {
                //TODO: error notification
                StopWebServerButton.IsEnabled = false;
                StartAndTestButton.IsEnabled = true;
                StartWebServerButton.IsEnabled = true;
                return;
            }

            UrlTextBox.Text = $"http://localhost:8080/{AppData.Insatnce.CurrentAddon.Class.ToLower()}/addon.json";
            Clipboard.SetText(UrlTextBox.Text);
        }

        public void StopWebServerButton_Click(object sender, RoutedEventArgs e)
        {
            AddonCompiler.Insatnce.WebServer.Stop();
            StopWebServerButton.IsEnabled = false;
            StartAndTestButton.IsEnabled = true;
            StartWebServerButton.IsEnabled = true;
        }

        private void OpenCompiledFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(AppData.Insatnce.Options.CompilePath);
        }

        private void OpenConstructButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppData.Insatnce.Options.OpenC3InWeb)
            {
                ProcessHelper.Insatnce.StartProcess("chrome.exe", "https://editor.construct.net/");
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(AppData.Insatnce.Options.C3DesktopPath))
                    {
                        throw new InvalidOperationException("Construct 3 Desktop Path is Invalid");
                    }

                    ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.C3DesktopPath);
                }
                catch (Exception ex)
                {
                    LogManager.Insatnce.Exceptions.Add(ex);
                    AppData.Insatnce.ErrorMessage("Invalid C3 desktop path, please check plath in options");
                }
            }
        }

        private void OpenConstructSafeButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessHelper.Insatnce.StartProcess("chrome.exe", "https://editor.construct.net/?safe-mode");
        }

        //sindow states
        public void OnEnter()
        { 
        }

        public void OnExit()    
        {  
        }

        public void Clear()
        {
        }

        //text box events
        private void SelectUrl(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
            var url = UrlTextBox.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                Clipboard.SetText(url);
                AppData.Insatnce.InfoMessage($"{url} copied to clipboard.");
            }
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }

        private async void CompileOnly_OnClick(object sender, RoutedEventArgs e)
        {
            AddonCompiler.Insatnce.UpdateLogText = s => Dispatcher.Invoke(() =>
            {
                LogText.AppendText(s);
                LogText.ScrollToLine(LogText.LineCount - 1);
            });

            var isValid = AddonValidator.Insatnce.Validate(AppData.Insatnce.CurrentAddon);
            await AddonCompiler.Insatnce.CompileAddon(AppData.Insatnce.CurrentAddon, false);
        }

        private void ValidateAllFiles_OnClick(object sender, RoutedEventArgs e)
        {
            AddonValidator.Insatnce.UpdateLogText = s => Dispatcher.Invoke(() =>
            {
                LogText.AppendText(s);
                LogText.ScrollToLine(LogText.LineCount - 1);
            });

            AddonValidator.Insatnce.Validate(AppData.Insatnce.CurrentAddon);
        }

        private void StartWebServerButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AddonCompiler.Insatnce.UpdateLogText = s => Dispatcher.Invoke(() =>
                {
                    LogText.AppendText(s);
                    LogText.ScrollToLine(LogText.LineCount - 1);
                });
                AddonCompiler.Insatnce.WebServer = new WebServerClient { UpdateLogText = AddonCompiler.Insatnce.UpdateLogText };
                AddonCompiler.Insatnce.WebServer.Start();
                StartAndTestButton.IsEnabled = false;
                StartWebServerButton.IsEnabled = false;
                StopWebServerButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                StopWebServerButton.IsEnabled = false;
                StartAndTestButton.IsEnabled = true;
                StartWebServerButton.IsEnabled = true;
            }
        }
    }
}
