using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using Esprima;
using Esprima.Ast;
using Esprima.Utils;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = System.Action;

namespace c3IDE.Managers
{
    public class C2AddonImporter : Singleton<C2AddonImporter>
    {
        private C2Addon c2addon;

        public void ReadEdittimeJs(string source)
        {
            c2addon = new C2Addon();
            var parser = new JavaScriptParser(source);
            var program = parser.ParseProgram();
            var json = program.ToJsonString();
            dynamic ast = JObject.Parse(json);


            TraverseJavascriptTree(ast);
        }

        public void TraverseJavascriptTree(dynamic p)
        {
            // get addon type
            var config = p.body[0];
            if (config.type == "FunctionDeclaration")
            {
                switch (config.id.name.ToString())
                {
                    case "GetPluginSettings":
                        c2addon.Type = "Plugin";
                        break;
                    case "GetBehaviorSettings":
                        c2addon.Type = "Behavior";
                        break;
                    case "Effect":
                        c2addon.Type = "Effect";
                        break;
                    default:
                        c2addon.Type = "Unknown";
                        break;
                }

                //get properties
                var props = config.body.body[0].argument.properties;
                foreach (dynamic prop in props)
                {
                    var key = prop.key.value.ToString();
                    var value = prop.value.value.ToString();
                    c2addon.Properties.Add(key, value);
                }
            }

            //get actions
            var body = p.body;
            //var needToStartNewArray = true;
            var ace = new C2Ace(); 
            var comboOptions = new List<string>();

            foreach (var value in body)
            {
                //check for value to be of type expression statement
                if (value.type == "ExpressionStatement")
                {
                    var expression = value.expression;
                    string caller = TryGet(
                        () => expression.callee.name.ToString(),
                        () => "none"
                    );

                    if (caller == "ACESDone")
                    {
                        continue;
                    }
                    else if (caller == "AddCondition" || caller == "AddAction" || caller == "AddExpression")
                    {
                        var args = expression.arguments;

                        var index = 0;
                        foreach (var arg in args)
                        {
                            switch (index)
                            {
                                case 0:

                                    ace.Id = TryGet(() => arg.value.ToString().ToString(), () => "");
                                    break;
                                case 1:
                                    ace.Flags = TryGet(() => arg.value.ToString().ToString(), () => arg.name.ToString());
                                    break;
                                case 2:
                                    ace.ListName = TryGet(() => arg.value.ToString().ToString(), () => "");
                                    break;
                                case 3:
                                    ace.Category = TryGet(() => arg.value.ToString().ToString(), () => "");
                                    break;
                                case 4:
                                    ace.DisplayString = TryGet(() => arg.value.ToString().ToString(), () => "");
                                    break;
                                case 5:
                                    ace.Description = TryGet(() => arg.value.ToString().ToString(), () => "");
                                    break;
                                case 6:
                                    ace.ScriptName = TryGet(() => arg.value.ToString().ToString(), () => "");
                                    break;
                            }
                            index++;
                        }

                        switch (caller)
                        {
                            case "AddCondition":
                                c2addon.Conditions.Add(ace);
                                break;
                            case "AddAction":
                                c2addon.Actions.Add(ace);
                                break;
                            case "AddExpression":
                                c2addon.Expressions.Add(ace);
                                break;
                            default:
                                Console.WriteLine(@"unknown add");
                                break;
                        }

                        ace = new C2Ace();
                        //needToStartNewArray = true;
                    }
                    else
                    {
                        //needToStartNewArray = false;

                        var paramOptions = TryGet(() => value.expression.callee.name.ToString(), () => string.Empty);
                        if (paramOptions == "AddComboParamOption")
                        {
                            comboOptions.Add(value.expressions.arguments[0].value.ToString());
                        }

                        var param = new C2AceParam();
                        param.CopmboItems = comboOptions;
                        comboOptions = new List<string>();

                        var args = value.expression.arguments;
                        if (args != null)
                        {
                            var index = 0;
                            foreach (var arg in args)
                            {
                                switch (index)
                                {
                                    case 0:
                                        param.Text = TryGet(() => arg.value.ToString().ToString(), () => "");
                                        break;
                                    case 1:
                                        param.Description = TryGet(() => arg.value.ToString().ToString(), () => arg.name.ToString());
                                        break;
                                    case 2: //check else
                                        param.DefaultValue = TryGet(() => arg.value.ToString().ToString(), () => "");
                                        break;
                                }
                                index++;
                            }
                            param.Script = paramOptions;
                            ace.Params.Add(param);
                        }
                    }
                }
            }

        }

        public string TryGet(Func<string> act, Func<string> err)
        {
            try
            {
                return act();
            }
            catch (RuntimeBinderException)
            {
                return err();
            }
        }
    }
}
