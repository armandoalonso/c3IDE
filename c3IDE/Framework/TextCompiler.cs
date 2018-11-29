using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.PluginModels;
using Scriban;

namespace c3IDE.Framework
{
    public class TextCompiler : Singleton<TextCompiler>
    {
        public string CompileTemplates(string templates, C3Plugin data)
        {
            var templateData = Template.Parse(templates);
            return templateData.Render(data);
        }
    }
}
