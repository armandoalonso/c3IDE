using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginTemplates
{
    public interface ITemplate
    {
        string PropertyTemplate { get; }
        string EditTimePluginJs { get; }
        string RunTimePluginJs { get; }
    }
}
