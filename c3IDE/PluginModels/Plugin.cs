using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginModels
{
    public class Plugin : C3ModelBase
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Author { get; set; }
        public string Version { get; set; } = "1.0.0.0";
        public string Category { get; set; } = "general";
        public string Description { get; set; }
        public Image Icon { get; set; }

        public List<Property> Properties { get; set; } = new List<Property>();
        public string EditTimeTemplate { get; set; }
        public string RunTimeTemplate { get; set; }

        public override Dictionary<string, string> GetPropertyDictionary()
        {
            var dictionary = base.GetPropertyDictionary();

            dictionary.Remove("properties");
            if (Properties.Any())
            {
                var propertyBuilder = new StringBuilder();
                foreach (var property in Properties)
                {
                    propertyBuilder.AppendLine(property.ToString());
                }
                dictionary.Add("properties", propertyBuilder.ToString());
            }

            return dictionary;
        }

    }
}
