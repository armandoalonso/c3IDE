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
using c3IDE.Windows.Interfaces;
using Newtonsoft.Json;

namespace c3IDE.Windows
{
    /// <summary>
    /// Interaction logic for LanguageWindow.xaml
    /// </summary>
    public partial class LanguageWindow : UserControl, IWindow
    {
        public string DisplayName { get; set; } = "Language";

        public LanguageWindow()
        {
            InitializeComponent();
        }

        public void OnEnter()
        {
            PropertyLanguageTextEditor.Text = AppData.Insatnce.CurrentAddon?.LanguageProperties;
        }

        public void OnExit()
        {
            if (AppData.Insatnce.CurrentAddon != null)
            {
                AppData.Insatnce.CurrentAddon.LanguageProperties = PropertyLanguageTextEditor.Text;
            }
        }

        //generate property json based on edittimeplugin properties
        //todo: figure out a way to preserve modified text
        private void GeneratePropertyText(object sender, RoutedEventArgs e)
        {
            //generate new property json
            var propertyRegex = new Regex(@"new SDK[.]PluginProperty\(\""(?<type>\w+)\""\W+(?<id>.*)\""");
            var propertyMatches = propertyRegex.Matches(AppData.Insatnce.CurrentAddon.PluginEditTime);

            var propList = new List<string>();
            foreach (Match m in propertyMatches)
            {
                var type = m.Groups["type"].ToString();
                var id = m.Groups["id"].ToString();

                //create new property
                string template = string.Empty;
                switch (type)
                {
                    case "combo":
                        template = $@"    ""{id}"" : {{
        ""name"": ""property name"",
        ""desc"": ""property desc"",
        ""items"": {{
            ""item-one"": ""item one"",
            ""item-two"": ""item two""
        }}
    }}";
                        break;
                    case "link":
                        template = $@"    ""{id}"" : {{
        ""name"": ""property name"",
        ""desc"": ""property desc"",
        ""link-text"": ""link text"",
    }}";
                        break;
                    default:
                        template = $@"    ""{id}"" : {{
        ""name"": ""property name"",
        ""desc"": ""property desc""
    }}";
                        break;
                }
                propList.Add(template);
            }

            //set the editor to the new property json
            PropertyLanguageTextEditor.Text = $@"""properties"":{{
{string.Join(",\n", propList)}
}}";
        }
    }
}
