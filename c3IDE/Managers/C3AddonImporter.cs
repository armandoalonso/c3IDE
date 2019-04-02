using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Utilities.Helpers;
using Esprima.Ast;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = c3IDE.Models.Action;
using Expression = c3IDE.Models.Expression;

namespace c3IDE.Managers
{
    public static class C3AddonImporter
    {
        public static C3Addon Import(string path)
        {
            try
            {
                var fi = new FileInfo(path);
                var tmpPath = OptionsManager.CurrentOptions.DataPath + "\\tmp";
                if (Directory.Exists(tmpPath)) Directory.Delete(tmpPath, true);

                //unzip c3addon to temp location
                ZipFile.ExtractToDirectory(path, tmpPath);

                var addon = JObject.Parse(File.ReadAllText(Path.Combine(tmpPath, "addon.json")));
                string type = addon["type"].ToString();
                string id = addon["id"].ToString();

                if (type != "effect")
                {
                    string pluginEdit, pluginRun;
                    pluginEdit = File.ReadAllText(Path.Combine(tmpPath, $"{type}.js"));
                    pluginRun = File.ReadAllText(Path.Combine(tmpPath, "c3runtime", $"{type}.js"));
                    string typeEdit = File.ReadAllText(Path.Combine(tmpPath, $"type.js"));
                    string typeRun = File.ReadAllText(Path.Combine(tmpPath, "c3runtime", $"type.js"));
                    string instanceEdit = File.ReadAllText(Path.Combine(tmpPath, $"instance.js"));
                    string instanceRun = File.ReadAllText(Path.Combine(tmpPath, "c3runtime", $"instance.js"));

                    PluginType pluginType = PluginType.SingleGlobalPlugin;
                    string pluginCat = "other";
                    switch (type)
                    {
                        case "plugin":
                            pluginType = pluginEdit.Contains("SetPluginType(\"world\")") ? PluginType.DrawingPlugin : PluginType.SingleGlobalPlugin;
                            pluginCat = Regex.Match(pluginEdit, @"PLUGIN_CATEGORY = ""(?<cat>).*""").Groups["cat"].Value;
                            break;
                        case "behavior":
                            pluginType = PluginType.Behavior;
                            pluginCat = Regex.Match(pluginEdit, @"BEHAVIOR_CATEGORY = ""(?<cat>.*)""").Groups["cat"].Value;
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(pluginCat)) pluginCat = "other";

                    var ace = JObject.Parse(File.ReadAllText(Path.Combine(tmpPath, "aces.json")));
                    var lang = JObject.Parse(File.ReadAllText(Path.Combine(tmpPath, "lang", "en-US.json")))["text"][type + "s"][id.ToLower()];

                    var prop = "\"properties\": " + lang["properties"];
                    var cats = "\"aceCategories\": " + lang["aceCategories"];

                    //pasre ace implementations
                    var actFuncs = JavascriptManager.GetAllFunction(File.ReadAllText(Path.Combine(tmpPath, "c3runtime", "actions.js")));
                    var cndFuncs = JavascriptManager.GetAllFunction(File.ReadAllText(Path.Combine(tmpPath, "c3runtime", "conditions.js")));
                    var expFuncs = JavascriptManager.GetAllFunction(File.ReadAllText(Path.Combine(tmpPath, "c3runtime", "expressions.js")));

                    var actionList = new List<Models.Action>();
                    var conditionList = new List<Models.Condition>();
                    var expressionList = new List<Models.Expression>();

                    foreach (JProperty category in ace.Properties())
                    {
                        //parse actions
                        var ationJson = ace[category.Name]["actions"]?.ToString();
                        var actions = ationJson != null ? JArray.Parse(ationJson) : null;
                        if (actions != null)
                        {
                            foreach (var action in actions.Children<JObject>())
                            {
                                var actionId = action["id"].ToString();
                                var actionAce = action.ToString();
                                var actionLang = $"\"{actionId}\":" + lang["actions"][actionId];
                                var actionScript = action["scriptName"].ToString();
                                var actionParams = string.Empty;

                                if (action["params"] != null && action["params"].Children<JObject>().Any())
                                {
                                    var ep = action["params"].Children<JObject>().Select(x => x["id"].ToString());
                                    actionParams = string.Join(",", ep);
                                }

                                actFuncs.TryGetValue(actionScript.Trim(), out var code);
                                if(code == null) continue; //todo: need toad some feedback that function were skipped (warning)
                                var act = new Models.Action
                                {
                                    Id = actionId,
                                    Category = category.Name,
                                    Ace = actionAce,
                                    Language = actionLang,
                                    //Code = $"{actionScript}({string.Join(",", actionParams)}) {{ \n}}"
                                    Code = FormatHelper.Insatnce.Javascript(code) ?? string.Empty
                                };

                                actionList.Add(act);
                            }
                        }


                        //parse conditions
                        var conditionJson = ace[category.Name]["conditions"]?.ToString();
                        var conditions = conditionJson != null ? JArray.Parse(conditionJson) : null;
                        if (conditions != null)
                        {
                            foreach (var condition in conditions.Children<JObject>())
                            {
                                var conditionId = condition["id"].ToString();
                                var conditionAce = condition.ToString();
                                var conditionLang = $"\"{conditionId}\":" + lang["conditions"][conditionId];
                                var conditionScript = condition["scriptName"].ToString();
                                var conditionParams = string.Empty;

                                if (condition["params"] != null && condition["params"].Children<JObject>().Any())
                                {
                                    var ep = condition["params"].Children<JObject>().Select(x => x["id"].ToString());
                                    conditionParams = string.Join(",", ep);
                                }

                                cndFuncs.TryGetValue(conditionScript.Trim(), out var code);
                                if (code == null) continue; //todo: need toad some feedback that function were skipped (warning)
                                var cnd = new Models.Condition()
                                {
                                    Id = conditionId,
                                    Category = category.Name,
                                    Ace = conditionAce,
                                    Language = conditionLang,
                                    //Code = $"{conditionScript}({string.Join(",", conditionParams)}) {{ \n}}"
                                    Code = FormatHelper.Insatnce.Javascript(code) ?? string.Empty
                                };

                                conditionList.Add(cnd);
                            }
                        }
                       
                        //parse expression
                        var expressionJson = ace[category.Name]["expressions"]?.ToString();
                        var expressions = expressionJson != null ? JArray.Parse(expressionJson) : null;
                        if (expressions != null)
                        {
                            foreach (var expression in expressions.Children<JObject>())
                            {
                                var expressionId = expression["id"].ToString();
                                var expressionAce = expression.ToString();
                                var expressionLang = $"\"{expressionId}\":" + lang["expressions"][expressionId];
                                var expressionScript = expression["expressionName"].ToString();
                                var expressionParams = string.Empty;

                                if (expression["params"] != null && expression["params"].Children<JObject>().Any())
                                {
                                    var ep = expression["params"].Children<JObject>().Select(x => x["id"].ToString());
                                    expressionParams = string.Join(",", ep);
                                }

                                expFuncs.TryGetValue(expressionScript.Trim(), out var code);
                                if (code == null) continue; //todo: need toad some feedback that function were skipped (warning)
                                var exp = new Models.Expression()
                                {
                                    Id = expressionId,
                                    Category = category.Name,
                                    Ace = expressionAce,
                                    Language = expressionLang,
                                    //Code = $"{expressionScript}({expressionParams}) {{ \n}}"
                                    Code = FormatHelper.Insatnce.Javascript(expFuncs[expressionScript.Trim()]) ?? string.Empty
                                };

                                expressionList.Add(exp);
                            }
                        }

                    }

                    var files = Regex.Matches(pluginEdit, @"filename\s?:\s?(""|')(?<file>.*)(""|')");
                    var thirdPartyFiles = new List<ThirdPartyFile>();
                    foreach (Match match in files)
                    {
                        var fn = match.Groups["file"].ToString();
                        var info = new FileInfo(Path.Combine(Path.Combine(tmpPath, fn)));
                        var f = new ThirdPartyFile
                        {
                            Bytes = null,
                            Content = File.ReadAllText(info.FullName),
                            Extention = info.Extension,
                            FileName = fn.Replace("c3runtime/", string.Empty),
                            PluginTemplate = TemplateHelper.ThirdPartyFile(fn.Replace("c3runtime/", string.Empty))
                        };
                        thirdPartyFiles.Add(f);
                    }


                    //todo: create c3addon, and map parsed data to c3addon 
                    var c3addon = new C3Addon
                    {
                        AddonId = id,
                        AddonCategory = pluginCat,
                        Author = addon["author"]?.ToString(),
                        Class = addon["name"]?.ToString()?.Replace(" ", string.Empty),
                        Company = addon["author"]?.ToString(),
                        Name = addon["name"]?.ToString(),
                        Description = addon["description"]?.ToString(),
                        AddonJson = addon.ToString(),
                        PluginRunTime = pluginRun,
                        PluginEditTime = pluginEdit,
                        TypeEditTime = typeEdit,
                        TypeRunTime = typeRun,
                        InstanceEditTime = instanceEdit,
                        InstanceRunTime = instanceRun,
                        LanguageProperties = prop,
                        LanguageCategories = cats,
                        Id = Guid.NewGuid(),
                        CreateDate = DateTime.Now,
                        LastModified = DateTime.Now,
                        Type = pluginType
                    };

                    c3addon.Actions = new Dictionary<string, Action>();
                    c3addon.Conditions = new Dictionary<string, Condition>();
                    c3addon.Expressions = new Dictionary<string, Expression>();

                    foreach (var action in actionList)
                    {
                        c3addon.Actions.Add(action.Id, action);
                    }

                    foreach (var condition in conditionList)
                    {
                        c3addon.Conditions.Add(condition.Id, condition);
                    }

                    foreach (var expression in expressionList)
                    {
                        c3addon.Expressions.Add(expression.Id, expression);
                    }

                    if (File.Exists(Path.Combine(tmpPath, "icon.svg")))
                    {
                        c3addon.IconXml = File.ReadAllText(Path.Combine(tmpPath, "icon.svg"));
                    }
                    else
                    {
                        c3addon.IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
                    }
                    c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);

                    //todo: add support for third part files
                    c3addon.ThirdPartyFiles = new Dictionary<string, ThirdPartyFile>();
                    foreach (var thirdPartyFile in thirdPartyFiles)
                    {
                        c3addon.ThirdPartyFiles.Add(thirdPartyFile.FileName, thirdPartyFile);
                    }


                    return c3addon;
                }
                else
                {
                    throw new NotImplementedException("effect importing not implemented yet");
                }
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                throw;
            }

        }
    }
}
