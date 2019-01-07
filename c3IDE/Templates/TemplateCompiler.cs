using System.Threading.Tasks;
using c3IDE.Utilities;
using Scriban;

namespace c3IDE.Templates
{
    namespace c3IDE.Templates
    {
        public class TemplateCompiler : Singleton<TemplateCompiler>
        {
            public string CompileTemplates(string templates, object data)
            {
                var templateData = Template.Parse(templates);
                return templateData.Render(data);
            }
        }
    }
}
