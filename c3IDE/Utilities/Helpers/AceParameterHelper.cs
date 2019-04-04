using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c3IDE.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace c3IDE.Utilities.Helpers
{
    public class AceParameterHelper : Singleton<AceParameterHelper>
    {
        public Models.Action GenerateParam(Models.Action action, string id, string type, string value, string name, string desc)
        {
            var isVariadic = type == "variadic";

            if (action.Ace.Contains("\"params\": ["))
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParam(id, type, value);

                //lang param
                var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                var newProperty = JObject.Parse(langTemplate);
                var langJson = JObject.Parse($"{{ {action.Language} }}")[action.Id];
                var langParams = langJson["params"];
                langParams.Last.AddAfterSelf(newProperty.Property(id));
                langJson["params"] = langParams;

                //code param
                var func = Regex.Match(action.Code, @"(?:\()(?<param>.*)(?:\))");
                var declaration = Regex.Match(action.Code, @".*(?:\()(?<param>.*)(?:\))").Value;
                var paramList = func.Groups["param"].Value.Split(',');
                var codeTemplate = TemplateHelper.AceCode(id, action.ScriptName, isVariadic, paramList);

                //updates
                action.Language = $"\"{action.Id}\": {langJson.ToString(formatting: Formatting.Indented)} ";
                action.Ace = FormatHelper.Insatnce.Json(Regex.Replace(action.Ace, @"}(\r\n?|\n|\s*)]", $"{aceTemplate}\r\n]"));
                action.Code = action.Code.Replace(declaration, codeTemplate);
            }
            //this will be the first param
            else
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParamFirst(id, type, value);

                //language param
                var langTemplate = TemplateHelper.AceLangFirst(id, type, name, desc);

                //code param
                var codeTemplate = TemplateHelper.AceCodeFirst(id, action.ScriptName, isVariadic);

                //updates
                action.Language = action.Language.Replace(@"""
}", langTemplate);
                action.Ace = FormatHelper.Insatnce.Json(action.Ace.Replace("}", aceTemplate));
                action.Code = action.Code.Replace($"{action.ScriptName}()", codeTemplate);
            }

            return action;
        }

        public Models.Condition GenerateParam(Models.Condition cnd, string id, string type, string value, string name, string desc)
        {
            if (cnd.Ace.Contains("\"params\": ["))
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParam(id, type, value);

                //lang param
                var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                var newProperty = JObject.Parse(langTemplate);
                var langJson = JObject.Parse($"{{ {cnd.Language} }}")[cnd.Id];
                var langParams = langJson["params"];
                langParams.Last.AddAfterSelf(newProperty.Property(id));
                langJson["params"] = langParams;

                //code param
                var func = Regex.Match(cnd.Code, @"(?:\()(?<param>.*)(?:\))");
                var declaration = Regex.Match(cnd.Code, @".*(?:\()(?<param>.*)(?:\))").Value;
                var paramList = func.Groups["param"].Value.Split(',');
                var codeTemplate = TemplateHelper.AceCode(id, cnd.ScriptName, paramList);

                //updates
                cnd.Language = $"\"{cnd.Id}\": {langJson.ToString(formatting: Formatting.Indented)} ";
                cnd.Ace = FormatHelper.Insatnce.Json(Regex.Replace(cnd.Ace, @"}(\r\n?|\n|\s*)]", $"{aceTemplate}\r\n]"));
                cnd.Code = cnd.Code.Replace(declaration, codeTemplate);
            }
            //this will be the first param
            else
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParamFirst(id, type, value);

                //language param
                var langTemplate = TemplateHelper.AceLangFirst(id, type, name, desc);

                //code param
                var codeTemplate = TemplateHelper.AceCodeFirst(id, cnd.ScriptName);

                //updates
                cnd.Language = cnd.Language.Replace(@"""
}", langTemplate);
                cnd.Ace = FormatHelper.Insatnce.Json(cnd.Ace.Replace("}", aceTemplate));
                cnd.Code = cnd.Code.Replace($"{cnd.ScriptName}()", codeTemplate);
            }

            return cnd;
        }

        public Models.Expression GenerateParam(Models.Expression exp, string id, string type, string value, string name, string desc)
        {
            if (exp.Ace.Contains("\"params\": ["))
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParam(id, type, value);

                //lang param
                var langTemplate = TemplateHelper.AceLang(id, type, name, desc);
                var newProperty = JObject.Parse(langTemplate);
                var langJson = JObject.Parse($"{{ {exp.Language} }}")[exp.Id];
                var langParams = langJson["params"];
                langParams.Last.AddAfterSelf(newProperty.Property(id));
                langJson["params"] = langParams;

                //code param
                var func = Regex.Match(exp.Code, @"(?:\()(?<param>.*)(?:\))");
                var declaration = Regex.Match(exp.Code, @".*(?:\()(?<param>.*)(?:\))").Value;
                var paramList = func.Groups["param"].Value.Split(',');
                var codeTemplate = TemplateHelper.AceCode(id, exp.ScriptName, paramList);

                //updates
                exp.Language = $"\"{exp.Id}\": {langJson.ToString(formatting: Formatting.Indented)} ";
                exp.Ace = FormatHelper.Insatnce.Json(Regex.Replace(exp.Ace, @"}(\r\n?|\n|\s*)]", $"{aceTemplate}\r\n]"));
                exp.Code = exp.Code.Replace(declaration, codeTemplate);
            }
            //this will be the first param
            else
            {
                //ace param
                var aceTemplate = TemplateHelper.AceParamFirst(id, type, value);

                //language param
                var langTemplate = TemplateHelper.AceLangFirst(id, type, name, desc);

                //code param
                var codeTemplate = TemplateHelper.AceCodeFirst(id, exp.ScriptName);

                //updates
                exp.Language = exp.Language.Replace(@"""
}", langTemplate);
                exp.Ace = FormatHelper.Insatnce.Json(exp.Ace.Replace("}", aceTemplate));
                exp.Code = exp.Code.Replace($"{exp.ScriptName}()", codeTemplate);
            }

            return exp;
        }
    }
}
 