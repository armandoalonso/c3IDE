using System;
using System.Collections.Generic;
using c3IDE.Models;
using c3IDE.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = c3IDE.Models.Action;

namespace c3IDE.Tests
{
    [TestClass]
    public class AceTemplateFactoryTest : TestBase
    {
        [TestMethod]
        public void GenerateActionStringTest()
        {
            var cat = new Dictionary<string, string>();
            cat.Add("test1", "Test1");
            cat.Add("test2", "Test2");

            var aceData = new Aces
            {
                Actions = new List<Action>
                {
                    new Action {  CategoryId = "test1", Highlight = false, Id = "no-param", ScriptName = "Test1" },
                    new Action {  CategoryId = "test1", Highlight = true, Id = "yes-param", ScriptName = "Test2",
                        Params = new List<AceParam>
                        {
                            new AceParam { Id="param1", Type="string", InitalValue="test" },
                            new AceParam { Id="param2", Type="number", InitalValue="5" },
                        }},
                    new Action {  CategoryId = "test2", Highlight = false, Id = "no-param", ScriptName = "Test3" },
                    new Action {  CategoryId = "test2", Highlight = true, Id = "yes-param", ScriptName = "Test4",
                        Params = new List<AceParam>
                        {
                            new AceParam { Id="param1", Type="string", InitalValue="hello,world" },
                            new AceParam { Id="param2", Type="number", InitalValue="10" },
                        }},
                },
                Categories = cat
            };

            var results = AceTemplateFactory.Insatnce.Create(aceData);
            VerifyFile("ace_action.txt", results, true);
        }
    }
}
