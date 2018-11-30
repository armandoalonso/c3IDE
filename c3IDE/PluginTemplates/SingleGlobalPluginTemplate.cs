using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Framework;

namespace c3IDE.PluginTemplates
{
    class SingleGlobalPluginTemplate : ITemplate
    {
        public SingleGlobalPluginTemplate()
        {
            ResourceTextReader.Insatnce.LogResourceFiles();
            EditTimePluginJs = ResourceTextReader.Insatnce.GetResourceText("c3IDE.PluginTemplates.TemplateFiles.SingleGlobal.EditTimePluginTemplate.txt");
            RunTimePluginJs = ResourceTextReader.Insatnce.GetResourceText("c3IDE.PluginTemplates.TemplateFiles.SingleGlobal.RunTimePluginTemplate.txt");    
        }

        public string EditTimePluginJs { get; set; }

        public string RunTimePluginJs { get; set; }

    }
}
