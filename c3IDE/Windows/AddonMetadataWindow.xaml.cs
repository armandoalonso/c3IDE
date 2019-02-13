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
using System.Windows.Navigation;
using System.Windows.Shapes;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for AddonMetadataWindow.xaml
    /// </summary>
    public partial class AddonMetadataWindow : UserControl, IWindow
    {
        public AddonMetadataWindow()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; } = "Addon Metadata";
        public void OnEnter()
        {
            throw new NotImplementedException();
        }

        public void OnExit()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        private void Addon_OnDragEnter(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddonIcon_OnDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
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
