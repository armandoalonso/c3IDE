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
using c3IDE.Windows.Interfaces;
using MahApps.Metro.Controls;
using Utils = c3IDE.Utilities.Utils;

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

            await Task.Run(() =>
            {
                AddonCompiler.Insatnce.CompileAddon(AppData.Insatnce.CurrentAddon);
            });

            //there was an error detected in complication
            if (!AddonCompiler.Insatnce.IsCompilationValid)
            {
                //TODO: error notification
                StopWebServerButton.IsEnabled = false;
                StartAndTestButton.IsEnabled = true;
                return;
            }

            UrlTextBox.Text = $"http://localhost:8080/{AppData.Insatnce.CurrentAddon.Class.ToLower()}/addon.json";

            //add url to clipboard
            Clipboard.SetText(UrlTextBox.Text);
        }

        public void StopWebServerButton_Click(object sender, RoutedEventArgs e)
        {
            AddonCompiler.Insatnce.WebServer.Stop();
            StopWebServerButton.IsEnabled = false;
            StartAndTestButton.IsEnabled = true;
        }

        private void OpenCompiledFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(AppData.Insatnce.Options.CompilePath);
        }

        private void OpenConstructButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.Insatnce.StartProcess("chrome.exe", "https://editor.construct.net/");
        }

        private void OpenConstructSafeButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.Insatnce.StartProcess("chrome.exe", "https://editor.construct.net/?safe-mode");
        }

        //sindow states
        public void OnEnter()
        {

        }

        public void OnExit()    
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
    }
}
