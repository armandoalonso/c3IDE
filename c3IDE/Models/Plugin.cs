using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using c3IDE.Templates;

namespace c3IDE.Models
{
    public class Plugin
    {
        public string Name { get; set; }
        public string ClassName => Name.Replace(" ", string.Empty);
        public string Company { get; set; }
        public string Author { get; set; } = "c3IDE";
        public string Version { get; set; } = "1.0.0.0";
        public string Category { get; set; } = "general";
        public string Description { get; set; }
        public string Website { get; set; } = "https://editor.construct.net/";
        public string Documentation { get; set; } = "https://editor.construct.net/";
        public string Base64Icon { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();

        public string PropertyList
        {
            get
            {
                var propertyList = Properties.Select(property => PropertyTemplateFactory.Insatnce.Create(property)).ToList();
                return string.Join(",\n                     ", propertyList);
            }
        }
    }
}

