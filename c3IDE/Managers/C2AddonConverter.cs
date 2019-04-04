using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Templates;
using c3IDE.Templates.c3IDE.Templates;
using c3IDE.Utilities;
using c3IDE.Utilities.Extentions;
using c3IDE.Utilities.Helpers;
using Action = c3IDE.Models.Action;

namespace c3IDE.Managers
{
    public class C2AddonConverter : Singleton<C2AddonConverter>
    {
        public C3Addon ConvertToC3(C2Addon c2addon)
        {
            var c3addon = new C3Addon();
            c3addon.Name = c2addon.Properties["name"];
            c3addon.Class = c2addon.Properties["name"];
            c3addon.Author = c2addon.Properties["author"];
            c3addon.AddonId = c2addon.Properties["id"];
            c3addon.Description = c2addon.Properties["description"];
            c3addon.AddonCategory = c2addon.Properties["category"];
            c3addon.Effect = new Effect();
            c3addon.IconXml = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.icon.svg");
            c3addon.CreateDate = DateTime.Now;
            c3addon.LastModified = DateTime.Now;

            //add version
           c3addon.MajorVersion = 1;
           c3addon.MinorVersion = 0;
           c3addon.RevisionVersion = 0;
           c3addon.BuildVersion = 0;

            PluginType pluginType = PluginType.SingleGlobalPlugin;
            switch (c2addon.Type)
            {
                case "Plugin":
                {
                    if (c2addon.Properties["type"] == "object")
                    {
                        pluginType = PluginType.SingleGlobalPlugin; break;
                    }
                    pluginType = PluginType.DrawingPlugin; break;
                }
                case "Behavior": pluginType = PluginType.Behavior; break;
                case "Effect": pluginType = PluginType.Effect; break;
            }
            c3addon.Type = pluginType;

            //get templates based on type
            c3addon.Template = TemplateFactory.Insatnce.CreateTemplate(c3addon.Type);

            //add actions
            c3addon.Actions = new Dictionary<string, Models.Action>();
            foreach (var c2Action in c2addon.Actions)
            {
                C2AcesToAction(c3addon, c2Action);
            }

            //conditions
            c3addon.Conditions = new Dictionary<string, Models.Condition>();
            foreach (var c2Condition in c2addon.Conditions)
            {
                C2AcesToCondition(c3addon, c2Condition);
            }

            //expression
            c3addon.Expressions = new Dictionary<string, Models.Expression>();
            foreach (var c2Expression in c2addon.Expressions)
            {
                C2AceToExpression(c3addon, c2Expression);
            }

            //third party files
            if (c2addon.Properties.ContainsKey("dependency"))
            {
                //todo: handle third party files
            }
            
            //compile other files
            AddonManager.CompileTemplates(c3addon);

            return c3addon;
        }

