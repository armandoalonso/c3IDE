using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using c3IDE.Models;

namespace c3IDE.Templates
{
    public class TemplateHelper
    {
        /// <summary>
        /// creates language property with combo type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string LanguagePropertyCombo(string id)
        {
            return $@"    ""{id}"" : {{
        ""name"": ""property name"",
        ""desc"": ""property desc"",
        ""items"": {{
            ""item1"": ""item one"",
            ""item2"": ""item two"",
            ""item3"": ""item three"",
        }}
    }}";
        }

        /// <summary>
        /// creates language property with link type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string LanguagePropertyLink(string id)
        {
            return $@"    ""{id}"" : {{
        ""name"": ""property name"",
        ""desc"": ""property desc"",
        ""link-text"": ""link text"",
    }}";
        }

        /// <summary>
        /// creates default language property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string LanguagePropertyDefault(string id)
        {
           return $@"    ""{id}"" : {{
        ""name"": ""property_name"",
        ""desc"": ""property_desc""
    }}";
        }

        /// <summary>
        /// create language properties for language file
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static string LanguageProperty(string properties)
        {
            return $@"""properties"":{{
{properties}
}}";
        }

        /// <summary>
        /// create ace param
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        public static string AceParam(string id, string type, string initValue)
        {
            string value;
            if (!string.IsNullOrWhiteSpace(initValue))
            {
                initValue = initValue.Trim();
                initValue = initValue.Trim('"');
                value = $",\n            \"initialValue\":\"{initValue}\"";
            }
            else
            {
                value = string.Empty;
            }
            var items = type == "combo" ? ",\n            \"items\":[\"item1\",\"item2\",\"item3\"]" : string.Empty;
            var objects = type == "object" ? ",\n            \"allowedPluginIds\":[\"Sprite\"]" : string.Empty;
        //    return $@"    ""params"": [
        //{{
        //    ""id"": ""{id}"",
        //    ""type"": ""{type}""{value}{items}{objects}
        //}},";
            return $@"}},
        {{
            ""id"": ""{id}"",
            ""type"": ""{type}""{value}{items}{objects}
        }}";
        }

        /// <summary>
        /// create ace language counterpart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string AceLang(string id, string type, string name, string desc)
        {
            var items = type == "combo" ? ",\n            \"items\":{\n                \"item1\": \"item_one\",\n                \"item2\": \"item_two\",\n                \"item3\": \"item_three\"\n            }" : string.Empty;
            var variadic = type == "variadic" ? " {n}" : string.Empty;

            //    return $@"    ""params"": {{
            //""{id}"": {{
            //    ""name"": ""{name}{variadic}"",
            //    ""desc"": ""{desc}""{items}
            //}},";

            return $@"{{ ""{id}"": {{
                ""name"": ""{name}{variadic}"",
                ""desc"": ""{desc}""{items}
            }} }}";
        }

        public static string AceCode(string id, string scriptName, IEnumerable<string> paramList)
        {
            return AceCode(id, scriptName, false, paramList);
        }

        /// <summary>
        /// create ace code segment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scriptName"></param>
        /// <param name="variadic"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static string AceCode(string id, string scriptName, bool variadic, IEnumerable<string> paramList)
        {
            var ti = new CultureInfo("en-US", false).TextInfo;
            var param = ti.ToTitleCase(id.Replace("-", " ").ToLower()).Replace(" ", string.Empty);
            param = char.ToLowerInvariant(param[0]) + param.Substring(1);
            var prefix = variadic ? "..." : string.Empty;
            var paramlist = paramList.ToList();
            paramlist.Add($"{prefix}{param}");
            var list = string.Join(",", paramlist);
            return $"{scriptName}({list})";
        }

        /// <summary>
        /// create ace parameter when it's the first 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        public static string AceParamFirst(string id, string type, string initValue)
        {
            string value;
            if (!string.IsNullOrWhiteSpace(initValue))
            {
                initValue = initValue.Trim();
                initValue = initValue.Trim('"');
                value = $",\n            \"initialValue\":\"{initValue}\"";
            }
            else
            {
                value = string.Empty;
            }
          
            var items = type == "combo" ? ",\n            \"items\":[\"item1\",\"item2\",\"item3\"]" : string.Empty;
            var objects = type == "object" ? ",\n            \"allowedPluginIds\":[\"Sprite\"]" : string.Empty;
            return $@",    ""params"": [
        {{
            ""id"": ""{id}"",
            ""type"": ""{type}""{value}{items}{objects}
        }}
    ]
}}";
        }

        /// <summary>
        /// create ace language counterpart when its the first
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string AceLangFirst(string id, string type, string name, string desc)
        {
            var items = type == "combo" ? ",\n            \"items\":{\n                \"item1\": \"item_one\",\n                \"item2\": \"item_two\",\n                \"item3\": \"item_three\"\n            }" : string.Empty;
            var variadic = type == "variadic" ? " {n}" : string.Empty;
            return $@""",
	""params"": {{
        ""{id}"": {{
            ""name"": ""{name}{variadic}"",
            ""desc"": ""{desc}""{items}
        }}
    }}
}}";
        }

        /// <summary>
        /// creates ace code segment when it's the first
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scriptName"></param>
        /// <param name="variadic"></param>
        /// <returns></returns>
        public static string AceCodeFirst(string id, string scriptName, bool variadic = false)
        {
            var ti = new CultureInfo("en-US", false).TextInfo;
            var param = ti.ToTitleCase(id.Replace("-", " ").ToLower()).Replace(" ", string.Empty);
            param = char.ToLowerInvariant(param[0]) + param.Substring(1);

            var prefix = variadic ? "..." : string.Empty;
            return $"{scriptName}({prefix}{param})";
        }

        /// <summary>
        /// creates ace for conditions
        /// </summary>
        /// <param name="cnd"></param>
        /// <returns></returns>
        public static string CndAce(Condition cnd)
        {
            var trigger = cnd.Trigger == "true" ? ",\n	\"isTrigger\": true" : string.Empty;
            var faketrigger = cnd.FakeTrigger == "true" ? ",\n	\"isFakeTrigger\": true" : string.Empty;
            var isstatic = cnd.Static == "true" ? ",\n	\"isStatic\": true" : string.Empty;
            var looping = cnd.Looping == "true" ? ",\n	\"isLooping\": true" : string.Empty;
            var invertible = cnd.Invertible == "false" ? ",\n	\"isInvertible\": false" : string.Empty;
            var triggercompatible = cnd.TriggerCompatible == "false" ? ",\n	\"isCompatibleWithTriggers\": false" : string.Empty;

            return $@"{{
	""id"": ""{cnd.Id}"",
	""scriptName"": ""{cnd.ScriptName}"",
	""highlight"": {cnd.Highlight}{trigger}{faketrigger}{isstatic}{looping}{invertible}{triggercompatible}
}}";
        }

        /// <summary>
        /// creates ace for expressions
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string ExpAces(Expression exp)
        {
            var isvariadic = exp.IsVariadicParameters == "true" ? ",\n	\"isVariadicParameters\": true" : string.Empty;

            return $@"{{
	""id"": ""{exp.Id}"",
	""expressionName"": ""{exp.ScriptName}"",
	""returnType"": ""{exp.ReturnType}""{isvariadic}
}}";
        }

        /// <summary>
        /// creates third party file section
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ThirdPartyFile(string filename)
        {
            return $@"{{
	filename: ""c3runtime/{filename}"",
	type: ""inline-script""
}}";
        }
    }
}
