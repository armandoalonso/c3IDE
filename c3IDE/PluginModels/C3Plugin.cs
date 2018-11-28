using c3IDE.PluginTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginModels
{
    public class C3Plugin
    {
        public Plugin Plugin { get; set; }

        public static C3Plugin CreatePlugin(ITemplate template)
        {
            //TODO: take into consideration the language properties in the model
            //TODO: create options section for common properties across plugins (author, company etc...)
            var data = new C3Plugin();
            data.Plugin = new Plugin
            {
                EditTimeTemplate = template.EditTimePluginJs,
                RunTimeTemplate = template.RunTimePluginJs,
                Version = "1.0.0.0",
                Author = "c3IDE",
                Category = "general",
                Name = "NewSingleGlobal",
                Company = "MyCompany",
                Description = "This is a new Single Global Plugin"
            };

            return data;
        }
    }
}
