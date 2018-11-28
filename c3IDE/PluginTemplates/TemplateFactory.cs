using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginTemplates
{
    public class TemplateFactory : Singleton<TemplateFactory>
    {
        public ITemplate CreateTemplate(PluginTypeEnum type)
        {
            switch (type)
            {
                case PluginTypeEnum.SingleGlobal:
                    return new SingleGlobalPluginTemplate();
            }

            throw new InvalidOperationException("Not Valid Plugin Type");
        }
    }
}
