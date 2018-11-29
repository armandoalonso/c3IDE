using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using c3IDE.PluginModels;

namespace c3IDE.Framework
{
    public class Global : Singleton<Global>
    {
        public C3Plugin CurrentPlugin { get; set; }
    }
}
