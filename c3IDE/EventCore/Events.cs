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
    public class UpdatePluginEvents : EventMessageBase
    {
        public C3Plugin PluginData { get; set; }
        public UpdatePluginEvents(object sender, object content) : base(sender, content)
        {
            PluginData = (C3Plugin)content;
        }
    }

    //this event is triggered when the user clicks the save button
    public class SavePluginEvents : EventMessageBase
    {
        public SavePluginEvents(object sender, object content) : base(sender, content)
        {
        }
    }

    //triggered when a code view type is changed
    public class ChangeCodeViewEvents : EventMessageBase
    {
        public EditiorViewData View { get; set; }
        public ChangeCodeViewEvents(object sender, object content) : base(sender, content)
        {
            View = (EditiorViewData)content;
        }
    }
}
