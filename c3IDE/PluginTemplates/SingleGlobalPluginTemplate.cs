using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginTemplates
{
    class SingleGlobalPluginTemplate : ITemplate
    {
        public string EditTimePluginJs => @"""use strict"";
{
    const PLUGIN_ID = ""<@company@>_<@name@>"";
        const PLUGIN_VERSION = ""<@version@>"";
        const PLUGIN_CATEGORY = ""<@category@>"";

        const PLUGIN_CLASS = SDK.Plugins.<@company@>_<@name@> = class Log<@name@> extends SDK.IPluginBase
        {
            constructor()
		{
			super(PLUGIN_ID);

            SDK.Lang.PushContext(""plugins."" + PLUGIN_ID.toLowerCase());
			this._info.SetIcon(""icon.png"", ""image/png"");
			this._info.SetName(lang("".name""));
			this._info.SetDescription(lang("".description""));
			this._info.SetVersion(PLUGIN_VERSION);
			this._info.SetCategory(PLUGIN_CATEGORY);
			this._info.SetAuthor(""<@author@>"");
			this._info.SetHelpUrl(lang("".help-url""));
			this._info.SetIsSingleGlobal(true);
			
			// Support both the C2 and C3 runtimes
			this._info.SetSupportedRuntimes([""c3""]);
			
			SDK.Lang.PushContext("".properties"");
			
			this._info.SetProperties([
                <@properties@>
            ]);
			
			SDK.Lang.PopContext();		// .properties
			SDK.Lang.PopContext();
		}
    };

    PLUGIN_CLASS.Register(PLUGIN_ID, PLUGIN_CLASS);
}";

        public string RunTimePluginJs => @"""use strict"";
{
    C3.Plugins.<@company@>_<@name@> = class Log<@name@> extends C3.SDKPluginBase
    {
        constructor(opts)
	    {
			super(opts);
        }

        Release()
        {
            super.Release();
        }
    };
}";

        public string PropertyBasicTemplate => @"new SDK.PluginProperty(""<@prop_type@>"", ""<@prop-id@>"", <@prop-value@>)";
    }
}
