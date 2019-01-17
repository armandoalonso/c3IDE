using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;
using c3IDE.Utilities;

namespace c3IDE.Templates
{
    public class TemplateFactory : Singleton<TemplateFactory>
    {
        public ITemplate CreateTemplate(PluginType type)
        {
            switch (type)
            {
                case PluginType.SingleGlobalPlugin:
                    return new SingleGlobalPluginTemplate();
                case PluginType.MultiInstance:
                    return new MultiInstancePluginTemplate(); 
            }

            throw new InvalidOperationException("Not Valid Plugin Type");
        }

    }
}
