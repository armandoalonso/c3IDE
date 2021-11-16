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
using System.Windows.Shapes;
using c3IDE.Managers;
using c3IDE.Utilities.Helpers;
using MahApps.Metro.Controls;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for AddonIconWindow.xaml
    /// </summary>
    public partial class AddonIconWindow : MetroWindow
    {
        public string IconXml { get; set; }

        public AddonIconWindow()
        {
            InitializeComponent();
        }

        public void Init(string iconXml)
        {
            try
            {
                IconXml = iconXml;
                AddonIcon.Source = ImageHelper.Insatnce.XmlToBitmapImage(IconXml);
            }
            catch(Exception ex)
            {
                LogManager.AddLogMessage($"IconXml: {iconXml}");
                throw ex;
            }
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
                    AddonIcon.Source = ImageHelper.Insatnce.XmlToBitmapImage(IconXml);
                }
            }
            catch (Exception exception)
            {
                LogManager.AddErrorLog(exception);
                NotificationManager.PublishErrorNotification($"error reading icon file, {exception.Message}");
            }
        }

        private void SaveIcon_Click(object sender, RoutedEventArgs e)
        {
            AddonManager.CurrentAddon.IconXml = IconXml;
            AddonManager.SaveCurrentAddon();
            AddonManager.LoadAllAddons();
            ApplicationWindows.DashboardWindow.OnExit();
            ApplicationWindows.DashboardWindow.OnEnter();
            this.Close();
        }
    }
}
