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
        const PLUGIN_ID = ""{{plugin.company}}_{{plugin.name}}"";
        const PLUGIN_VERSION = ""{{plugin.version}}"";
        const PLUGIN_CATEGORY = ""{{plugin.category}}"";

        const PLUGIN_CLASS = SDK.Plugins.{{plugin.company}}_{{plugin.name}} = class {{plugin.name}}Plugin extends SDK.IPluginBase
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
			    this._info.SetAuthor(""{{plugin.author}}"");
			    this._info.SetHelpUrl(lang("".help-url""));
			    this._info.SetIsSingleGlobal(true);
			
			    // Support both the C2 and C3 runtimes
			    this._info.SetSupportedRuntimes([""c3""]);
			
			    SDK.Lang.PushContext("".properties"");
			
			    this._info.SetProperties([
                    {{for prop in properties}}
                        new SDK.PluginProperty(""{{prop.type}}"", ""{{prop.id}}"",  {{prop.value}})
                    {{end}}
                ]);
			
			    SDK.Lang.PopContext();		// .properties
			    SDK.Lang.PopContext();
		    }
        };

        PLUGIN_CLASS.Register(PLUGIN_ID, PLUGIN_CLASS);
}";

        public string RunTimePluginJs => @"""use strict"";
{
    C3.Plugins.{{plugin.company}}_{{plugin.name}} = class {{plugin.name}}Plugin extends C3.SDKPluginBase
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

    }
}
