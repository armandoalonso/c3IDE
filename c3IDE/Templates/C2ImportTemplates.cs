using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;

namespace c3IDE.Templates
{
    public class C2ImportTemplates
    {
        public static string ActionAceImport = @"{
	""id"": ""{{id}}"",
	""scriptName"": ""{{script_name}}"",
	""highlight"": {{highlight}},
    ""c2id"" : {{c2_id}},
    ""deprecated"" : {{deprecated}}
}";

        public static string ExpressionAceImport(Expression exp)
        {
            var isvariadic = exp.IsVariadicParameters == "true" ? ",\n	\"isVariadicParameters\": true" : string.Empty;

            return $@"{{
	""id"": ""{exp.Id}"",
    ""c2id"" : {exp.C2Id},
    ""deprecated"" : {exp.Deprecated},
	""expressionName"": ""{exp.ScriptName}"",
	""returnType"": ""{exp.ReturnType}""{isvariadic}
}}";
        }

        public static string ConditionAceImport(Condition cnd)
        {
            var trigger = cnd.Trigger == "true" ? ",\n	\"isTrigger\": true" : string.Empty;
            var faketrigger = cnd.FakeTrigger == "true" ? ",\n	\"isFakeTrigger\": true" : string.Empty;
            var isstatic = cnd.Static == "true" ? ",\n	\"isStatic\": true" : string.Empty;
            var looping = cnd.Looping == "true" ? ",\n	\"isLooping\": true" : string.Empty;
            var invertible = cnd.Invertible == "false" ? ",\n	\"isInvertible\": false" : string.Empty;
            var triggercompatible = cnd.TriggerCompatible == "false"
                ? ",\n	\"isCompatibleWithTriggers\": false"
                : string.Empty;

            return $@"{{
	""id"": ""{cnd.Id}"",
	""scriptName"": ""{cnd.ScriptName}"",
    ""c2id"" : {cnd.C2Id},
    ""deprecated"" : {cnd.Deprecated},
	""highlight"": {cnd.Highlight}{trigger}{faketrigger}{isstatic}{looping}{invertible}{triggercompatible}
}}";
        }

    }
}