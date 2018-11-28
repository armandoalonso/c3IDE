using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace c3IDE.EventCore
{
    public class EventMessageBase : GenericTinyMessage<object>
    {
        public EventMessageBase(object sender, object content) : base(sender, content)
        {
        }
    }
}
