using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using c3IDE.Compiler;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using c3IDE.Windows.Interfaces;
using Newtonsoft.Json;


namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Dashboard";
        private CollectionView View;

        /// <summary>
        /// window constructor, loads all addons from storage, setups up default values 
        /// </summary>
        public DashboardWindow()
        {
            InitializeComponent();
        
            //load addon if it was passed through cmd args
            if (AddonManager.CurrentAddon != null && AddonManager.CurrentAddon.Id != Guid.Empty)
            {
                for (int i = 0; i < AddonManager.AllAddons.Count; i++)
                {
                    if (AddonManager.AllAddons[i].Equals(AddonManager.CurrentAddon))
                    {
                        AddonListBox.SelectedIndex = i;
                    }
                }
            }
            else
            {
                AddonManager.CurrentAddon = null;
            }
        }

        /// <summary>
        /// executes when the window is becomes the main window, sets the list of addons to the addonlist in memeory
        /// </summary>
        public void OnEnter()
        {
            //AddonManager.LoadAllAddons(); //removed for dashboard load performace ?? do we really need this call?
            AddonFilter.Text = string.Empty;
            AddonListBox.ItemsSource = AddonManager.AllAddons;
            View = CollectionViewSource.GetDefaultView(AddonListBox.ItemsSource) as CollectionView;
            if (View != null) View.Filter = SearchFilter;
        }

        private bool SearchFilter(object obj)
        {
            if (string.IsNullOrEmpty(AddonFilter.Text))
            {
                return true;
            }

            return obj.ToString().IndexOf(AddonFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// executes when this window is no longer the main window
        /// </summary>
        public void OnExit()
        { 
        }

        /// <summary>
        /// clear shoudl clear all the windows data
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// display copy cursor when hovering a file over the listbox
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

        //todo: we want to use the same guid, to overwrite existing addon.
        //todo: we want to parse a different version of the .c3ide file with relative paths 
        //todo: abstract this functionaility out to a helper class

        /// <summary>
        /// when a file is dropped into the list box, if the file is a .c3ide file parse the file and load the addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddonFile_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var file = ((string[])e.Data.GetData(DataFormats.FileDrop))?.FirstOrDefault();
                var info = new FileInfo(file);
                C3Addon c3addon;

                if (info.Extension.Contains("c3ide"))
                {
                    var addonInfo = File.ReadAllLines(info.FullName)[0];
                    if (addonInfo == "@@METADATA")
                    {
                        c3addon = ProjectManager.ReadProject(info.FullName);
                    }
                    else
                    {
                        var data = File.ReadAllText(info.FullName);
                        c3addon = JsonConvert.DeserializeObject<C3Addon>(data);
                    }

                    //when you import the project, it should not overwrite any other project
                    if (OptionsManager.CurrentOptions.OverwriteGuidOnImport)
                    {
                        c3addon.Id = Guid.NewGuid();
                    }

                    //get the plugin template
                    c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);

                    var addonIndex = AddonManager.AllAddons.Count - 1;
                    AddonManager.CurrentAddon = c3addon;
                    AddonManager.SaveCurrentAddon();
                    AddonManager.LoadAllAddons();
                    AddonListBox.ItemsSource = AddonManager.AllAddons;
                    AddonManager.LoadAddon(c3addon);
                }
                else
                {
                    throw new InvalidOperationException("invalid file type, for import");
                }
            }
            catch (Exception exception)
            {
                LogManager.AddErrorLog(exception);
                NotificationManager.PublishErrorNotification($"error importing c3ide file, {exception.Message}");
            }
        }

        //todo: abstract the load addon functionaity to a helper class
        /// <summary>
        /// handles when the user double clicks an addon from the list, we want to make that addon the currently loaded addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddonListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("error loading c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.LoadAddon(currentAddon);
        }

        /// <summary>
        /// naviagte to addon metadata window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewAddon_Click(object sender, RoutedEventArgs e)
        {
            //skip loaded addon callback
            AddonManager.CurrentAddon = null;
            WindowManager.ChangeWindow(ApplicationWindows.MetadataWindow);
        }

        /// <summary>
        /// loads the selected addon as the current addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadSelectedAddon_Click(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("error loading c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.LoadAddon(currentAddon);
        }

        /// <summary>
        /// removes the selected addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSelectedAddon_Click(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("error removing c3addon, no c3addon selected");
                return;
            }
            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.DeleteAddon(currentAddon);
            AddonListBox.ItemsSource = AddonManager.AllAddons;
            NotificationManager.PublishNotification($"addon removed successfully");
        }

        /// <summary>
        /// creates a copy of the selected addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DuplicateSelectedAddon_Click(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
               NotificationManager.PublishErrorNotification("error duplicating c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.DuplicateAddon(currentAddon);
            AddonListBox.ItemsSource = AddonManager.AllAddons;
            NotificationManager.PublishNotification($"addon duplicated successfully");
        }

        /// <summary>
        /// exports the cuurently selected addon to c3ide project file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportAddonProject_Click(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("error exporting c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.LoadAddon(currentAddon);
            ProcessHelper.Insatnce.StartProcess(AddonManager.ExportAddonProject());
        }

        /// <summary>
        /// builds the c3addon zip file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildSelectedAddon_Click(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("error building c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.LoadAddon(currentAddon);
            AddonExporter.Insatnce.ExportAddon(AddonManager.CurrentAddon);
            ProcessHelper.Insatnce.StartProcess(OptionsManager.CurrentOptions.C3AddonPath);

            AddonManager.CurrentAddon.BuildVersion++;
            AddonManager.SaveCurrentAddon();

            AddonManager.LoadAllAddons();
            AddonListBox.ItemsSource = AddonManager.AllAddons;
        }

        /// <summary>
        /// loads compiles and test the selected addon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CompileAndTest_Click(object sender, RoutedEventArgs e)
        {
            if (AddonListBox.SelectedIndex == -1)
            {
                NotificationManager.PublishErrorNotification("error compiling c3addon, no c3addon selected");
                return;
            }

            var currentAddon = (C3Addon)AddonListBox.SelectedItem;
            AddonManager.LoadAddon(currentAddon);

            WindowManager.ChangeWindow(ApplicationWindows.TestWidnow);
            await ApplicationWindows.TestWidnow.Test();
        }

        /// <summary>
        /// filters dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddonFilter_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(AddonListBox.ItemsSource).Refresh();
        }
    }
}
