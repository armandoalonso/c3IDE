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
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using c3IDE.Utilities.ThemeEngine;
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
            ControlHelper.Insatnce.PopulateComboBox(FontSizeCombo, 
                8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f, 17f ,18f, 19f, 20f, 21f, 22f, 23f, 24f, 25f);

            ControlHelper.Insatnce.PopulateComboBox(FontFamilyCombo, "Consolas", "Calibri", "Cambria", "Candara",
                "Constantia", "Corbel", "Lucida Console, Monaco, monospace", "Courier New, Courier, monospace", "Arial, Helvetica, sans-serif",
                "Courier New", "Georgia, serif", "Trebuchet MS, Helvetica, sans-serif", "Verdana, Geneva, sans-serif", "Tahoma, Geneva, sans-serif",
                "Palatino Linotype, Book Antiqua, Palatino, serif", "Lucida Sans Unicode, Lucida Grande, sans-serif");
        }

        //window states
        public void OnEnter()
        {
            CompilePathText.Text = AppData.Insatnce.Options.CompilePath;
            ExportPathText.Text = AppData.Insatnce.Options.ExportPath;
            DataPathText.Text = AppData.Insatnce.Options.DataPath;
            DefaultAuthorTextBox.Text = AppData.Insatnce.Options.DefaultAuthor;
            //DefaultCompanyTextBox.Text = AppData.Insatnce.Options.DefaultCompany;
            C3AddonPathText.Text = AppData.Insatnce.Options.C3AddonPath;
            FontSizeCombo.Text = AppData.Insatnce.Options.FontSize.ToString();
            FontFamilyCombo.Text = AppData.Insatnce.Options.FontFamily;
            ThemeCombo.Text = AppData.Insatnce.Options.ThemeKey;
            IncludeTimeStamp.IsChecked = AppData.Insatnce.Options.IncludeTimeStampOnExport;
            OpenC3InWeb.IsChecked = AppData.Insatnce.Options.OpenC3InWeb;
            C3DesktopPathText.Text = AppData.Insatnce.Options.C3DesktopPath;
            PinMainMenu.IsChecked = AppData.Insatnce.Options.PinMenu;
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
                    //DefaultCompany = !string.IsNullOrWhiteSpace(DefaultCompanyTextBox.Text) ? DefaultCompanyTextBox.Text : App.DefaultOptions.DefaultCompany, 
                    DefaultAuthor = !string.IsNullOrWhiteSpace(DefaultAuthorTextBox.Text) ? DefaultAuthorTextBox.Text : App.DefaultOptions.DefaultAuthor,
                    C3AddonPath = !string.IsNullOrWhiteSpace(C3AddonPathText.Text) ? C3AddonPathText.Text : App.DefaultOptions.C3AddonPath,
                    FontSize = Convert.ToDouble(FontSizeCombo.Text),
                    FontFamily = !string.IsNullOrWhiteSpace(FontFamilyCombo.Text) ? FontFamilyCombo.Text : App.DefaultOptions.FontFamily,
                    ThemeKey = !string.IsNullOrWhiteSpace(ThemeCombo.Text) ? ThemeCombo.Text : App.DefaultOptions.ThemeKey,
                    IncludeTimeStampOnExport = IncludeTimeStamp.IsChecked != null && IncludeTimeStamp.IsChecked.Value,
                    OpenC3InWeb = OpenC3InWeb.IsChecked != null && OpenC3InWeb.IsChecked.Value,
                    C3DesktopPath = C3DesktopPathText.Text,
                    PinMenu = PinMainMenu.IsChecked != null && PinMainMenu.IsChecked.Value
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

        public void Clear()
        {
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
                ProcessHelper.Insatnce.StartProcess(CompilePathText.Text);
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                AppData.Insatnce.ErrorMessage($"error opening compiled folder, {ex.Message}");
            }
        }

        private void OpenExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessHelper.Insatnce.StartProcess(ExportPathText.Text);
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                AppData.Insatnce.ErrorMessage($"error opening export folder, {ex.Message}");
            }
        }

        private void OpenDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessHelper.Insatnce.StartProcess(DataPathText.Text);
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
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
                ProcessHelper.Insatnce.StartProcess(C3AddonPathText.Text);
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                AppData.Insatnce.ErrorMessage($"error opening c3addon folder, {ex.Message}");
            }
        }

        private void FontSizeCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var size = Convert.ToDouble(FontSizeCombo.Text);
            AppData.Insatnce.Options.FontSize = size;
        }

        private void FontFamilyCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var font = FontFamilyCombo.Text;
            AppData.Insatnce.Options.FontFamily = font;
            FontFamilyCombo.FontFamily = new FontFamily(font);
        }

        private void ThemeCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ComboBoxItem)ThemeCombo.SelectedItem).Content;
            AppData.Insatnce.Options.ThemeKey = selection.ToString();
            AppData.Insatnce.SetupTheme();
            //AppData.Insatnce.ThemeChangedEvent(ThemeResolver.Insatnce.Resolve(selection));
        }

        private void OpenChangeLog_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessHelper.Insatnce.StartProcess("chrome.exe", "https://github.com/armandoalonso/c3IDE/blob/master/CHANGELOG.md");
        }

        private void PinMainMenu_OnChecked(object sender, RoutedEventArgs e)
        {
            AppData.Insatnce.Options.PinMenu = PinMainMenu.IsChecked != null && PinMainMenu.IsChecked.Value;
            AppData.Insatnce.OptionChanged(AppData.Insatnce.Options);
            DataAccessFacade.Insatnce.OptionData.Upsert(AppData.Insatnce.Options);
        }
    }
}
