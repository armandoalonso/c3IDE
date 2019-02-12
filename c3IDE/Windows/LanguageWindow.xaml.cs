using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.CodeCompletion;
using c3IDE.Utilities.Helpers;
using c3IDE.Utilities.Search;
using c3IDE.Utilities.SyntaxHighlighting;
using c3IDE.Windows.Interfaces;
using c3IDE.Utilities.ThemeEngine;
using ICSharpCode.AvalonEdit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for LanguageWindow.xaml
    /// </summary>
    public partial class LanguageWindow : UserControl, IWindow
    {
        //properties
        public string DisplayName { get; set; } = "Language";

        //ctor
        public LanguageWindow()
        {
            InitializeComponent();

            PropertyLanguageTextEditor.Options.EnableHyperlinks = false;
            PropertyLanguageTextEditor.Options.EnableEmailHyperlinks = false;

            CategoryLanguageTextEditor.Options.EnableHyperlinks = false;
            CategoryLanguageTextEditor.Options.EnableEmailHyperlinks = false;
        }

        //window states
        public void OnEnter()
        {
            AppData.Insatnce.SetupTextEditor(PropertyLanguageTextEditor, Syntax.Json);
            AppData.Insatnce.SetupTextEditor(CategoryLanguageTextEditor, Syntax.Json);

            PropertyLanguageTextEditor.Text = AppData.Insatnce.CurrentAddon?.LanguageProperties;
            CategoryLanguageTextEditor.Text = AppData.Insatnce.CurrentAddon?.LanguageCategories;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.LanguageProperties = PropertyLanguageTextEditor.Text;
                AppData.Insatnce.CurrentAddon.LanguageCategories = CategoryLanguageTextEditor.Text;
                DataAccessFacade.Insatnce.AddonData.Upsert(AppData.Insatnce.CurrentAddon);
            }
        }

        public void Clear()
        {
            PropertyLanguageTextEditor.Text = string.Empty;
            CategoryLanguageTextEditor.Text = string.Empty;
        }

        private void TextEditor_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                //AppData.Insatnce.GlobalSave(false);
                Searcher.Insatnce.UpdateFileIndex("lang_property.js", PropertyLanguageTextEditor.Text, AppData.Insatnce.MainWindow._languageWindow);
                Searcher.Insatnce.UpdateFileIndex("lang_category.js", CategoryLanguageTextEditor.Text, AppData.Insatnce.MainWindow._languageWindow);
                var editor = ((TextEditor)sender);
                var text = editor.SelectedText;
                Searcher.Insatnce.GlobalFind(text, this);
            }
        }

        //context menu
        private void GeneratePropertyText(object sender, RoutedEventArgs e)
        {
            //generate new property json
            var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+\""(?<id>(\w+|-)+)\""");
            var propertyMatches = propertyRegex.Matches(AppData.Insatnce.CurrentAddon.PluginEditTime);

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
            PropertyLanguageTextEditor.Text = FormatHelper.Insatnce.Json(TemplateHelper.LanguageProperty(string.Join(",\n", propList)), true);
        }

        private void GenerateCategoryText(object sender, RoutedEventArgs e)
        {
            var catList = new List<string>();
            var dynamicCats = JToken.Parse($"{{{CategoryLanguageTextEditor.Text}}}")["aceCategories"];

            foreach (var category in AppData.Insatnce.CurrentAddon.Categories)
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

        //heleprs
        private void IdentifyAllProperties()
        {

        }
    }
}
