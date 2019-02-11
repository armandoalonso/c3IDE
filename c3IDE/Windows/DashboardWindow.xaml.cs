using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using c3IDE.Compiler;
using c3IDE.DataAccess;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Logging;
using c3IDE.Utilities.Search;
using c3IDE.Windows.Interfaces;
using c3IDE.Utilities.ThemeEngine;
using Newtonsoft.Json;
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
        //properties
        public string DisplayName { get; set; } = "Dashboard";
        private string IconXml { get; set; }
        private string EffectXml { get; set; }
        public System.Action AddonLoaded { get; set; }

        //ctor
        public DashboardWindow()
        {
            InitializeComponent();
            IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
            EffectXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.fx.svg");
            AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml)); //ImageHelper.Insatnce.Base64ToBitmap(defaultIcon);
            AddonTypeDropdown.ItemsSource = Enum.GetValues(typeof(PluginType));
            AddonTypeDropdown.SelectedIndex = -1;
            ResetInputs();

            //load addon if it was passed through cmd args
            if (AppData.Insatnce.CurrentAddon.Id != Guid.Empty)
            {
                LoadAddon(AppData.Insatnce.CurrentAddon);
                for (int i = 0; i < AppData.Insatnce.AddonList.Count; i++)
                {
                    if (AppData.Insatnce.AddonList[i].Equals(AppData.Insatnce.CurrentAddon))
                    {
                        AddonListBox.SelectedIndex = i;
                    }
                }
            }
            else
            {
                AppData.Insatnce.CurrentAddon = null;
            }
        }

        //window states
        public void OnEnter()
        {
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
        }

        public void OnExit()
        { 
        }

        public void Clear()
        {
        }


        //file drop
        private void Addon_OnDragEnter(object sender, DragEventArgs e)
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
                LogManager.Insatnce.Exceptions.Add(exception);
                Console.WriteLine(exception.Message);
                AppData.Insatnce.ErrorMessage($"error reading icon file, {exception.Message}");
            }
        }

        private void AddonFile_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var file = ((string[])e.Data.GetData(DataFormats.FileDrop))?.FirstOrDefault();
                var info = new FileInfo(file);

                if (info.Extension.Contains("c3ide"))
                {
                    var data = File.ReadAllText(info.FullName);
                    var c3addon = JsonConvert.DeserializeObject<C3Addon>(data);
                    //when you import the project, it should not overwrite any other project 
                    c3addon.Id = Guid.NewGuid();
                    //get the plugin template
                    c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);

                    var addonIndex = AppData.Insatnce.AddonList.Count - 1;
                    AppData.Insatnce.CurrentAddon = c3addon;
                    DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
                    AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
                    AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
                    AddonListBox.SelectedIndex = addonIndex + 1;
                }
                else
                {
                    throw new InvalidOperationException("invalid file type, for import");
                }
            }
            catch (Exception exception)
            {
                LogManager.Insatnce.Exceptions.Add(exception);
                Console.WriteLine(exception.Message);
                AppData.Insatnce.ErrorMessage($"error importing c3ide file, {exception.Message}");
            }
        }

        //button double click
        private void AddonListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error loading c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            LoadAddon(currentAddon);
        }

        //button click
        private void CreateAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            Enum.TryParse<PluginType>(AddonTypeDropdown.SelectedValue.ToString(), out var pluginType);
            var addon = new C3Addon
            {
                Name = AddonNameText.Text,
                Class = AddonClassText.Text.Replace(" ", string.Empty),
                Company = AuthorText.Text,
                Author = AuthorText.Text.Replace(" ", string.Empty),
                Version = VersionText.Text,
                Description = DescriptionText.Text,
                Type = pluginType,
                IconXml = IconXml,
                //IconBase64 = ImageHelper.Insatnce.BitmapImageToBase64(AddonIcon.Source as BitmapImage),
                Template = TemplateFactory.Insatnce.CreateTemplate(pluginType),
                CreateDate = DateTime.Now,
                LastModified = DateTime.Now
            };

            //validate all fields are entered
            if (string.IsNullOrWhiteSpace(addon.Class) ||
                string.IsNullOrWhiteSpace(addon.Company) ||
                string.IsNullOrWhiteSpace(addon.Name) ||
                string.IsNullOrWhiteSpace(addon.Author) ||
                string.IsNullOrWhiteSpace(addon.Version) ||
                string.IsNullOrWhiteSpace(addon.Description))
            {
                AppData.Insatnce.ErrorMessage("addon data fields cannot be blank");
                return;
            }

            if (addon.Type == PluginType.Effect)
            {
                addon.AddonJson = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.AddonJson, addon);
                addon.EffectCode = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.EffectCode, addon);
                addon.EffectLanguage = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.EffectLangauge, addon);
            }
            else
            {
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
                addon.ThirdPartyFiles = new Dictionary<string, ThirdPartyFile>();
                addon.LanguageProperties = addon.Template.LanguageProperty;
            }

            var addonIndex = AppData.Insatnce.AddonList.Count - 1;
            AppData.Insatnce.CurrentAddon = addon;
            DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
            AddonListBox.SelectedIndex = addonIndex + 1;

            AppData.Insatnce.InfoMessage($"{addon.Name} created successfully...");
            ResetInputs();
        }

        private void LoadAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error loading c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            LoadAddon(currentAddon);
        }

        private void RemoveAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error removing c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            RemoveAddon(currentAddon);
        }

        private void ExportAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error exporting c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            ExportAddon(currentAddon);
        }

        private void ExportFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.ExportPath);
        }

        private void ClearInputsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ResetInputs();
        }

        private void CreateC3AddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error creating c3addon file, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonExporter.Insatnce.ExportAddon(currentAddon);
            ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.C3AddonPath);
        }

        //context menu
        private void LoadAddonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error loading c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            LoadAddon(currentAddon);
        }

        private void ExportProjectMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error exporting c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            ExportAddon(currentAddon);
        }

        private void RemoveAddonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error removing c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            RemoveAddon(currentAddon);
        }

        // logic
        private void LoadAddon(C3Addon currentAddon)
        {
            AddonNameText.Text = currentAddon.Name;
            AddonClassText.Text = currentAddon.Class;
            //CompanyNameText.Text = currentAddon.Company;
            AuthorText.Text = currentAddon.Author;
            VersionText.Text = currentAddon.Version;
            AddonTypeDropdown.Text = currentAddon.Type.ToString();
            DescriptionText.Text = currentAddon.Description;
            AddonIcon.Source = currentAddon.IconImage;

            AppData.Insatnce.CurrentAddon = currentAddon;
            AppData.Insatnce.InfoMessage($"{currentAddon.Name} loaded successfully");
            AppData.Insatnce.LoadAddon(AppData.Insatnce.CurrentAddon.Name);

            AddonLoaded?.Invoke();
        }

        private void ExportAddon(C3Addon addon)
        {
            var addonJson = JsonConvert.SerializeObject(addon);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var name = AppData.Insatnce.Options.IncludeTimeStampOnExport
                ? $"{addon.Class}_{timestamp}.c3ide"
                : $"{addon.Class}.c3ide";

            ProcessHelper.Insatnce.WriteFile(Path.Combine(AppData.Insatnce.Options.ExportPath, name), addonJson);
            ProcessHelper.Insatnce.StartProcess(AppData.Insatnce.Options.ExportPath);
        }

        private void RemoveAddon(C3Addon currentAddon)
        {
            AppData.Insatnce.CurrentAddon = null;
            DataAccessFacade.Insatnce.AddonData.Delete(currentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;

            AppData.Insatnce.InfoMessage($"addon removed successfully");
        }

        private void ResetInputs()
        {
            AddonNameText.Text = string.Empty;
            AddonClassText.Text = string.Empty;
            //CompanyNameText.Text = AppData.Insatnce.Options.DefaultCompany;
            AuthorText.Text = AppData.Insatnce.Options.DefaultAuthor;
            VersionText.Text = "1.0.0.0";
            AddonTypeDropdown.SelectedIndex = 0;
            DescriptionText.Text = string.Empty;
            IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
            AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml));
        }

        //text box events
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

        private void UpdateAddonButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1 && AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.ErrorMessage("error removing c3addon, no c3addon selected");
                return;
            }
            var currentAddon = AppData.Insatnce.CurrentAddon;

            if (currentAddon == null)
            {
                AppData.Insatnce.ErrorMessage("update addon failed, no c3 addon selected");
                return;
            }

            var addonclass = AddonClassText.Text;
            var name = AddonNameText.Text;
            var author = AuthorText.Text.Replace(" ", string.Empty);
            var version = VersionText.Text;
            var description = DescriptionText.Text;

            //validate all fields are entered
            if (string.IsNullOrWhiteSpace(addonclass) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(author) ||
                string.IsNullOrWhiteSpace(version) ||
                string.IsNullOrWhiteSpace(description))
            {
                AppData.Insatnce.ErrorMessage("addon data fields cannot be blank");
                return;
            }

            FindAndReplaceHelper.Insatnce.ReplaceMetadata(addonclass, name, author, version, description, currentAddon);

            DataAccessFacade.Insatnce.AddonData.Upsert(currentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
        }

        private void DuplicateProjectMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error duplicating c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            currentAddon.Id = Guid.NewGuid();

            DataAccessFacade.Insatnce.AddonData.Upsert(currentAddon);
            AppData.Insatnce.AddonList = DataAccessFacade.Insatnce.AddonData.GetAll().ToList();
            AddonListBox.ItemsSource = AppData.Insatnce.AddonList;
            AppData.Insatnce.InfoMessage($"{currentAddon.Name} duplicated successfully...");
            ResetInputs();
        }

        private void OtherInstanceOpenMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                AppData.Insatnce.ErrorMessage("error duplicating c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            var currentAppPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            ProcessHelper.Insatnce.StartProcess(currentAppPath, currentAddon.Id.ToString());
        }
    }
}
