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
    /// Interaction logic for InstanceWindow.xaml
    /// </summary>
    public partial class InstanceWindow : UserControl,IWindow
    {
        public InstanceWindow()
        {
            InitializeComponent();
        }

        public string DisplayName { get; set; } = "Instance";
        public void OnEnter()
        {
            EditTimeInstanceTextEditor.Text = AppData.Insatnce.CurrentAddon?.InstanceEditTime;
            RunTimeInstanceTextEditor.Text = AppData.Insatnce.CurrentAddon?.InstanceRunTime;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.InstanceEditTime = EditTimeInstanceTextEditor.Text;
                AppData.Insatnce.CurrentAddon.InstanceRunTime = RunTimeInstanceTextEditor.Text;
            }
        }
    }
}
