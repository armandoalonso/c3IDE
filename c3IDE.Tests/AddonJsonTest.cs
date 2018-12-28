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
    public class AddonJsonTest
    {
        [TestMethod]
        public void VerifySingleGlobalAddonJson()
        {
            var template = TemplateFactory.Insatnce.CreateTemplate(PluginType.SingleGlobalPlugin);
            var data = C3PluginFactory.Insatnce.Create(template, new ApplicationOptions {Author = "Steve", Company = "Company2"});

            var compiled = TemplateCompiler.Insatnce.CompileTemplates(template.AddonJson, data);

            Assert.AreEqual(@"{
     ""is-c3-addon"": true,
     ""type"": ""plugin"",
     ""name"": ""New Plugin"",
     ""id"": ""Company2_NewPlugin"",
     ""version"": ""0.0.0.1"",
     ""author"": ""Steve"",
     ""website"": ""https://github.com/armandoalonso/c3IDE"",
     ""documentation"": ""https://github.com/armandoalonso/c3IDE"",
     ""description"": ""This plugin was created using c3IDE"",
     ""editor-scripts"": [
                              ""plugin.js"",
                              ""type.js"",
                              ""instance.js""
                         ],
     ""file-list"": [
          ""c3runtime/plugin.js"",
          ""c3runtime/type.js"",
          ""c3runtime/instance.js"",
          ""c3runtime/conditions.js"",
          ""c3runtime/actions.js"",
          ""c3runtime/expressions.js"",
          ""lang/en-US.json"",
          ""aces.json"",
          ""addon.json"",
          ""icon.png"",
          ""instance.js"",
          ""plugin.js"",
          ""type.js""
     ]
}", compiled);
        }
    }
}
