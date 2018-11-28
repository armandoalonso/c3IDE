using System;
using System.Collections.Generic;
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
        public List<Property> Properties { get; set; }
        public string EditTimeTemplate { get; set; }
        public string RunTimeTemplate { get; set; }
    }
}
