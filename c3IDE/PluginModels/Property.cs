using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Framework;

namespace c3IDE.PluginModels
{
    public class Property : C3ModelBase
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }

        public override string ToString()
        {
            if (Type == "string") Value = $"\"{Value}\"";
            return TextCompiler.Insatnce.CompileTemplates(Template, GetPropertyDictionary());
        }
    }
}
