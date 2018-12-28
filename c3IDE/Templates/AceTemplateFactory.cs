using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;
using Action = c3IDE.Models.Action;

namespace c3IDE.Templates
{
    public class AceTemplateFactory : Singleton<AceTemplateFactory>
    {
        private Dictionary<string, List<Action>> _actions;

        public string Create(Aces ace)
        {
            //setup containers based on categories
            _actions = new Dictionary<string, List<Action>>();

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

            return string.Empty;
        }

        //this function assumes all actions belong to the same category
        private string GenerateJsonSection(string categoryId, List<Action> actions)
        {
            var template = $@"        ""{categoryId}"":{{
        ""conditions"": [
            
        ],
        ""actions"": [			
            
        ],
        ""expressions"": [

        ]
    }}";

            return string.Empty;
        }

        private string GenerateActionString(List<Action> actions)
        {
            var actionList = new List<string>();
            foreach (var action in actions)
            {
                var highlight = action.Highlight ? "true" : "false";
                if (action.Params.Any())
                {
                    var template = $@"            {{
                ""id"": ""{action.Id}"",
                ""scriptName"": ""{action.ScriptName}"",
                ""highlight"": {highlight},
                ""params"": [
                    //TODO : insert params here
                ]
            }}";
                    actionList.Add(template);
                }
                else
                {
                    var template = $@"            {{
                ""id"": ""{action.Id}"",
                ""scriptName"": ""{action.ScriptName}"",
                ""highlight"": {highlight}
            }}";

                    actionList.Add(template);
                }
            }

            return string.Join(",\n", actionList);
        }
    }
}
