using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities;

namespace c3IDE.Templates
{
    public class SingleGlobalPluginTemplate : ITemplate
    {
        public SingleGlobalPluginTemplate()
        {
            ResourceReader.Insatnce.LogResourceFiles();
            AddonJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Templates.Files.SingleGlobal.addon.txt");
        }

        public string AddonJson { get; }
    }
}
