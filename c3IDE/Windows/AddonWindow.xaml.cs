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

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for AddonWindow.xaml
    /// </summary>
    public partial class AddonWindow : UserControl
    {
        public string DisplayName { get; set; } = "Addon";

        public AddonWindow()
        {
            InitializeComponent();
        }

        public void OnEnter()
        {
            AddonTextEditor.Text = AppData.Insatnce.CurrentAddon?.AddonJson;
        }

        public void OnExit()
        {
            if(AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.AddonJson = AddonTextEditor.Text;
            }  
        }
    }
}
