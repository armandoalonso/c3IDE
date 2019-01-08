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
    /// Interaction logic for TypeWindow.xaml
    /// </summary>
    public partial class TypeWindow : UserControl, IWindow
    {
        public TypeWindow()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; } = "Type";
        public void OnEnter()
        {
            EditTimeTypeTextEditor.Text = AppData.Insatnce.CurrentAddon?.TypeEditTime;
            RunTimeTypeTextEditor.Text = AppData.Insatnce.CurrentAddon?.TypeRunTime;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.TypeEditTime = EditTimeTypeTextEditor.Text;
                AppData.Insatnce.CurrentAddon.TypeRunTime = RunTimeTypeTextEditor.Text;
            }
        }
    }
}
