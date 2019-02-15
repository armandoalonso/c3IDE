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
            if (AddonManager.CurrentAddon != null)
            {
                AddonNameText.Text = AddonManager.CurrentAddon.Name;
                AddonClassText.Text = AddonManager.CurrentAddon.Class;
                AuthorText.Text = AddonManager.CurrentAddon.Author;
                VersionText.Text = AddonManager.CurrentAddon.Version;
                AddonTypeDropdown.Text = AddonManager.CurrentAddon.Type.ToString();
                DescriptionText.Text = AddonManager.CurrentAddon.Description;
                AddonIcon.Source = AddonManager.CurrentAddon.IconImage;
            }
        }

        /// <summary>
        /// called when this widnows stops being the main window
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.Name = AddonNameText.Text;
                AddonManager.CurrentAddon.Class = AddonClassText.Text;
                AddonManager.CurrentAddon.Company= AuthorText.Text;
                AddonManager.CurrentAddon.Author = AuthorText.Text;
                AddonManager.CurrentAddon.Version = VersionText.Text;
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
        /// focus on teh textbox when clicked
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
        private void SaveAddon_Click(object sender, RoutedEventArgs e)
        {
            if (AddonManager.CurrentAddon != null)
            {
                Enum.TryParse<PluginType>(AddonTypeDropdown.SelectedValue.ToString(), out var pluginType);
                AddonManager.CurrentAddon.Name = AddonNameText.Text;
                AddonManager.CurrentAddon.Class = AddonClassText.Text.Replace(" ", string.Empty).Trim();
                AddonManager.CurrentAddon.Company = AuthorText.Text.Replace(" ", string.Empty).Trim();
                AddonManager.CurrentAddon.Author = AuthorText.Text;
                AddonManager.CurrentAddon.Version = VersionText.Text;
                AddonManager.CurrentAddon.Type = pluginType;
                AddonManager.CurrentAddon.IconXml = ImageHelper.Insatnce.BitmapImageToBase64(AddonIcon.Source as BitmapImage);
                AddonManager.CurrentAddon.Template = TemplateFactory.Insatnce.CreateTemplate(pluginType);
                AddonManager.CurrentAddon.LastModified = DateTime.Now;

                //validate addon
                if (!AddonManager.ValidateCurrentAddon())
                {
                    NotificationManager.PublishErrorNotification("addon data fields cannot be blank");
                    return;
                }

                AddonManager.CompileTemplates();
                AddonManager.SaveCurrentAddon();
                AddonManager.LoadAllAddons();
                NotificationManager.PublishNotification($"{AddonManager.CurrentAddon.Name} has been saved successfully");
                WindowManager.ChangeWindow(ApplicationWindows.DashboardWindow);
            }
        }
    }
}
