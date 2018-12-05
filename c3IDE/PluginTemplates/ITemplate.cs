﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginTemplates
{
    public interface ITemplate
    {
        string EditTimePluginJs { get; }
        string RunTimePluginJs { get; }
        string EditTimeTypeJs { get; }
        string RunTimeTypeJs { get; }
        string IconBase64 { get; }
    }
}
