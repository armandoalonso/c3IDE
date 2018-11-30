using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE
{
    public enum PluginTypeEnum
    {
        SingleGlobal = 0,
        Drawing = 1,
        Behavior = 3
    }

    public enum PropertySaveType
    {
        NewProperty,
        EditProperty
    }
}
