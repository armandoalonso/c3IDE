using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Templates;

namespace c3IDE.Models
{
    public class Condition
    {
        public string CategoryId { get; set; }
        public string Id { get; set; }
        public string ScriptName { get; set; }
        public bool Highlight { get; set; }
        public bool IsDeprecated { get; set; }
        public bool IsTrigger { get; set; }
        public bool IsFakeTrigger { get; set; }
        public bool IsStatic { get; set; }
        public bool IsLooping { get; set; }
        public bool IsInvertible { get; set; }
        public bool IsCompatibleWithTriggers { get; set; }
        public List<AceParam> Params { get; set; } = new List<AceParam>();
        public string ParamList
        {
            get
            {
                var paramList = Params.Select(param => AceParamTemplateFactory.Insatnce.Create(param)).ToList();
                return string.Join(",\n               ", paramList);
            }
        }

        public string ListName { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }
    }
}
