using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class AddonJsonTest : TestBase
    {
        [TestMethod]
        public void VerifySingleGlobalAddonJson()
        {
            var template = TemplateFactory.Insatnce.CreateTemplate(PluginType.SingleGlobalPlugin);
            var data = C3PluginFactory.Insatnce.Create(template, new ApplicationOptions {Author = "Steve", Company = "Company2"});
            var compiled = TemplateCompiler.Insatnce.CompileTemplates(template.AddonJson, data);
            VerifyFile("addon_json.txt", compiled);
        }
    }
}
