using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using c3IDE.DataAccess;
using c3IDE.Managers;
using c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using ICSharpCode.AvalonEdit;
using Newtonsoft.Json.Linq;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for LanguageWindow.xaml
    /// </summary>
    public partial class LanguageWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Language";

        /// <summary>
        /// language window constructor 
        /// </summary>
        public LanguageWindow()
        {
            InitializeComponent();

            PropertyLanguageTextEditor.Options.EnableHyperlinks = false;
            PropertyLanguageTextEditor.Options.EnableEmailHyperlinks = false;
            CategoryLanguageTextEditor.Options.EnableHyperlinks = false;
            CategoryLanguageTextEditor.Options.EnableEmailHyperlinks = false;
        }

        /// <summary>
        /// handles when the language window gets focus
        /// </summary>
        public void OnEnter()
        {
            ThemeManager.SetupTextEditor(PropertyLanguageTextEditor, Syntax.Json);
            ThemeManager.SetupTextEditor(CategoryLanguageTextEditor, Syntax.Json);

            if (AddonManager.CurrentAddon != null)
            {
                PropertyLanguageTextEditor.Text = AddonManager.CurrentAddon.LanguageProperties;
                CategoryLanguageTextEditor.Text = AddonManager.CurrentAddon.LanguageCategories;
            }         
        }

        /// <summary>
        /// handles when the language windows loses focus
        /// </summary>
        public void OnExit()
        {
            if (AddonManager.CurrentAddon != null)
            {
                AddonManager.CurrentAddon.LanguageProperties = PropertyLanguageTextEditor.Text;
                AddonManager.CurrentAddon.LanguageCategories = CategoryLanguageTextEditor.Text;
                AddonManager.SaveCurrentAddon();
            }
        }

        /// <summary>
        /// clears all input from language window
        /// </summary>
        public void Clear()
        {
            PropertyLanguageTextEditor.Text = string.Empty;
            CategoryLanguageTextEditor.Text = string.Empty;
        }

        public void ChangeTab(string tab, int lineNum)
        {

        }

        /// <summary>
        /// handles keyboard shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                //AppData.Insatnce.GlobalSave(false);
                Searcher.Insatnce.UpdateFileIndex("lang_property.js", PropertyLanguageTextEditor.Text, ApplicationWindows.LanguageWindow);
                Searcher.Insatnce.UpdateFileIndex("lang_category.js", CategoryLanguageTextEditor.Text, ApplicationWindows.LanguageWindow);
                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
            else if (e.Key == Key.F5)
            {
                WindowManager.MainWindow.Save(true, true);
            }
        }

        /// <summary>
        /// generate properties json from configured configured properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneratePropertyText(object sender, RoutedEventArgs e)
        {
            try
            {
                var propLang = TemplateHelper.GeneratePropertyLang(AddonManager.CurrentAddon.PluginEditTime,
                    PropertyLanguageTextEditor.Text);
                PropertyLanguageTextEditor.Text = propLang;
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to generate properties => {ex.Message}");
            }
           
        }

        /// <summary>
        /// generate category json from configured categories
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateCategoryText(object sender, RoutedEventArgs e)
        {
            try
            {
                var category = TemplateHelper.GenerateCategoryLang(AddonManager.CurrentAddon.Categories,
                        CategoryLanguageTextEditor.Text);
                CategoryLanguageTextEditor.Text = category;
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to generate category => {ex.Message}");
            }
        }

        private void FindGlobal_Click(object sender, RoutedEventArgs e)
        {
            //AppData.Insatnce.GlobalSave(false);
            Searcher.Insatnce.UpdateFileIndex("lang_property.js", PropertyLanguageTextEditor.Text, ApplicationWindows.LanguageWindow);
            Searcher.Insatnce.UpdateFileIndex("lang_category.js", CategoryLanguageTextEditor.Text, ApplicationWindows.LanguageWindow);

            MenuItem mnu = sender as MenuItem;
            TextEditor editor = null;

            if (mnu != null)
            {
                editor = ((ContextMenu)mnu.Parent).PlacementTarget as TextEditor;
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        private void Compile_OnClick(object sender, RoutedEventArgs e)
        {
            WindowManager.MainWindow.Save(true, true);
        }
    }
}
