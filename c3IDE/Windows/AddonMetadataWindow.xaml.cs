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
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using c3IDE.Windows.Interfaces;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for AddonMetadataWindow.xaml
    /// </summary>
    public partial class AddonMetadataWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Addon Metadata";
        private string IconXml { get; set; }
        public bool IsEdit { get; set; } = false;
        private bool IsSaved = false;

        /// <summary>
        /// addon metadata window constructor
        /// </summary>
        public AddonMetadataWindow()
        {
            InitializeComponent();
            IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
            AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml)); //ImageHelper.Insatnce.Base64ToBitmap(defaultIcon);
            AddonTypeDropdown.ItemsSource = Enum.GetValues(typeof(PluginType));
            AddonTypeDropdown.SelectedIndex = -1;
        }

        /// <summary>
        /// called when this widnows becomes the main widnow
        /// </summary>
        public void OnEnter()
        {
            IsSaved = false;
            if (IsEdit && AddonManager.CurrentAddon != null)
            {
                AddonNameText.Text = AddonManager.CurrentAddon.Name;
                AddonClassText.Text = AddonManager.CurrentAddon.Class;
                AuthorText.Text = AddonManager.CurrentAddon.Author;
                AddonTypeDropdown.Text = AddonManager.CurrentAddon.Type.ToString();
                DescriptionText.Text = AddonManager.CurrentAddon.Description;
                AddonIcon.Source = AddonManager.CurrentAddon.IconImage;
            }
            else
            {
                AddonNameText.Text = "New Plugin";
                AddonClassText.Text = "NewPlugin";
                AuthorText.Text = OptionsManager.CurrentOptions.DefaultAuthor;
                AddonTypeDropdown.Text = "SingleGlobalPlugin";
                DescriptionText.Text = string.Empty;
                AddonCategoryDropdown.Text = "general";
            }
        }

        /// <summary>
        /// called when this windows stops being the main window
        /// </summary>
        public void OnExit()
        {
            if (IsSaved && AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.Name = AddonNameText.Text;
                AddonManager.CurrentAddon.Class = AddonClassText.Text;
                AddonManager.CurrentAddon.Company= AuthorText.Text;
                AddonManager.CurrentAddon.Author = AuthorText.Text;
                AddonManager.CurrentAddon.AddonCategory = AddonTypeDropdown.Text;
            }
        }

        /// <summary>
        /// this clears all the inputs in the widnow
        /// </summary>
        public void Clear()
        {
           
        }

        /// <summary>
        /// hover effect to copy when icon is dragged over icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Addon_OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// change the icon on drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddonIcon_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var file = ((string[])e.Data.GetData(DataFormats.FileDrop))?.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(file))
                {
                    IconXml = File.ReadAllText(file);
                    AddonIcon.Source = ImageHelper.Insatnce.SvgToBitmapImage(ImageHelper.Insatnce.SvgFromXml(IconXml));
                }
            }
            catch (Exception exception)
            {   
                LogManager.AddErrorLog(exception);
                NotificationManager.PublishErrorNotification($"error reading icon file, {exception.Message}");
            }
        }

        /// <summary>
        /// change textbox behavior to select all text on focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var tb = (sender as TextBox);
            tb?.SelectAll();
        }

        /// <summary>
        /// focus on the textbox when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }

        /// <summary>
        /// this saves the currently selected addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveAddon_Click(object sender, RoutedEventArgs e)
        {
            AddonManager.CurrentAddon = new C3Addon
            {
                Name = string.Empty,
                Class = string.Empty,
                Company = string.Empty,
                Author = string.Empty,
                Description = string.Empty,
                AddonCategory = string.Empty,
                Type = PluginType.SingleGlobalPlugin,
                Effect = new Effect(),
                IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg"),
                CreateDate = DateTime.Now
            };


            Enum.TryParse<PluginType>(AddonTypeDropdown.SelectedValue.ToString(), out var pluginType);
            var addonCategory = string.IsNullOrWhiteSpace(AddonCategoryDropdown.Text)? "other": AddonCategoryDropdown.Text;

            AddonManager.CurrentAddon.Name = AddonNameText.Text;
            AddonManager.CurrentAddon.Class = AddonClassText.Text.Replace(" ", string.Empty).Trim();
            AddonManager.CurrentAddon.Company = AuthorText.Text.Replace(" ", string.Empty).Trim();
            AddonManager.CurrentAddon.Author = AuthorText.Text;
            AddonManager.CurrentAddon.Description = DescriptionText.Text;
            AddonManager.CurrentAddon.Type = pluginType;
            AddonManager.CurrentAddon.AddonCategory = addonCategory;
            AddonManager.CurrentAddon.IconXml = IconXml;
            AddonManager.CurrentAddon.Template = TemplateFactory.Insatnce.CreateTemplate(pluginType);
            AddonManager.CurrentAddon.LastModified = DateTime.Now;

            //add version
            AddonManager.CurrentAddon.MajorVersion = 1;
            AddonManager.CurrentAddon.MinorVersion = 0;
            AddonManager.CurrentAddon.RevisionVersion = 0;
            AddonManager.CurrentAddon.BuildVersion = 0;

            //validate addon
            if (!AddonManager.ValidateCurrentAddon())
            {
                NotificationManager.PublishErrorNotification("addon data fields cannot be blank");
                return;
            }

            AddonMetadataGrid.IsEnabled = false;
            await Task.Run(() =>
            {
                IsSaved = true;
                AddonManager.CompileTemplates();
                AddonManager.SaveCurrentAddon();
                AddonManager.LoadAllAddons();
            });

            AddonMetadataGrid.IsEnabled = true;
            AddonManager.LoadAddon(AddonManager.CurrentAddon);
            NotificationManager.PublishNotification($"{AddonManager.CurrentAddon.Name} has been saved successfully");
            WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
        }

        /// <summary>
        /// when the plugin type changes, change the available categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddonTypeDropdown_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.AddedItems[0].ToString();
            AddonCategoryDropdown.SelectedIndex = -1;
            switch (selected)
            {
                case "Behavior":
                    AddonCategoryDropdown.Items.Clear();
                    foreach (var s in new[] { "general", "attributes", "movements", "other" })
                    {
                        AddonCategoryDropdown.Items.Add(s);
                    }

                    break;
                case "Effect":
                    AddonCategoryDropdown.Items.Clear();
                    foreach (var s in new[] { "blend", "color", "distortion", "normal-mapping", "other" })
                    {
                        AddonCategoryDropdown.Items.Add(s);
                    }

                    break;
                default:
                    AddonCategoryDropdown.Items.Clear();
                    foreach (var s in new[] { "general", "data-and-storage", "form-controls", "input", "media", "monetisation", "platform-specific", "web", "other" })
                    {
                        AddonCategoryDropdown.Items.Add(s);
                    }

                    break;
            }
            AddonCategoryDropdown.SelectedIndex = 0;
        }
    }
}


