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
using c3IDE.Windows.Interfaces;
using Newtonsoft.Json;

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
        }

        //window states
        public void OnEnter()
        {
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

        //context menu
        private void GeneratePropertyText(object sender, RoutedEventArgs e)
        {
            //generate new property json
            var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+\""(?<id>(\w+|-)+)\""");
            var propertyMatches = propertyRegex.Matches(AppData.Insatnce.CurrentAddon.PluginEditTime);

            var propList = new List<string>();
            foreach (Match m in propertyMatches)
            {
                var type = m.Groups["type"].ToString();
                var id = m.Groups["id"].ToString();

                //create new property
                string template;
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
                propList.Add(template);
            }

            //set the editor to the new property json
            PropertyLanguageTextEditor.Text = TemplateHelper.LanguageProperty(string.Join(",\n", propList));
        }

        private void GenerateCategoryText(object sender, RoutedEventArgs e)
        {
            var catList = new List<string>();
            foreach(var category in AppData.Insatnce.CurrentAddon.Categories)
            {
                catList.Add($"    \"{category}\" : \"value\"");
            }

            CategoryLanguageTextEditor.Text = $@"""aceCategories"": {{
{string.Join(",\n", catList)}
}}";
        }
    }
}