        public void C2AcesToAction(C3Addon addon, C2Ace ace)
        {
            try
            {
                var action = new Action
                {
                    Category = ace.Category.ToLower(),
                    Id = ace.ScriptName.SplitCamelCase("-"),
                    DisplayText = ace.DisplayString,
                    ListName = ace.ListName,
                    Description = ace.Description,
                    Highlight = "false",
                    C2Id = ace.Id,
                    Deprecated = ace.Flags.Contains("af_deprecated").ToString().ToLower()
                };

                if (ace.Params.Any())
                {
                    action.Ace = TemplateCompiler.Insatnce.CompileTemplates(C2ImportTemplates.ActionAceImport, action);
                    action.Language = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionLanguage, action);
                    action.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionCode, action);

                    var paramId = 0;
                    //action params
                    foreach (var param in ace.Params)
                    {
                        var paramType = ResolveParamType(param);
                        action = paramType == "combo"
                            ? AceParameterHelper.Insatnce.GenerateParam(action, $"param{paramId}", paramType, param.DefaultValue, $"param{paramId}", param.Description, param.ComboItems)
                            : AceParameterHelper.Insatnce.GenerateParam(action, $"param{paramId}", paramType, param.DefaultValue, $"param{paramId}", param.Description);
                        paramId++;
                    }
                }
                else
                {
                    action.Ace = TemplateCompiler.Insatnce.CompileTemplates(C2ImportTemplates.ActionAceImport, action);
                    action.Language = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionLanguage, action);
                    action.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionCode, action);
                }

                addon.Actions.Add(action.Id, action);
            }
            catch (Exception ex)
            {
                //todo: do something here, add to log
                return;
            }
        }

        public void C2AcesToCondition(C3Addon addon, C2Ace ace)
        {
            try
            {
                var condition = new Condition
                {
                    Category = ace.Category.ToLower(),
                    Id = ace.ScriptName.SplitCamelCase("-"),
                    DisplayText = ace.DisplayString,
                    ListName = ace.ListName,
                    Description = ace.Description,
                    Highlight = "false",
                    C2Id = ace.Id,
                    Deprecated = ace.Flags.Contains("cf_deprecated").ToString().ToLower(),
                    Trigger = ace.Flags.Contains("cf_trigger").ToString().ToLower(),
                    FakeTrigger = ace.Flags.Contains("cf_fake_trigger").ToString().ToLower(),
                    Static = ace.Flags.Contains("cf_static").ToString().ToLower(),
                    Looping = ace.Flags.Contains("cf_looping").ToString().ToLower(),
                    Invertible = (!ace.Flags.Contains("cf_not_invertible")).ToString().ToLower(),
                    TriggerCompatible = (!ace.Flags.Contains("cf_incompatible_with_triggers")).ToString().ToLower(),
                };

                if (ace.Params.Any())
                {
                    condition.Ace = C2ImportTemplates.ConditionAceImport(condition);
                    condition.Language = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionLanguage, condition);
                    condition.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionCode, condition);

                    var paramId = 0;
                    //action params
                    foreach (var param in ace.Params)
                    {
                        var paramType = ResolveParamType(param);
                        condition = paramType == "combo"
                            ? AceParameterHelper.Insatnce.GenerateParam(condition, $"param{paramId}", paramType, param.DefaultValue, $"param{paramId}", param.Description, param.ComboItems)
                            : AceParameterHelper.Insatnce.GenerateParam(condition, $"param{paramId}", paramType, param.DefaultValue, $"param{paramId}", param.Description);
                        paramId++;
                    }
                }
                else
                {
                    condition.Ace = C2ImportTemplates.ConditionAceImport(condition);
                    condition.Language = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionLanguage, condition);
                    condition.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionCode, condition);
                }

                addon.Conditions.Add(condition.Id, condition);
            }
            catch (Exception ex)
            {
                //todo: do something here, add to log
                return;
            }
        }

        public void C2AceToExpression(C3Addon addon, C2Ace ace)
        {
            try
            {
                var exp = new Expression
                {
                    Category = ace.Category.ToLower(),
                    Id = ace.ScriptName.SplitCamelCase("-"),
                    TranslatedName = ace.DisplayString,
                    ReturnType = ResolveRetrunType(ace.Flags),
                    Description = ace.Description,
                    C2Id = ace.Id,
                    Deprecated = ace.Flags.Contains("ef_deprecated").ToString().ToLower(),
                    IsVariadicParameters = ace.Flags.Contains("ef_variadic_parameters").ToString().ToLower()
                };

                if (ace.Params.Any())
                {
                    exp.Ace = C2ImportTemplates.ExpressionAceImport(exp);
                    exp.Language = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionLanguage, exp);
                    exp.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionCode, exp);

                    var paramId = 0;
                    foreach (var param in ace.Params)
                    {
                        var paramType = ResolveParamType(param);
                        AceParameterHelper.Insatnce.GenerateParam(exp, $"param{paramId}", paramType, param.DefaultValue, $"param{paramId}", param.Description);
                        paramId++;
                    }
                }
                else
                {
                    exp.Ace = C2ImportTemplates.ExpressionAceImport(exp);
                    exp.Language = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionLanguage, exp);
                    exp.Code = TemplateCompiler.Insatnce.CompileTemplates(addon.Template.ActionCode, exp);
                }

                addon.Expressions.Add(exp.Id, exp);
            }
            catch (Exception ex)
            {
                //todo: do something here, add to log
                return;
            }
        }

        private string ResolveRetrunType(string aceFlags)
        {
            if (aceFlags.Contains("ef_return_number")) return "number";
            if (aceFlags.Contains("ef_return_string")) return "string";
            if (aceFlags.Contains("ef_return_any")) return "any";

            throw new Exception("no value return type specificed");
        }

        public string ResolveParamType(C2AceParam param)
        {
            switch (param.Script)
            {
                case "AddNumberParam": return "number";
                case "AddStringParam": return "string";
                case "AddAnyTypeParam": return "any";
                case "AddCmpParam": return "cmp";
                case "AddComboParam": return "combo";
                case "AddObjectParam": return "object";
                case "AddLayerParam": return "layer";
                case "AddLayoutParam": return "layout";
                case "AddKeybParam": return "keyb";
                case "AddAnimationParam": return "animation";
                case "AddAudioFileParam": return string.Empty; //todo: figure out what equivalent is in c3
                case "AddVariadicParams": return string.Empty; //todo: figure out what to dof or varadic params 
                case "AddFunctionNameParam": throw new Exception("AddFunctionNameParam not supported by C3SDK");
                default: return string.Empty;
            }
        }
    }
}
