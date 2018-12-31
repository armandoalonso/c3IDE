using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Templates
{
    public class AceParamTemplateFactory : Singleton<AceParamTemplateFactory>
    {
        public string Create(AceParam param)
        {
            string template;
            switch (param.Type)
            {
                case "object":
                    var allowed = param.AllowedPluginIds.Any() ? $", \"allowedPluginIds\":[{string.Join(",", param.AllowedPluginIds.Select(x => $"\"{x}\""))}]" : string.Empty;
                    template = $"{{\"id\": \"{param.Id}\",\"type\": \"{param.Type}\"{allowed} }}";
                    break;
                case "combo":
                    var items = param.Items.Any() ? string.Join(",", param.Items.Keys.Select(x => $"\"{x}\"")) : string.Empty;
                    template = $"{{\"id\": \"{param.Id}\",\"type\": \"{param.Type}\", \"items\": [{items}] }}";
                    break;
                case "string":
                case "layer":
                case "objectname":
                case "animation":
                    var stringValue = string.IsNullOrWhiteSpace(param.InitalValue) ? $", \"initialValue\": \"{param.InitalValue}\"" : string.Empty;
                    template = $"{{\"id\": \"{param.Id}\",\"type\": \"{param.Type}\"{stringValue} }}"; ;
                    break;
                default:
                    var value = string.IsNullOrWhiteSpace(param.InitalValue) ? $", \"initialValue\": {param.InitalValue}" : string.Empty;
                    template = $"{{\"id\": \"{param.Id}\",\"type\": \"{param.Type}\"{value} }}"; ;
                    break;
            }
            return template;
        }

        public string CreateLanguage(AceParam param)
        {
            string template;
            switch (param.Type)
            {
                case "combo":
                    var items = string.Join(",\n", param.Items.Select(x => $"\"{x.Key}\": \"{x.Value}\"")) ;
                    template = $@"""{param.Id}"": {{
                        ""name"": {param.Name},
                        ""desc"": {param.Description},
                        ""items"": {{
                            {items}
                        }}
                    }}";
                    break;
                default:
                    template = $@"""{param.Id}"": {{
                        ""name"": {param.Name},
                        ""desc"": {param.Description}
                    }}";
                    break;
            }

            return template;
        }
    }
}
