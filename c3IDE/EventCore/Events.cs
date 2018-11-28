using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.EventCore
{
    public class NewPluginEvents : EventMessageBase
    {
        public PluginTypeEnum Type { get; set; }
        public NewPluginEvents(object sender, object content) : base(sender, content)
        {
            Type =  (PluginTypeEnum)content;
        }
    }
}
