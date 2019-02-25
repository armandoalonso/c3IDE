using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using c3IDE.Compiler;
using c3IDE.Managers;
using c3IDE.Server;
using c3IDE.Utilities.Helpers;
using c3IDE.Windows.Interfaces;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Test";

        /// <summary>
        /// test window constructor
        /// </summary>
        public TestWindow()
        {
            InitializeComponent();
            LogManager.CompilerLog.AddUpdateCallback((s) =>
            {
                Dispatcher.Invoke(() =>
                {
                    LogText.AppendText(s);
                    if (LogText.LineCount > 0)
                    {
                        LogText.ScrollToLine(LogText.LineCount - 1);
                    }
                });
            });        
        }

        /// <summary>
        /// handles the test window getting focus
        /// </summary>
        public void OnEnter()
        {
            Update();
        }

        /// <summary>
        /// handles the test window losing focus
        /// </summary>
        public void OnExit()
        {
        }

        /// <summary>
        /// clears all test window inputs
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// updates all button states
        /// </summary>
        public void Update()
        {
            StopWebServerButton.IsEnabled = WebServerManager.WebServerStarted;
            StartAndTestButton.IsEnabled = !WebServerManager.WebServerStarted;
            StartWebServerButton.IsEnabled = !WebServerManager.WebServerStarted;
        }

        /// <summary>
        /// compile and test selected addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TestC3AddonButton_Click(object sender, RoutedEventArgs e)
        {
            LogText.Text = string.Empty;
            var isValid = await AddonCompiler.Insatnce.CompileAddon(AddonManager.CurrentAddon);

            //there was an error detected in complication
            if (!isValid)
            {
                return;
            }

            Update();
            UrlTextBox.Text = $"http://localhost:8080/{AddonManager.CurrentAddon.Class.ToLower()}/addon.json";
            Clipboard.SetText(UrlTextBox.Text);
        }

        /// <summary>
        /// started the web server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartWebServerButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AddonCompiler.Insatnce.WebServer = new WebServerClient();
                AddonCompiler.Insatnce.WebServer.Start();
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to start web server => {ex.Message}");
            }

            Update();
        }

        /// <summary>
        /// stop the web server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StopWebServerButton_Click(object sender, RoutedEventArgs e)
        {
            AddonCompiler.Insatnce.WebServer.Stop();
            Update();
        }

        /// <summary>
        /// opens the compiled folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCompiledFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(OptionsManager.CurrentOptions.CompilePath);
        }

        /// <summary>
        /// opens construct on the web or desktop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenConstructButton_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsManager.CurrentOptions.OpenC3InWeb)
            {
                ProcessHelper.Insatnce.StartProcess("chrome.exe", "https://editor.construct.net/");
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(OptionsManager.CurrentOptions.C3DesktopPath))
                    {
                        throw new InvalidOperationException("Construct 3 Desktop Path is Invalid");
                    }

                    ProcessHelper.Insatnce.StartProcess(OptionsManager.CurrentOptions.C3DesktopPath);
                }
                catch (Exception ex)
                {
                    LogManager.AddErrorLog(ex);
                    NotificationManager.PublishErrorNotification("Invalid C3 desktop path, please check plath in options");
                }
            }
        }

        /// <summary>
        /// opens construct using the safe mode lag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenConstructSafeButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessHelper.Insatnce.StartProcess("chrome.exe", "https://editor.construct.net/?safe-mode");
        }

        /// <summary>
        /// handles select all when uri text box is focused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectUrl(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
            var url = UrlTextBox.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                Clipboard.SetText(url);
                NotificationManager.PublishNotification($"{url} copied to clipboard.");
            }
        }

        /// <summary>
        /// focuses text box on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }

        /// <summary>
        /// only compile the selected addon, do not start web server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CompileOnly_OnClick(object sender, RoutedEventArgs e)
        {
            var isValid = await AddonCompiler.Insatnce.CompileAddon(AddonManager.CurrentAddon, false);
        }

        /// <summary>
        /// validate addon 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidateAllFiles_OnClick(object sender, RoutedEventArgs e)
        {
            AddonValidator.Insatnce.UpdateLogText = s => Dispatcher.Invoke(() =>
            {
                LogText.AppendText(s);
                LogText.ScrollToLine(LogText.LineCount - 1);
            });

            AddonValidator.Insatnce.Validate(AddonManager.CurrentAddon);
        }
        
        /// <summary>
        /// create c3ide project file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonManager.CurrentAddon == null)
            {
                NotificationManager.PublishErrorNotification("error exporting c3addon, no c3addon selected");
                return;
            }

            var addonJson = JsonConvert.SerializeObject(AddonManager.CurrentAddon);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var name = OptionsManager.CurrentOptions.IncludeTimeStampOnExport
                ? $"{AddonManager.CurrentAddon.Class}_{timestamp}.c3ide"
                : $"{AddonManager.CurrentAddon.Class}.c3ide";

            ProcessHelper.Insatnce.WriteFile(Path.Combine(OptionsManager.CurrentOptions.ExportPath, name), addonJson);
            ProcessHelper.Insatnce.StartProcess(OptionsManager.CurrentOptions.ExportPath);
        }

        /// <summary>
        /// opens the export folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessHelper.Insatnce.StartProcess(OptionsManager.CurrentOptions.ExportPath);
        }

        /// <summary>
        /// creates c3addon file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateC3AddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonManager.CurrentAddon == null)
            {
                NotificationManager.PublishErrorNotification("error creating c3addon file, no c3addon selected");
                return;
            }
            AddonExporter.Insatnce.ExportAddon(AddonManager.CurrentAddon);
            ProcessHelper.Insatnce.StartProcess(OptionsManager.CurrentOptions.C3AddonPath);
        }
    }
}
