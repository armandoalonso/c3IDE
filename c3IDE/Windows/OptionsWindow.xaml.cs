using c3IDE.DataAccess;
using c3IDE.Models;
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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : UserControl
    {
        public string DisplayName { get; set; } = "Options";

        public OptionsWindow()
        {
            InitializeComponent();
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {

        }

        private async void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await AppData.Insatnce.ShowDialog("Delete saved addon data", "All saved addon's will be deleted");

            if (result)
            {
                DataAccessFacade.Insatnce.AddonData.ResetCollection();
                AppData.Insatnce.CurrentAddon = null;
                AppData.Insatnce.AddonList = new List<C3Addon>();
            }
        }
    }
}
