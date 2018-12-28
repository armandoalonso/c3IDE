using c3IDE.Models;
using c3IDE.Utilities;
using System.Threading.Tasks;
using Scriban;

namespace c3IDE.Templates
{
    public class TemplateCompiler : Singleton<TemplateCompiler>
    {
        public string CompileTemplates(string templates, C3Plugin data)
        {
            var templateData = Template.Parse(templates);
            return templateData.Render(data);
        }
    }
}
