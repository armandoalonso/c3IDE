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
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : UserControl
    {
        public string DisplayName { get; set; } = "Dashboard";
        public DashboardWindow()
        {
            InitializeComponent();
            var defaultIcon = ResourceReader.Insatnce.GetResourceAsBase64("c3IDE.Templates.Files.icon.png");
            AddonIcon.Source = ImageHelper.Insatnce.Base64ToBitmap(defaultIcon);
            AddonTypeDropdown.ItemsSource = Enum.GetValues(typeof(PluginType));
            AddonTypeDropdown.SelectedIndex = 0;
        }

        public void OnEnter()
        {
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
        }

        public void OnExit()
        {
            
        }

        private void CreateAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            Enum.TryParse<PluginType>(AddonTypeDropdown.SelectedValue.ToString(), out var pluginType);
            var addon = new C3Addon
            {
                Name = AddonNameText.Text,
                Class = AddonClassText.Text,
                Company = CompanyNameText.Text,
                Author = AuthorText.Text,
                Version = VersionText.Text,
                Type = pluginType,
                IconBase64 = ImageHelper.Insatnce.BitmapImageToBase64(AddonIcon.Source as BitmapImage),
                Template = TemplateFactory.Insatnce.CreateTemplate(pluginType),
                CreateDate = DateTime.Now,
                LastModified = DateTime.Now,
            };

            //apply the templates
            addon.AddonJson = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.AddonJson, addon);

            AppData.Insatnce.CurrentAddon = addon;
            DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
        }

        private void AddonIcon_OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void AddonIcon_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var file = ((string[]) e.Data.GetData(DataFormats.FileDrop))?.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(file))
                {
                    var img = ImageHelper.Insatnce.ImageToBase64(file);
                    AddonIcon.Source = ImageHelper.Insatnce.Base64ToBitmap(img);
                }
            }
            catch(Exception exception) 
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void LoadAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                Console.WriteLine(@"No C3 addon selected to load");
                return;
            } 

            var currentAddon = (C3Addon) AddonListBox.SelectedItem;
            currentAddon.LastModified = DateTime.Now;

            AddonNameText.Text = currentAddon.Name;
            AddonClassText.Text = currentAddon.Class;
            CompanyNameText.Text = currentAddon.Company;
            AuthorText.Text = currentAddon.Author;
            VersionText.Text = currentAddon.Version;
            AddonTypeDropdown.Text = currentAddon.Type.ToString();
            AddonIcon.Source = currentAddon.IconImage;

            AppData.Insatnce.CurrentAddon = currentAddon;
        }

        private void RemoveAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                Console.WriteLine(@"No C3 addon selected to load");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;

            AppData.Insatnce.CurrentAddon = null;
            DataAccessFacade.Insatnce.AddonData.Delete(currentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
        }

        private void AddonListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                Console.WriteLine(@"No C3 addon selected to load");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            currentAddon.LastModified = DateTime.Now;

            AddonNameText.Text = currentAddon.Name;
            AddonClassText.Text = currentAddon.Class;
            CompanyNameText.Text = currentAddon.Company;
            AuthorText.Text = currentAddon.Author;
            VersionText.Text = currentAddon.Version;
            AddonTypeDropdown.Text = currentAddon.Type.ToString();
            AddonIcon.Source = currentAddon.IconImage;

            AppData.Insatnce.CurrentAddon = currentAddon;
        }
    }
}
