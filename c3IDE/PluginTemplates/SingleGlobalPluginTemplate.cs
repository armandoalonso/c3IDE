using System;
using System.Collections.Generic;
using System.Drawing;
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
            IconBase64 = ResourceTextReader.Insatnce.GetResourceImage("c3IDE.PluginTemplates.TemplateFiles.icon.png");
        }

        public string EditTimePluginJs { get; set; }

        public string RunTimePluginJs { get; set; }

        public string IconBase64 { get; set; }

    }
}
