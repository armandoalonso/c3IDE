using System;
using c3IDE.Managers;
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json.Linq;
using Action = System.Action;

namespace c3IDE.Compiler
{
    public class AddonValidator : Singleton<AddonValidator>
    {
        public Action<string> UpdateLogText;

        /// <summary>
        /// validates the addon
        /// </summary>
        /// <param name="addon"></param>
        /// <returns></returns>
        public bool Validate(C3Addon addon)
        {
            LogManager.CompilerLog.Insert("***validating json files***");
            LogManager.CompilerLog.Insert("==============================");
            var isValid = ValidateJsonFiles(addon);

            if (isValid)
            {
                LogManager.CompilerLog.Insert("==============================");
                LogManager.CompilerLog.Insert("***validating json files - completed***");
                LogManager.CompilerLog.Insert("***all json files are valid***");
            }
            else
            {
                LogManager.CompilerLog.Insert("==============================");
                LogManager.CompilerLog.Insert("***validating json files - has errors***");
            }

            return isValid;
        }

        private bool ValidateJsonFiles(C3Addon addon)
        {
            var isValid = true;

            if (addon.Type != PluginType.Effect && addon.Type != PluginType.Theme)
            {
                //addon.json
                isValid = TryAction(() => JObject.Parse(addon.AddonJson));
                if (!isValid) { LogManager.CompilerLog.Insert("failed validation on addon.json"); return false; } else { LogManager.CompilerLog.Insert("addon.json is valid json"); }

                //validate plugin.js edittime
                if (addon.PluginEditTime.Contains("this._info.AddFileDependency()"))
                {
                    LogManager.CompilerLog.Insert("file dependency in plugin.js cannot have empty params");
                    return false;
                }

                //aces.json
                foreach (var action in addon.Actions.Values)
                {
                    isValid = TryAction(() => JObject.Parse(action.Ace));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on action : {action.Id} ace.json"); return false; } else { LogManager.CompilerLog.Insert($"action : {action.Id} ace.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(action.Language, true));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on action : {action.Id} lang.json"); return false; } else { LogManager.CompilerLog.Insert($"action : {action.Id} lang.json is valid json"); }

                    //variadic param type warning
                    if (action.Ace.Contains("\"type\": \"variadic\""))
                    {
                        LogManager.CompilerLog.Insert($"WARNING _ {action.Id} is using a variadic parameter, this feature is not documented in the C3 SDK => USE AT YOUR OWN RISK!");
                    }
                }

                foreach (var condition in addon.Conditions.Values)
                {
                    isValid = TryAction(() => JObject.Parse(condition.Ace));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on condition : {condition.Id} ace.json"); return false; } else { LogManager.CompilerLog.Insert($"condition : {condition.Id} ace.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(condition.Language, true));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on condition : {condition.Id} lang.json"); return false; } else { LogManager.CompilerLog.Insert($"condition : {condition.Id} lang.json is valid json"); }
                }

                foreach (var expression in addon.Expressions.Values)
                {
                    isValid = TryAction(() => JObject.Parse(expression.Ace));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on expression : {expression.Id} ace.json"); return false; } else { LogManager.CompilerLog.Insert($"expression : {expression.Id} ace.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(expression.Language, true));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on expression : {expression.Id} lang.json"); return false; } else { LogManager.CompilerLog.Insert($"expression : {expression.Id} lang.json is valid json"); }
                }

                //property lang
                isValid = TryAction(() => FormatHelper.Insatnce.Json(addon.LanguageProperties, true));
                if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on language properties json"); return false; } else { LogManager.CompilerLog.Insert($"language properties json is valid json"); }

                //property categories
                isValid = TryAction(() => FormatHelper.Insatnce.Json(addon.LanguageCategories, true));
                if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on language categories json"); return false; } else { LogManager.CompilerLog.Insert($"language categories json is valid json"); }

            }
            else
            {
                foreach (var parameter in addon.Effect.Parameters.Values)
                {
                    isValid = TryAction(() => JObject.Parse(parameter.Json));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on effect param : {parameter.Key} addon.json"); return false; } else { LogManager.CompilerLog.Insert($"effect param : {parameter.Key}, addon.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(parameter.Lang, true));
                    if (!isValid) { LogManager.CompilerLog.Insert($"failed validation on effect param : {parameter.Key} lang.json"); return false; } else { LogManager.CompilerLog.Insert($"effect param : {parameter.Key} lang.json, is valid json"); }
                }
            }
            return true;
        }

        public bool TryAction(Action act)
        {
            try
            {
                act();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                LogManager.CompilerLog.Insert(ex.Message, "Error");
                return false;
            }
        }
    }
}
