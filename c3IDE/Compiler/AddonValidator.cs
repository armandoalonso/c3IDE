using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using c3IDE.Utilities.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = System.Action;

namespace c3IDE.Compiler
{
    public class AddonValidator : Singleton<AddonValidator>
    {
        public Action<string> UpdateLogText;

        public bool Validate(C3Addon addon)
        {
            var _log = AppData.Insatnce.CompilerLog;

            _log.Insert("***validating json files***");
            _log.Insert("==============================");
            var isValid = ValidateJsonFiles(addon);

            if (isValid)
            {
                _log.Insert("==============================");
                _log.Insert("***validating json files - completed***");
                _log.Insert("***all json files are valid***");
            }
            else
            {
                _log.Insert("==============================");
                _log.Insert("***validating json files - has errors***");
            }

            return isValid;
        }

        private bool ValidateJsonFiles(C3Addon addon)
        {
            var _log = AppData.Insatnce.CompilerLog;
            var isValid = true;

            //addon.json
            isValid = TryAction(() => JObject.Parse(addon.AddonJson));
            if(!isValid) { _log.Insert("failed validation on addon.json"); return false; } else { _log.Insert("addon.json is valid json");}


            if (addon.Type != PluginType.Effect)
            {
                //aces.json
                foreach (var action in addon.Actions.Values)
                {
                    isValid = TryAction(() => JObject.Parse(action.Ace));
                    if (!isValid) { _log.Insert($"failed validation on action : {action.Id} ace.json"); return false; } else { _log.Insert($"action : {action.Id} ace.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(action.Language, true));
                    if (!isValid) { _log.Insert($"failed validation on action : {action.Id} lang.json"); return false; } else { _log.Insert($"action : {action.Id} lang.json is valid json"); }

                    //variadic param type warning
                    if (action.Ace.Contains("\"type\": \"variadic\""))
                    {
                        _log.Insert($"WARNING _ {action.Id} is using a variadic parameter, this feature is not documented in the C3 SDK => USE AT YOUR OWN RISK!");
                    }
                }

                foreach (var condition in addon.Conditions.Values)
                {
                    isValid = TryAction(() => JObject.Parse(condition.Ace));
                    if (!isValid) { _log.Insert($"failed validation on condition : {condition.Id} ace.json"); return false; } else { _log.Insert($"condition : {condition.Id} ace.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(condition.Language, true));
                    if (!isValid) { _log.Insert($"failed validation on condition : {condition.Id} lang.json"); return false; } else { _log.Insert($"condition : {condition.Id} lang.json is valid json"); }
                }

                foreach (var expression in addon.Expressions.Values)
                {
                    isValid = TryAction(() => JObject.Parse(expression.Ace));
                    if (!isValid) { _log.Insert($"failed validation on expression : {expression.Id} ace.json"); return false; } else { _log.Insert($"expression : {expression.Id} ace.json is valid json"); }

                    isValid = TryAction(() => FormatHelper.Insatnce.Json(expression.Language, true));
                    if (!isValid) { _log.Insert($"failed validation on expression : {expression.Id} lang.json"); return false; } else { _log.Insert($"expression : {expression.Id} lang.json is valid json"); }
                }

                //property lang
                isValid = TryAction(() => FormatHelper.Insatnce.Json(addon.LanguageProperties, true));
                if (!isValid) { _log.Insert($"failed validation on language properties json"); return false; } else { _log.Insert($"language properties json is valid json"); }

                //property categories
                isValid = TryAction(() => FormatHelper.Insatnce.Json(addon.LanguageCategories, true));
                if (!isValid) { _log.Insert($"failed validation on language categories json"); return false; } else { _log.Insert($"language categories json is valid json"); }

            }
            else
            {
                isValid = TryAction(() => FormatHelper.Insatnce.Json(addon.EffectLanguage, false));
                if (!isValid) { _log.Insert($"failed validation on effect language json"); return false; } else { _log.Insert($"effect language json is valid json"); }
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
            catch (Exception e)
            {
                AppData.Insatnce.CompilerLog.Insert(e.Message, "ERROR");
                return false;
            }
        }
    }
}
