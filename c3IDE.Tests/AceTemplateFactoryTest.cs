using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Action = c3IDE.Models.Action;

namespace c3IDE.Tests
{
    [TestClass]
    public class AceTemplateFactoryTest : TestBase
    {

        private readonly List<Action> actions = new List<Action> {
              new Action {  CategoryId = "test1", Highlight = false, Id = "no-param", ScriptName = "Test1" },
              new Action {  CategoryId = "test1", Highlight = true, Id = "yes-param", ScriptName = "Test2",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="test" },
                      new AceParam { Id="param2", Type="number", InitalValue="5" },
                  }
              },
              new Action {  CategoryId = "test2", Highlight = false, Id = "no-param", ScriptName = "Test3" },
              new Action {  CategoryId = "test2", Highlight = true, Id = "yes-param", ScriptName = "Test4",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="hello,world" },
                      new AceParam { Id="param2", Type="number", InitalValue="10" },
                  }
              },
        };

        private readonly List<Condition> conditions = new List<Condition> {
              new Condition {  CategoryId = "test1", Highlight = false, Id = "no-param", ScriptName = "Test1", IsLooping = true },
              new Condition {  CategoryId = "test1", Highlight = true, Id = "yes-param", ScriptName = "Test2", IsTrigger = true,
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="test" },
                      new AceParam { Id="param2", Type="number", InitalValue="5" },
                  }
              },
              new Condition {  CategoryId = "test2", Highlight = false, Id = "no-param", ScriptName = "Test3", IsInvertible = true, IsCompatibleWithTriggers = false },
              new Condition {  CategoryId = "test2", Highlight = true, Id = "yes-param", ScriptName = "Test4", IsInvertible = false, IsCompatibleWithTriggers = true,
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="hello,world" },
                      new AceParam { Id="param2", Type="number", InitalValue="10" },
                  }
              },
        };

        private readonly List<Expression> expressions = new List<Expression> {
              new Expression {  CategoryId = "test1", Highlight = false, Id = "no-param", ExpressionName = "Test1", ReturnType = "any"},
              new Expression {  CategoryId = "test1", Highlight = true, Id = "yes-param", ExpressionName = "Test2", ReturnType = "string",
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="test" },
                      new AceParam { Id="param2", Type="number", InitalValue="5" },
                  }
              },
              new Expression {  CategoryId = "test2", Highlight = false, Id = "no-param", ExpressionName = "Test3", ReturnType = "number", },
              new Expression {  CategoryId = "test2", Highlight = true, Id = "yes-param", ExpressionName = "Test4", ReturnType = "any", IsVariadicParams = true ,
                  Params = new List<AceParam>
                  {
                      new AceParam { Id="param1", Type="string", InitalValue="hello,world" },
                      new AceParam { Id="param2", Type="number", InitalValue="10" },
                  }
              },
        };

        [TestMethod]
        public void GenerateActionStringTest()
        {
            var cat = new Dictionary<string, string>
            {
                { "test1", "Test1" },
                { "test2", "Test2" }
            };

            var aceData = new Aces
            {
                Actions = actions,
                Categories = cat
            };

            var results = AceTemplateFactory.Insatnce.Create(aceData);
            VerifyFile("ace_action.txt", results);
        }

        [TestMethod]
        public void GenerateConditionStringTest()
        {
            var cat = new Dictionary<string, string>
            {
                { "test1", "Test1" },
                { "test2", "Test2" }
            };

            var aceData = new Aces
            {
                Conditions = conditions,
                Categories = cat
            };

            var results = AceTemplateFactory.Insatnce.Create(aceData);
            VerifyFile("ace_condition.txt", results);
        }

        [TestMethod]
        public void GenerateExpressionStringTest()
        {
            var cat = new Dictionary<string, string>
            {
                { "test1", "Test1" },
                { "test2", "Test2" }
            };

            var aceData = new Aces
            {
                Expressions = expressions,
                Categories = cat
            };

            var results = AceTemplateFactory.Insatnce.Create(aceData);
            VerifyFile("ace_expression.txt", results);
        }


        [TestMethod]
        public void GenerateAllAcesStringTest()
        {
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

            var results = AceTemplateFactory.Insatnce.Create(aceData);
            VerifyFile("ace_all.txt", results);
        }
    }
}
