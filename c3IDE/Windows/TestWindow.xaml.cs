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
using Newtonsoft.Json;
using Path = System.IO.Path;
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

            AppData.Insatnce.CompilerLog.AddUpdateCallback((s) =>
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

            AppData.Insatnce.UpdateTestWindow = Update;
        }

        public void Update()
        {
            StopWebServerButton.IsEnabled = AppData.Insatnce.WebServerStarted;
            StartAndTestButton.IsEnabled = !AppData.Insatnce.WebServerStarted;
            StartWebServerButton.IsEnabled = !AppData.Insatnce.WebServerStarted;
        }

        //button clicks
        private async void TestC3AddonButton_Click(object sender, RoutedEventArgs e)
        {
            //compile the addon
            //LogText.Document.Blocks.Clear();
            LogText.Text = string.Empty;

            var isValid = await AddonCompiler.Insatnce.CompileAddon(AppData.Insatnce.CurrentAddon);

            //there was an error detected in complication
            if (!isValid)
            {
                return;
            }

            Update();
            UrlTextBox.Text = $"http://localhost:8080/{AppData.Insatnce.CurrentAddon.Class.ToLower()}/addon.json";
            Clipboard.SetText(UrlTextBox.Text);
        }

        public void StopWebServerButton_Click(object sender, RoutedEventArgs e)
        {
            AddonCompiler.Insatnce.WebServer.Stop();
            Update();
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
            Update();
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
            var isValid = await AddonCompiler.Insatnce.CompileAddon(AppData.Insatnce.CurrentAddon, false);
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
                AddonCompiler.Insatnce.WebServer = new WebServerClient();
                AddonCompiler.Insatnce.WebServer.Start();
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                AppData.Insatnce.ErrorMessage($"failed to start web server => {ex.Message}");
            }

            Update();
        }

        private void ExportAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AppData.Insatnce.CurrentAddon == null)
            {
                AppData.Insatnce.ErrorMessage("error exporting c3addon, no c3addon selected");
                return;
            }

            var addonJson = JsonConvert.SerializeObject(AppData.Insatnce.CurrentAddon);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var name = AppData.Insatnce.Options.IncludeTimeStampOnExport
                ? $"{AppData.Insatnce.CurrentAddon.Class}_{timestamp}.c3ide"
                : $"{AppData.Insatnce.CurrentAddon.Class}.c3ide";

            ProcessHelper.Insatnce.WriteFile(Path.Combine(AppData.Insatnce.Options.ExportPath, name), addonJson);
            ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.ExportPath);
        }

        private void ExportFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.ExportPath);
        }

        private void CreateC3AddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AppData.Insatnce.CurrentAddon == null)
            {
                AppData.Insatnce.ErrorMessage("error creating c3addon file, no c3addon selected");
                return;
            }
            AddonExporter.Insatnce.ExportAddon(AppData.Insatnce.CurrentAddon);
            ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.C3AddonPath);
        }
    }
}
