using System;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class EditTimeInstanceTest: TestBase
    {
        [TestMethod]
        public void VerifyEditTimePluginJavascript()
        {
            var template = TemplateFactory.Insatnce.CreateTemplate(PluginType.SingleGlobalPlugin);
            var data = C3PluginFactory.Insatnce.Create(template);
            var compiled = TemplateCompiler.Insatnce.CompileTemplates(template.EditTimeInstanceJs, data);
            VerifyFile("edit_time_instance.txt", compiled, true);
        }
    }
}
