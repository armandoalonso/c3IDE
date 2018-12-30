using System.Collections.Generic;
using System.Linq;
using c3IDE.Models;
using c3IDE.Utilities;
using Action = c3IDE.Models.Action;

namespace c3IDE.Templates
{
    public class AceTemplateFactory : Singleton<AceTemplateFactory>
    {
        private Dictionary<string, List<Action>> _actions;
        private Dictionary<string, List<Condition>> _conditions;

        public string Create(Aces ace)
        {
            //setup containers based on categories
            _actions = new Dictionary<string, List<Action>>();
            _conditions = new Dictionary<string, List<Condition>>();

            //sort actions by categories
            foreach (var act in ace.Actions)
            {
                if (_actions.ContainsKey(act.CategoryId))
                {
                    _actions[act.CategoryId].Add(act);
                }
                else
                {
                    _actions.Add(act.CategoryId, new List<Action>());
                    _actions[act.CategoryId].Add(act);
                }
            }

            foreach (var cnd in ace.Conditions)
            {
                if (_conditions.ContainsKey(cnd.CategoryId))
                {
                    _conditions[cnd.CategoryId].Add(cnd);
                }
                else
                {
                    _conditions.Add(cnd.CategoryId, new List<Condition>());
                    _conditions[cnd.CategoryId].Add(cnd);
                }
            }

            //section string list
            var sections = new List<string>();
            //generate aces file
            foreach(var category in ace.Categories)
            {
                var currentActions =  _actions.ContainsKey(category.Key) ? _actions[category.Key] : new List<Action>();
                var currentConditions = _conditions.ContainsKey(category.Key) ? _conditions[category.Key] : new List<Condition>();
                sections.Add(GenerateJsonSection(category.Key, currentActions, currentConditions));
            }

            //TODO: use different formatting method
            //TODO: make sure extra comma's get removed by formatting function
            return JsonHelper.Insatnce.FormatJson("{"+ string.Join(",", sections) + "}");
        }

        //this function assumes all actions belong to the same category
        private string GenerateJsonSection(string categoryId, List<Action> actions, List<Condition> conditions)
        {
            var template = $@"        ""{categoryId}"":{{
        ""conditions"": [
            {GenerateConditionString(conditions)}
        ],
        ""actions"": [			
            {GenerateActionString(actions)}
        ],
        ""expressions"": [

        ]
    }}";

            return template;
        }

        private string GenerateActionString(List<Action> actions)
        {
            var actionList = new List<string>();
            foreach (var action in actions)
            {
                var highlight = action.Highlight ? "\"highlight\" : true" : string.Empty;
                var isdeprecated = action.IsDeprecated ? "\"isDeprecated\" : true" : string.Empty;
                var parameters = $@"                ""params"": [
                    {action.ParamList}
                ]";

                if (action.Params.Any())
                {
                    var props = string.Join(",\n", highlight, isdeprecated, parameters);
                    var template = $@"            {{
                ""id"": ""{action.Id}"",
                ""scriptName"": ""{action.ScriptName}"",
                {props}
            }}";
                    actionList.Add(template);
                }
                else
                {
                    var props = string.Join(",\n", highlight, isdeprecated);
                    var template = $@"            {{
                ""id"": ""{action.Id}"",
                ""scriptName"": ""{action.ScriptName}"",
                {props}
            }}";

                    actionList.Add(template);
                }
            }

            return string.Join(",\n", actionList);
        }

        private string GenerateConditionString(List<Condition> conditions)
        {
            var conditionList = new List<string>();
            foreach (var condition in conditions)
            {
                var highlight = condition.Highlight ? "\"highlight\" : true" : string.Empty;
                var istrigger = condition.IsTrigger ? "\"isTrigger\" : true" : string.Empty;
                var isfaketrigger = condition.IsFakeTrigger ? "\"isFakeTrigger\" : true" : string.Empty;
                var isstatic = condition.IsStatic ? "\"isStatic\" : true" : string.Empty;
                var islooping = condition.IsLooping ? "\"isLooping\" : true" : string.Empty;
                var isinvertible = !condition.IsInvertible ? "\"isInvertible\" : false" : string.Empty;
                var iscompatiblewithtrigger = !condition.IsCompatibleWithTriggers ? "\"isCompatibleWithTriggers\" : false" : string.Empty;
                var parameters = $@"                ""params"": [
                    {condition.ParamList}
                ]";

                if (condition.Params.Any())
                {
                    var props = string.Join(",\n", highlight, istrigger, isfaketrigger, isstatic, islooping, isinvertible, iscompatiblewithtrigger, parameters);
                    var template = $@"            {{
                ""id"": ""{condition.Id}"",
                ""scriptName"": ""{condition.ScriptName}"",
                {props}
            }}";
                    conditionList.Add(template);
                }
                else
                {
                    var props = string.Join(",\n", highlight, istrigger, isfaketrigger, isstatic, islooping, isinvertible, iscompatiblewithtrigger);
                    var template = $@"            {{
                ""id"": ""{condition.Id}"",
                ""scriptName"": ""{condition.ScriptName}"",
                {props}
            }}";

                    conditionList.Add(template);
                }
            }

            return string.Join(",\n", conditionList);
        }
    }
}
