using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Windows.Interfaces;
using Action = c3IDE.Models.Action;
using Condition = c3IDE.Models.Condition;
using Expression = c3IDE.Models.Expression;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Dashboard";
        private string IconXml { get; set; }
        public DashboardWindow()
        {
            InitializeComponent();
            IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
            AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml)); //ImageHelper.Insatnce.Base64ToBitmap(defaultIcon);
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
                Description = DescriptionText.Text,
                Type = pluginType,
                IconXml = IconXml,
                //IconBase64 = ImageHelper.Insatnce.BitmapImageToBase64(AddonIcon.Source as BitmapImage),
                Template = TemplateFactory.Insatnce.CreateTemplate(pluginType),
                CreateDate = DateTime.Now,
                LastModified = DateTime.Now,
            };

            //validate all fields are entered
            if (string.IsNullOrWhiteSpace(addon.Class) ||
                string.IsNullOrWhiteSpace(addon.Company) ||
                string.IsNullOrWhiteSpace(addon.Name) ||
                string.IsNullOrWhiteSpace(addon.Author) ||
                string.IsNullOrWhiteSpace(addon.Version))
            {
                AppData.Insatnce.ErrorMessage("addon data fields cannot be blank");
                return;
            }

            //apply the templates
            addon.AddonJson = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.AddonJson, addon);
            addon.PluginEditTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.PluginEditTime, addon);
            addon.PluginRunTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.PluginRunTime, addon);
            addon.TypeEditTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.TypeEditTime, addon);
            addon.TypeRunTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.TypeRunTime, addon);
            addon.InstanceEditTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.InstanceEditTime, addon);
            addon.InstanceRunTime = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.InstanceRunTime, addon);
            addon.Actions = new Dictionary<string, Action>();
            addon.Conditions = new Dictionary<string, Condition>();
            addon.Expressions = new Dictionary<string, Expression>();
            addon.LanguageProperties = addon.Template.LanguageProperty;

            AppData.Insatnce.CurrentAddon = addon;
            DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;

            AppData.Insatnce.InfoMessage($"{addon.Name} created successfully...");
            ClearInputControls();
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
                    IconXml = File.ReadAllText(file);
                    AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml));
                }
            }
            catch(Exception exception) 
            {
                Console.WriteLine(exception.Message);
                AppData.Insatnce.ErrorMessage($"error reading icon file, {exception.Message}");
            }
        }

        private void LoadAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error loading c3addon, no c3addon selected");
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
            AppData.Insatnce.InfoMessage($"{currentAddon.Name} loaded successfully");
        }

        private void RemoveAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error removing c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;

            AppData.Insatnce.CurrentAddon = null;
            DataAccessFacade.Insatnce.AddonData.Delete(currentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;

            AppData.Insatnce.InfoMessage($"addon removed successfully");
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
            AppData.Insatnce.InfoMessage($"{currentAddon.Name} loaded successfully");
        }

        private void ExportAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            //todo: implement export/import
        }

        private void ClearInputControls()
        {
            AddonNameText.Text = string.Empty;
            AddonClassText.Text = string.Empty;
            CompanyNameText.Text = string.Empty;
            AuthorText.Text = string.Empty;
            VersionText.Text = string.Empty;
            AddonTypeDropdown.SelectedIndex = -1;
            IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
            AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml));
        }
    }
}
