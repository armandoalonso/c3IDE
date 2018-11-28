using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.PluginModels;

namespace c3IDE.Framework
{
    public class Global : Singleton<Global>
    {
        public C3Plugin CurrentPlugin { get; set; }
    }
}
