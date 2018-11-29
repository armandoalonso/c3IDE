using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginModels
{
    public class Plugin
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Author { get; set; }
        public string Version { get; set; } = "1.0.0.0";
        public string Category { get; set; } = "general";
        public string Description { get; set; }
        public Image Icon { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();

        public string EditTimeFile { get; set; }
        public string RunTimeFile { get; set; }

        public string EditTimeTemplate { get; set; }
        public string RunTimeTemplate { get; set; }
    }

    public class Property
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
