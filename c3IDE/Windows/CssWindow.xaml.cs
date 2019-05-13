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
    /// Interaction logic for CssWindow.xaml
    /// </summary>
    public partial class CssWindow : UserControl, IWindow
    {
        public CssWindow()
        {
            InitializeComponent();
        }

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public string DisplayName { get; set; }
        public void OnEnter()
        {
            //throw new NotImplementedException();
        }

        public void OnExit()
        {
            //throw new NotImplementedException();
        }

        public void Clear()
        {
            //throw new NotImplementedException();
        }

        public void ChangeTab(string tab, int lineNum)
        {
            //throw new NotImplementedException();
        }
    }
}
