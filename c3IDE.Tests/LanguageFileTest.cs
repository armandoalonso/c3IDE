using System;
using System.Collections.Generic;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = c3IDE.Models.Action;

namespace c3IDE.Tests
{
    [TestClass]
    public class LanguageFileTest : TestBase
    {

        private readonly List<Action> actions = new List<Action> {
              new Action {  CategoryId = "test1", Id = "no-param", ListName = "Test1_list", DisplayText = "Test1_display", Description = "Test1_desc"},
              new Action {  CategoryId = "test1", Highlight = true, Id = "yes-param", ListName = "Test2_list", DisplayText = "Test2_display", Description = "Test2_desc",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="test", Name = "param1_name", Description = "param1_desc"},
                      new AceParam { Id="param2", Type="number", InitalValue="5", Name = "param2_name", Description = "param2_desc" },
                  }
              },
              new Action {  CategoryId = "test2", Highlight = false, Id = "no-param", ListName = "Test3_list", DisplayText = "Test3_display", Description = "Test3_desc" },
              new Action {  CategoryId = "test2", Highlight = true, Id = "yes-param", ListName = "Test3_list", DisplayText = "Test3_display", Description = "Test3_desc",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="hello,world", Name = "param1_name", Description = "param1_desc"},
                      new AceParam { Id="param2", Type="number", InitalValue="10", Name = "param2_name", Description = "param2_desc" },
                  }
              },
        };

        private readonly List<Condition> conditions = new List<Condition> {
              new Condition {  CategoryId = "test1", Highlight = false, Id = "no-param", ScriptName = "Test1", IsLooping = true, ListName = "Test1_list", DisplayText = "Test1_display", Description = "Test1_desc"},
              new Condition {  CategoryId = "test1", Highlight = true, Id = "yes-param", ScriptName = "Test2", IsTrigger = true, ListName = "Test2_list", DisplayText = "Test2_display", Description = "Test2_desc",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="test", Name = "param1_name", Description = "param1_desc"},
                      new AceParam { Id="param2", Type="number", InitalValue="5", Name = "param2_name", Description = "param2_desc"},
                  }
              },
              new Condition {  CategoryId = "test2", Highlight = false, Id = "no-param", ScriptName = "Test3", IsInvertible = true, IsCompatibleWithTriggers = false, ListName = "Test3_list", DisplayText = "Test3_display", Description = "Test3_desc"},
              new Condition {  CategoryId = "test2", Highlight = true, Id = "yes-param", ScriptName = "Test4", IsInvertible = false, IsCompatibleWithTriggers = true, ListName = "Test4_list", DisplayText = "Test4_display", Description = "Test4_desc",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="hello,world", Name = "param1_name", Description = "param1_desc"},
                      new AceParam { Id="param2", Type="number", InitalValue="10", Name = "param2_name", Description = "param2_desc"},
                  }
              },
        };

        private readonly List<Expression> expressions = new List<Expression> {
              new Expression {  CategoryId = "test1", Highlight = false, Id = "no-param", ExpressionName = "Test1", ReturnType = "any", Description = "Test1_desc", TranslatedName = "Test1_trans" },
              new Expression {  CategoryId = "test1", Highlight = true, Id = "yes-param", ExpressionName = "Test2", ReturnType = "string", Description = "Test2_desc", TranslatedName = "Test2_trans",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="test", Name = "param1_name", Description = "param1_desc"},
                      new AceParam { Id="param2", Type="number", InitalValue="5", Name = "para2_name", Description = "param2_desc"},
                  }
              },
              new Expression {  CategoryId = "test2", Highlight = false, Id = "no-param", ExpressionName = "Test3", ReturnType = "number", Description = "Test3_desc", TranslatedName = "Test3_trans" },
              new Expression {  CategoryId = "test2", Highlight = true, Id = "yes-param", ExpressionName = "Test4", ReturnType = "any", IsVariadicParams = true , Description = "Test4_desc", TranslatedName = "Test4_trans",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="hello,world", Name = "param3_name", Description = "param3_desc"},
                      new AceParam { Id="param2", Type="number", InitalValue="10", Name = "param4_name", Description = "param4_desc"},
                  }
              },
        };


        [TestMethod]
        public void GenerateLanguageFile()
        {
            var template = TemplateFactory.Insatnce.CreateTemplate(PluginType.SingleGlobalPlugin);
            var data = C3PluginFactory.Insatnce.Create(template);

            var cat = new Dictionary<string, string>
            {
                { "test1", "Test1" },
                { "test2", "Test2" }
            };

            var aceData = new Aces
            {
                Actions = actions,
                Conditions = conditions,
                Expressions = expressions,
                Categories = cat
            };

            data.Aces = aceData;

            var results = LanaguageTemplateFactory.Insatnce.Create(data);
            VerifyFile("lang_file.txt", results, true);
        }
    }
}
