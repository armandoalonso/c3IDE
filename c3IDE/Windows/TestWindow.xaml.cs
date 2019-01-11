using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using c3IDE.Compiler;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : UserControl, IWindow
    {
        public TestWindow()
        {
            InitializeComponent();
        }

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

                //there was an error detected in complication
                if (!AddonCompiler.Insatnce.IsCompilationValid)
                {
                    //todo: error notification
                    StopWebServerButton.IsEnabled = false;
                    StartAndTestButton.IsEnabled = true;
                }
            });

            //add url to clipboard
            Clipboard.SetText("http://localhost:8080/addon.json");
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

        public string DisplayName { get; set; } = "Test";
        public void OnEnter()
        {

        }

        public void OnExit()    
        {
            
        }
    }
}
