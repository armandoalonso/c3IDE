using System;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class EditTimeTypeTest : TestBase
    {
        [TestMethod]
        public void VerifyEditTimeTypeJavascript()
        {
            var template = TemplateFactory.Insatnce.CreateTemplate(PluginType.SingleGlobalPlugin);
            var data = C3PluginFactory.Insatnce.Create(template, new ApplicationOptions { Author = "Steve", Company = "Company2" });
            var compiled = TemplateCompiler.Insatnce.CompileTemplates(template.EditTimeTypeJs, data);
            VerifyFile("edit_time_type.txt", compiled);
        }
    }
}
