using System;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class EditTimePluginTest : TestBase
    {
        [TestMethod]
        public void VerifyEditTimePluginJavascript()
        {
            var template = TemplateFactory.Insatnce.CreateTemplate(PluginType.SingleGlobalPlugin);
            var data = C3PluginFactory.Insatnce.Create(template);
            var compiled = TemplateCompiler.Insatnce.CompileTemplates(template.EditTimePluginJs, data);
            VerifyFile("edit_time_plugin.txt", compiled);
        }
    }
}
