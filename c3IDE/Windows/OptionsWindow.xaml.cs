using c3IDE.DataAccess;
using c3IDE.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using c3IDE.Utilities;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Options";

        //ctor
        public OptionsWindow()
        {
            InitializeComponent();
        }

        //window states
        public void OnEnter()
        {
            CompilePathText.Text = AppData.Insatnce.Options.CompilePath;
            ExportPathText.Text = AppData.Insatnce.Options.ExportPath;
            DataPathText.Text = AppData.Insatnce.Options.DataPath;
            DefaultAuthorTextBox.Text = AppData.Insatnce.Options.DefaultAuthor;
            DefaultCompanyTextBox.Text = AppData.Insatnce.Options.DefaultCompany;
            C3AddonPathText.Text = AppData.Insatnce.Options.C3AddonPath;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.Options != null)
            {
                AppData.Insatnce.Options = new Options
                {
                    CompilePath = !string.IsNullOrWhiteSpace(CompilePathText.Text) ? CompilePathText.Text : App.DefaultOptions.CompilePath,
                    ExportPath = !string.IsNullOrWhiteSpace(ExportPathText.Text) ? ExportPathText.Text : App.DefaultOptions.ExportPath ,
                    DataPath = !string.IsNullOrWhiteSpace(DataPathText.Text) ? DataPathText.Text : App.DefaultOptions.DataPath,
                    DefaultCompany = !string.IsNullOrWhiteSpace(DefaultCompanyTextBox.Text) ? DefaultCompanyTextBox.Text : App.DefaultOptions.DefaultCompany, 
                    DefaultAuthor = !string.IsNullOrWhiteSpace(DefaultAuthorTextBox.Text) ? DefaultAuthorTextBox.Text : App.DefaultOptions.DefaultAuthor,
                    C3AddonPath = !string.IsNullOrWhiteSpace(C3AddonPathText.Text) ? C3AddonPathText.Text : App.DefaultOptions.C3AddonPath
                };

                //create exports folder if it does not exists
                if (!System.IO.Directory.Exists(AppData.Insatnce.Options.DataPath)) Directory.CreateDirectory(AppData.Insatnce.Options.DataPath);
                if (!System.IO.Directory.Exists(AppData.Insatnce.Options.ExportPath)) Directory.CreateDirectory(AppData.Insatnce.Options.ExportPath);
                if (!System.IO.Directory.Exists(AppData.Insatnce.Options.CompilePath)) Directory.CreateDirectory(AppData.Insatnce.Options.CompilePath);
                if (!System.IO.Directory.Exists(AppData.Insatnce.Options.C3AddonPath)) Directory.CreateDirectory(AppData.Insatnce.Options.C3AddonPath);

                //persist options
                DataAccessFacade.Insatnce.OptionData.Upsert(AppData.Insatnce.Options);
            }
        }

        //buttons
        private async void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await AppData.Insatnce.ShowDialog("Delete saved addon data", "All saved addon's will be deleted");

            if (result)
            {
                DataAccessFacade.Insatnce.AddonData.ResetCollection();
                DataAccessFacade.Insatnce.OptionData.Delete(AppData.Insatnce.Options);
                AppData.Insatnce.CurrentAddon = null;
                AppData.Insatnce.AddonList = new List<C3Addon>();
                AppData.Insatnce.Options = App.DefaultOptions;
            }
        }

        private void OpenCompiledButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Utils.Insatnce.StartProcess(CompilePathText.Text);
            }
            catch (Exception ex)
            {
                AppData.Insatnce.ErrorMessage($"error opening compiled folder, {ex.Message}");
            }
        }

        private void OpenExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Utils.Insatnce.StartProcess(ExportPathText.Text);
            }
            catch (Exception ex)
            {
                AppData.Insatnce.ErrorMessage($"error opening export folder, {ex.Message}");
            }
        }

        private void OpenDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Utils.Insatnce.StartProcess(DataPathText.Text);
            }
            catch (Exception ex)
            {
                AppData.Insatnce.ErrorMessage($"error opening data folder, {ex.Message}");
            }
        }

        private void ResetOptionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            AppData.Insatnce.Options = App.DefaultOptions;
            DataAccessFacade.Insatnce.OptionData.Upsert(AppData.Insatnce.Options);

            //create exports folder if it does not exists
            if (!System.IO.Directory.Exists(AppData.Insatnce.Options.DataPath)) Directory.CreateDirectory(AppData.Insatnce.Options.DataPath);
            if (!System.IO.Directory.Exists(AppData.Insatnce.Options.ExportPath)) Directory.CreateDirectory(AppData.Insatnce.Options.ExportPath);
            if (!System.IO.Directory.Exists(AppData.Insatnce.Options.CompilePath)) Directory.CreateDirectory(AppData.Insatnce.Options.CompilePath);
            if (!System.IO.Directory.Exists(AppData.Insatnce.Options.C3AddonPath)) Directory.CreateDirectory(AppData.Insatnce.Options.C3AddonPath);
        }

        private void OpenAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Utils.Insatnce.StartProcess(C3AddonPathText.Text);
            }
            catch (Exception ex)
            {
                AppData.Insatnce.ErrorMessage($"error opening c3addon folder, {ex.Message}");
            }
        }
    }
}
