using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Managers
{
    public class C2AddonConverter : Singleton<C2AddonConverter>
    {
        public C3Addon ConvertToC3(C2Addon c2addon)
        {
            var c3addon = new C3Addon();
            c3addon.Name = c2addon.Properties["name"];
            c3addon.Class = c2addon.Properties["name"];
            c3addon.Author = c2addon.Properties["author"];
            c3addon.AddonId = c2addon.Properties["id"];
            c3addon.Description = c2addon.Properties["description"];
            c3addon.AddonCategory = c2addon.Properties["category"];

            PluginType pluginType = PluginType.SingleGlobalPlugin;
            switch (c2addon.Type)
            {
                case "Plugin":
                {
                    if (c2addon.Properties["type"] == "object")
                    {
                        pluginType = PluginType.SingleGlobalPlugin; break;
                    }
                    pluginType = PluginType.DrawingPlugin; break;
                }
                case "Behavior": pluginType = PluginType.Behavior; break;
                case "Effect": pluginType = PluginType.Effect; break;
            }

            //add actions
            c3addon.Actions = new Dictionary<string, Models.Action>();
            foreach (var c2Action in c2addon.Actions)
            {

            }

            //conditions
            c3addon.Conditions = new Dictionary<string, Models.Condition>();
            foreach (var c2Condition in c2addon.Conditions)
            {

            }

            //expression
            c3addon.Expressions = new Dictionary<string, Models.Expression>();
            foreach (var c2Expression in c2addon.Expressions)
            {

            }

            c3addon.Type = pluginType;

            return c3addon;
        }
    }
}
