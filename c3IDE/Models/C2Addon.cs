using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class C2Addon
    {
        public string Type { get; set; }
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        public List<C2Ace> Conditions = new List<C2Ace>();
        public List<C2Ace> Actions = new List<C2Ace>();
        public List<C2Ace> Expressions = new List<C2Ace>();
    }

    public class C2Ace
    {
        public string Id { get; set; }
        public string Flags { get; set; }
        public string ListName { get; set; }
        public string Category { get; set; }
        public string DisplayString { get; set; }
        public string Description { get; set; }
        public string ScriptName { get; set; }
        public List<C2AceParam> Params = new List<C2AceParam>();
    }

    public class C2AceParam
    {
        public string Script { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public List<string> CopmboItems { get; set; }
    }


}
