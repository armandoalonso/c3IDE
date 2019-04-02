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
                //generate new property json
                var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+\""(?<id>(\w+|-)+)\""");
                var propertyMatches = propertyRegex.Matches(AddonManager.CurrentAddon.PluginEditTime);

                //get current dynamic properties
                var dynamicProps = JToken.Parse($"{{{PropertyLanguageTextEditor.Text}}}")["properties"];

                var propList = new List<string>();
                foreach (Match m in propertyMatches)
                {
                    var type = m.Groups["type"].ToString();
                    var id = m.Groups["id"].ToString();

                    string template;
                    if (dynamicProps?[id] != null)
                    {
                        //prop already exists
                        var value = dynamicProps[id].ToString();
                        template = $"\"{id}\": {value}";
                    }
                    else
                    {
                        //this prop is new
                        switch (type)
                        {
                            case "combo":
                                template = TemplateHelper.LanguagePropertyCombo(id);
                                break;
                            case "link":
                                template = TemplateHelper.LanguagePropertyLink(id);
                                break;
                            default:
                                template = TemplateHelper.LanguagePropertyDefault(id);
                                break;
                        }
                    }

                    //create new property
                    propList.Add(template);
                }

                //set the editor to the new property json
                PropertyLanguageTextEditor.Text =
                    FormatHelper.Insatnce.Json(TemplateHelper.LanguageProperty(string.Join(",\n", propList)), true);
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
                var catList = new List<string>();
                var dynamicCats = JToken.Parse($"{{{CategoryLanguageTextEditor.Text}}}")["aceCategories"];

                foreach (var category in AddonManager.CurrentAddon.Categories)
                {
                    if (dynamicCats?[category] != null)
                    {
                        var value = dynamicCats[category];
                        catList.Add($"    \"{category}\" : \"{value}\"");
                    }

                    else
                    {
                        catList.Add($"    \"{category}\" : \"value\"");
                    }
                }

                CategoryLanguageTextEditor.Text = $@"""aceCategories"": {{
{string.Join(",\n", catList)}
}}";
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                NotificationManager.PublishErrorNotification($"failed to generate category => {ex.Message}");
            }
        }
    }
}
