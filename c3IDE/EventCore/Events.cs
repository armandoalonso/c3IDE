using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.PluginModels;

namespace c3IDE.EventCore
{
    //this event is called when a new plugin is created from the home screen 
    public class NewPluginEvents : EventMessageBase
    {
        public PluginTypeEnum Type { get; set; }
        public NewPluginEvents(object sender, object content) : base(sender, content)
        {
            Type =  (PluginTypeEnum)content;
        }
    }

    //this event is called when a new c3 plugin object is created from template, that c3 plugin object is boardcast to all user controls 
    public class PluginInitEvents : EventMessageBase
    {
        public C3Plugin PluginData { get; set; }
        public PluginInitEvents(object sender, object content) : base(sender, content)
        {
            PluginData = (C3Plugin)content;
        }
    }
}
