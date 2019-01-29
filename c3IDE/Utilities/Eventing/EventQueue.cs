using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Utilities.Eventing
{
    public static class EventQueue
    {
        public static TinyMessengerHub Hub { get; set; } = new TinyMessengerHub();
    }
}
