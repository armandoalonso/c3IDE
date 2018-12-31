using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class AceParam
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string InitalValue { get; set; }
        public Dictionary<string, string> Items { get; set; }
        public List<string> AllowedPluginIds { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
