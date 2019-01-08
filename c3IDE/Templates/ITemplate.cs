using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Templates
{
    public interface ITemplate
    {
        string AddonJson { get; }
        string PluginEditTime { get; }
        string PluginRunTime { get; }
        string TypeEditTime { get; }
        string TypeRunTime { get; }
        string InstanceEditTime { get; }
        string InstanceRunTime { get; }
        string ActionAces { get; }
        string ActionLanguage { get; }
        string ActionCode { get; }
        string LanguageProperty { get; }
    }
}
