using System;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace c3IDE.Tests
{
    [TestClass]
    public class C2AddonConverterTest
    {
        [TestMethod]
        public void TestSimpleAction()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
  ""Params"": [],
  ""Id"": ""2"",
  ""Flags"": ""af_none"",
  ""ListName"": ""Clear"",
  ""Category"": ""General"",
  ""DisplayString"": ""Clear"",
  ""Description"": ""Clears the stack"",
  ""ScriptName"": ""Clear""
}");
            var c3addon = new C3Addon{Type = PluginType.SingleGlobalPlugin,};
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);

            var action = C2AddonConverter.Insatnce.C2AcesToAction(c3addon, ace);
        }
    }
}
