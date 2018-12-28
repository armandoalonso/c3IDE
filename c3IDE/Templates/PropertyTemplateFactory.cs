using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Templates
{
    public class PropertyTemplateFactory : Singleton<PropertyTemplateFactory>
    {
        public string Create(Property property)
        {
            string template;
            switch (property.Type)
            {
                case "combo":
                    var items = property.Items.Any() ? string.Join(",", property.Items.Select(x => $"\"{x}\"")) : string.Empty;
                    template = $"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\", {{ \"initialValue\":\"{property.Value}\", \"items\": ["+items+"] })";
                    break;
                case "color":
                    template = $"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\", {{ \"initialValue\":[{property.Value}] }})";
                    break;
                case "link":
                    var callbackType = property.ForEachInstance ? "\"for-each-instance\"" : "\"once -for-type\"";
                     template = $"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\", {{ \"linkCallback\":\"{property.Value}\", \"callbackType\":{callbackType} }})";
                    break;
                case "info":
                    template = $"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\", {{ \"infoCallback\":\"{property.Value}\" }})";
                    break;
                case "integer":
                case "float":
                case "percent":
                    var minMax = property.HasMinMax ? $", \"min\":{property.MinValue}, \"max\":{property.MaxValue}" : string.Empty;
                    var drag = property.HasDragSpeed ? $", \"dragSpeedMultiplier\": {property.DragSpeedValue}" : string.Empty;
                    template =$"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\", {{ \"initialValue\":{property.Value} {minMax}{drag} }})";
                    break;
                case "check":
                    template = $"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\",  {property.Value} )";
                    break;
                default:
                    template = $"new SDK.PluginProperty(\"{property.Type}\", \"{property.Id}\",  \"{property.Value}\" )";
                    break;
            }

            return template;
        }
    }
}
