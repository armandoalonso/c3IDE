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
    }
}
