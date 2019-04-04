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
    public class C2AddonParser : Singleton<C2AddonParser>
    {
        private C2Addon c2addon;
        private Dictionary<string, string> AllFunctions;

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
                    //var value = prop.value.value.ToString();
                    var value = TryGet(
                        () => throw new Exception(),
                        () => prop.value.value.ToString(),
                        () =>
                        {
                            if (prop.value.type.ToString() == "BinaryExpression")
                                return GetBinaryExpression(prop.value.left, prop.value.right);
                            throw new RuntimeBinderException();
                        });

                    c2addon.Properties.Add(key, value);
                }
            }

            //get actions
            var body = p.body;
            var ace = new C2Ace(); 
            var comboOptions = new List<string>();

            foreach (var value in body)
            {
                //check for value to be of type expression statement
                if (value.type == "ExpressionStatement")
                {
                    var expression = value.expression;
                    string caller = TryGet(
                        () => string.Empty,
                        () => expression.callee.name.ToString()
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

                                    ace.Id = TryGet(
                                        () => string.Empty,
                                        () => arg.value.ToString());
                                    break;
                                case 1:
                                    ace.Flags = TryGet(
                                        () => throw new Exception(),
                                        () => arg.value.ToString(), 
                                        () => arg.name.ToString(),
                                        () =>
                                        {
                                            if (arg.type.ToString() == "BinaryExpression")
                                                return GetBinaryExpression(arg.left, arg.right);
                                            throw new RuntimeBinderException();
                                        });
                                    break;
                                case 2:
                                    ace.ListName = TryGet(
                                        () => string.Empty,
                                        () => arg.value.ToString());
                                    break;
                                case 3:
                                    ace.Category = TryGet(
                                        () => string.Empty,
                                        () => arg.value.ToString());
                                    break;
                                case 4:
                                    ace.DisplayString = TryGet(
                                        () => string.Empty,
                                        () => arg.value.ToString());
                                    break;
                                case 5:
                                    ace.Description = TryGet(
                                        () => string.Empty,
                                        () => arg.value.ToString());
                                    break;
                                case 6:
                                    ace.ScriptName = TryGet(
                                        () => string.Empty,
                                        () => arg.value.ToString());
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
                    }
                    else
                    {
                        var paramOptions = TryGet(
                            () => string.Empty,
                            () => value.expression.callee.name.ToString());
                        var param = new C2AceParam {Script = paramOptions};

                        //todo: if script is not one of the param functions => resolve function;

                        if (paramOptions == "AddComboParamOption")
                        {
                            comboOptions.Add(value.expression.arguments[0].value.ToString());
                        }

                        param.ComboItems = comboOptions;
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
                                        param.Text = TryGet(
                                            () => string.Empty,
                                            () => arg.value.ToString());
                                        break;
                                    case 1:
                                        param.Description = TryGet(
                                            () => string.Empty,
                                            () => arg.value.ToString());
                                        break;
                                    case 2: //check else
                                        param.DefaultValue = TryGet(
                                            () => string.Empty,
                                            () => arg.value.ToString());
                                        break;
                                }
                                index++;
                            }
                            ace.Params.Add(param);
                        }
                    }
                }
            }
        }

        public string TryGet(Func<string> err, params Func<string>[] acts)
        {
            foreach (var func in acts)
            {
                try
                {
                    return func();
                }
                catch (RuntimeBinderException)
                {
                    continue;
                }
            }

            return err();
        }

        public string GetBinaryExpression(dynamic left, dynamic right)
        {
            var lStr = string.Empty;
            var rStr = string.Empty;

            lStr = left.type.ToString() == "BinaryExpression" ? GetBinaryExpression(left.left, left.right) : TryGet(() => string.Empty, () => left.name.ToString()) ;
            rStr = right.type.ToString() == "BinaryExpression" ? GetBinaryExpression(right.left, right.right) : TryGet(() => string.Empty, () => right.name.ToString());

            return $"{lStr} {rStr}";
        }
    }
}
