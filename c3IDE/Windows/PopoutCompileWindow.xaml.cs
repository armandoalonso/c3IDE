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
using System.Windows.Shapes;
using c3IDE.Compiler;
using c3IDE.Server;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using MahApps.Metro.Controls;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for PopoutCompileWindow.xaml
    /// </summary>
    public partial class PopoutCompileWindow : MetroWindow
    {
        private readonly int callbackIndex;
        public PopoutCompileWindow()
        {
            InitializeComponent();
            AppData.Insatnce.WebServiceUrlChanged = s => Dispatcher.Invoke(() => { UrlTextBox.Text = s; });
            AppData.Insatnce.WebServerStateChanged = b => Dispatcher.Invoke(() =>
            {
                WebServerButton.Content = b ? "Stop Web Server" : "Start Web Server";
            });

            callbackIndex = AppData.Insatnce.CompilerLog.AddUpdateCallback((s) =>
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

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppData.Insatnce.CompilerLog.RemoveCallback(callbackIndex);
        }

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

        private void StartWebServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!AppData.Insatnce.WebServerStarted)
                {
                    AddonCompiler.Insatnce.WebServer = new WebServerClient();
                    AddonCompiler.Insatnce.WebServer.Start();
                }
                else
                {
                    AddonCompiler.Insatnce.WebServer.Stop();
                }
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                AppData.Insatnce.WebServerStarted = false;
            }

            WebServerButton.Content = AppData.Insatnce.WebServerStarted ? "Stop Web Server" : "Start Web Server";
            AppData.Insatnce.UpdateTestWindow();
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
    }
}
