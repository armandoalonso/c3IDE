using TinyMessenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.EventCore
{
    public class EventSystem : Singleton<EventSystem>
    {
        public TinyMessengerHub Hub { get; set; } = new TinyMessengerHub();
    }
}
