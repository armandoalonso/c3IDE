using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Action = c3IDE.Models.Action;

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
            c3addon.Actions = new Dictionary<string, Action>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToAction(c3addon, ace);
            var action = c3addon.Actions.FirstOrDefault().Value;
            Console.WriteLine(action.Ace);
            Console.WriteLine();
            Console.WriteLine(action.Language);
            Console.WriteLine();
            Console.WriteLine(action.Code);
        }

        [TestMethod]
        public void TestSimpleActionWithOneParam()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
  ""Params"": [ {
          ""Script"": ""AddAnyTypeParam"",
          ""Text"": ""Element"",
          ""Description"": ""Element to push"",
          ""DefaultValue"": null,
          ""ComboItems"": []
        }],
 ""Id"": ""0"",
 ""Flags"": ""af_none"",
 ""ListName"": ""Push"",
 ""Category"": ""General"",
 ""DisplayString"": ""Push {0}"",
 ""Description"": ""Push an element to the top of the stack"",
 ""ScriptName"": ""Push""
}");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Actions = new Dictionary<string, Action>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToAction(c3addon, ace);
            var action = c3addon.Actions.FirstOrDefault().Value;
            Console.WriteLine(action.Ace);
            Console.WriteLine();
            Console.WriteLine(action.Language);
            Console.WriteLine();
            Console.WriteLine(action.Code);
        }

        [TestMethod]
        public void TestSimpleActionWithTwoParam()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
  ""Params"": [ {
          ""Script"": ""AddAnyTypeParam"",
          ""Text"": ""Element"",
          ""Description"": ""Element to push"",
          ""DefaultValue"": null,
          ""ComboItems"": []
        },
        {
          ""Script"": ""AddAnyTypeParam"",
          ""Text"": ""Element"",
          ""Description"": ""Element to push"",
          ""DefaultValue"": null,
          ""ComboItems"": []
        }],
 ""Id"": ""0"",
 ""Flags"": ""af_none"",
 ""ListName"": ""Push"",
 ""Category"": ""General"",
 ""DisplayString"": ""Push {0}"",
 ""Description"": ""Push an element to the top of the stack"",
 ""ScriptName"": ""Push""
}");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Actions = new Dictionary<string, Action>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToAction(c3addon, ace);
            var action = c3addon.Actions.FirstOrDefault().Value;
            Console.WriteLine(action.Ace);
            Console.WriteLine();
            Console.WriteLine(action.Language);
            Console.WriteLine();
            Console.WriteLine(action.Code);
        }

        [TestMethod]
        public void TestSimpleActionWithComboParam()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
  ""Params"": [ {
          ""Script"": ""AddComboParam"",
          ""Text"": ""Element"",
          ""Description"": ""Element to push"",
          ""DefaultValue"": null,
          ""ComboItems"": [""one"", ""two"", ""three""]
        }],
 ""Id"": ""0"",
 ""Flags"": ""af_none"",
 ""ListName"": ""Push"",
 ""Category"": ""General"",
 ""DisplayString"": ""Push {0}"",
 ""Description"": ""Push an element to the top of the stack"",
 ""ScriptName"": ""Push""
}");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Actions = new Dictionary<string, Action>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToAction(c3addon, ace);
            var action = c3addon.Actions.FirstOrDefault().Value;
            Console.WriteLine(action.Ace);
            Console.WriteLine();
            Console.WriteLine(action.Language);
            Console.WriteLine();
            Console.WriteLine(action.Code);
        }



        [TestMethod]
        public void TestSimpleCondtion()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
      ""Params"": [],
      ""Id"": ""0"",
      ""Flags"": ""cf_none"",
      ""ListName"": ""Is empty"",
      ""Category"": ""General"",
      ""DisplayString"": ""Is empty"",
      ""Description"": ""Check if the stack is empty"",
      ""ScriptName"": ""IsEmpty""
 }");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Conditions = new Dictionary<string, Condition>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToCondition(c3addon, ace);
            var cnd = c3addon.Conditions.FirstOrDefault().Value;
            Console.WriteLine(cnd.Ace);
            Console.WriteLine();
            Console.WriteLine(cnd.Language);
            Console.WriteLine();
            Console.WriteLine(cnd.Code);
        }

        [TestMethod]
        public void TestSimpleCondtionWithOneParam()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"     {
      ""Params"": [
        {
          ""Script"": ""AddCmpParam"",
          ""Text"": ""Comaprison"",
          ""Description"": ""How to compare the count to"",
          ""DefaultValue"": null,
          ""ComboItems"": []
        }
      ],
      ""Id"": ""1"",
      ""Flags"": ""cf_trigger cf_fake_trigger, cf_static cf_not_invertible cf_deprecated cf_incompatible_with_triggers cf_looping"",
      ""ListName"": ""Compare count"",
      ""Category"": ""General"",
      ""DisplayString"": ""Compare count"",
      ""Description"": ""Compare a number with the count of the stack"",
      ""ScriptName"": ""CompareCount""
    }");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Conditions = new Dictionary<string, Condition>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToCondition(c3addon, ace);
            var cnd = c3addon.Conditions.FirstOrDefault().Value;
            Console.WriteLine(cnd.Ace);
            Console.WriteLine();
            Console.WriteLine(cnd.Language);
            Console.WriteLine();
            Console.WriteLine(cnd.Code);
        }

        [TestMethod]
        public void TestSimpleCondtionWithTwoParam()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
      ""Params"": [
        {
          ""Script"": ""AddCmpParam"",
          ""Text"": ""Comaprison"",
          ""Description"": ""How to compare the count to"",
          ""DefaultValue"": null,
          ""CopmboItems"": []
        },
        {
          ""Script"": ""AddNumberParam"",
          ""Text"": ""Number"",
          ""Description"": ""Number to compare the count to"",
          ""DefaultValue"": null,
          ""CopmboItems"": []
        }
      ],
      ""Id"": ""1"",
      ""Flags"": ""cf_none"",
      ""ListName"": ""Compare count"",
      ""Category"": ""General"",
      ""DisplayString"": ""Compare count"",
      ""Description"": ""Compare a number with the count of the stack"",
      ""ScriptName"": ""CompareCount""
    }");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Conditions = new Dictionary<string, Condition>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToCondition(c3addon, ace);
            var cnd = c3addon.Conditions.FirstOrDefault().Value;
            Console.WriteLine(cnd.Ace);
            Console.WriteLine();
            Console.WriteLine(cnd.Language);
            Console.WriteLine();
            Console.WriteLine(cnd.Code);
        }

        [TestMethod]
        public void TestSimpleCondtionWithComboParam()
        {
            var ace = JsonConvert.DeserializeObject<C2Ace>(@"{
  ""Params"": [ {
          ""Script"": ""AddComboParam"",
          ""Text"": ""Element"",
          ""Description"": ""Element to push"",
          ""DefaultValue"": null,
          ""ComboItems"": [""one"", ""two"", ""three""]
        }],
 ""Id"": ""0"",
 ""Flags"": ""af_none"",
 ""ListName"": ""Push"",
 ""Category"": ""General"",
 ""DisplayString"": ""Push {0}"",
 ""Description"": ""Push an element to the top of the stack"",
 ""ScriptName"": ""Push""
}");
            var c3addon = new C3Addon { Type = PluginType.SingleGlobalPlugin, };
            c3addon.Conditions = new Dictionary<string, Condition>();
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);
            C2AddonConverter.Insatnce.C2AcesToCondition(c3addon, ace);
            var cnd = c3addon.Conditions.FirstOrDefault().Value;
            Console.WriteLine(cnd.Ace);
            Console.WriteLine();
            Console.WriteLine(cnd.Language);
            Console.WriteLine();
            Console.WriteLine(cnd.Code);
        }


        [TestMethod]
        public void SimpleC2AddonImportTest()
        {
            var addon = JsonConvert.DeserializeObject<C2Addon>(File.ReadAllText("TestFiles\\example_parsed.json"));
            var c3addon = C2AddonConverter.Insatnce.ConvertToC3(addon);
        }
    }
}
