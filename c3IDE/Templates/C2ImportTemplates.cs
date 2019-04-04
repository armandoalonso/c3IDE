using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Models;

namespace c3IDE.Templates
{
    public class C2ImportTemplates
    {
        public static string ActionAceImport = @"{
	""id"": ""{{id}}"",
	""scriptName"": ""{{script_name}}"",
	""highlight"": {{highlight}},
    ""c2id"" : {{c2_id}},
    ""deprecated"" : {{deprecated}}
}";

        public static string ExpressionAceImport(Expression exp)
        {
            var isvariadic = exp.IsVariadicParameters == "true" ? ",\n	\"isVariadicParameters\": true" : string.Empty;

            return $@"{{
	""id"": ""{exp.Id}"",
    ""c2id"" : {exp.C2Id},
    ""deprecated"" : {exp.Deprecated},
	""expressionName"": ""{exp.ScriptName}"",
	""returnType"": ""{exp.ReturnType}""{isvariadic}
}}";
        }

        public static string ConditionAceImport(Condition cnd)
        {
            var trigger = cnd.Trigger == "true" ? ",\n	\"isTrigger\": true" : string.Empty;
            var faketrigger = cnd.FakeTrigger == "true" ? ",\n	\"isFakeTrigger\": true" : string.Empty;
            var isstatic = cnd.Static == "true" ? ",\n	\"isStatic\": true" : string.Empty;
            var looping = cnd.Looping == "true" ? ",\n	\"isLooping\": true" : string.Empty;
            var invertible = cnd.Invertible == "false" ? ",\n	\"isInvertible\": false" : string.Empty;
            var triggercompatible = cnd.TriggerCompatible == "false"
                ? ",\n	\"isCompatibleWithTriggers\": false"
                : string.Empty;

            return $@"{{
	""id"": ""{cnd.Id}"",
	""scriptName"": ""{cnd.ScriptName}"",
    ""c2id"" : {cnd.C2Id},
    ""deprecated"" : {cnd.Deprecated},
	""highlight"": {cnd.Highlight}{trigger}{faketrigger}{isstatic}{looping}{invertible}{triggercompatible}
}}";
        }

        public static string GeneratePluginJs(C2Addon addon)
        {
            var id = addon.Properties["id"];
            var name = addon.Properties["name"];
            var author = addon.Properties["author"];
            var singleglobal = addon.Properties["flags"].Contains("pf_singleglobal").ToString().ToLower();
            var pluginType = addon.Properties["type"];
            var hasImage = addon.Properties["flags"].Contains("pf_texture").ToString().ToLower();
            var isTiled = addon.Properties["flags"].Contains("pf_tiling").ToString().ToLower();
            var acePos = addon.Properties["flags"].Contains("pf_position_aces") ? "\n                    this._info.AddCommonPositionACEs();" : string.Empty;
            var aceSize = addon.Properties["flags"].Contains("pf_size_aces") ? "\n                    this._info.AddCommonSizeACEs();" : string.Empty;
            var aceApp = addon.Properties["flags"].Contains("pf_appearance_aces") ? "\n                    this._info.AddCommonAppearanceACEs();" : string.Empty;
            var aceZor = addon.Properties["flags"].Contains("pf_zorder_aces") ? "\n                    this._info.AddCommonZOrderACEs();" : string.Empty;
            var aceAngle = addon.Properties["flags"].Contains("pf_angle_aces") ? "\n                    this._info.AddCommonAngleACEs();" : string.Empty;
            var effectAllowed = addon.Properties["flags"].Contains("pf_effects").ToString().ToLower();
            var predraw = addon.Properties["flags"].Contains("pf_predraw").ToString().ToLower();
            var nosize = (!addon.Properties["flags"].Contains("pf_nosize")).ToString().ToLower();
            

            var rotate = addon.Properties["rotatable"];

            var template = $@"""use strict"";
{{
            const PLUGIN_ID = ""{id}"";	
            const PLUGIN_VERSION = ""1.0.0.0"";
            const PLUGIN_CATEGORY = ""{addon.Properties["category"]}"";
	
            const PLUGIN_CLASS = SDK.Plugins.{id} = class {name}Plugin extends SDK.IPluginBase
            {{
                constructor()
                {{
                    super(PLUGIN_ID);
			
                    SDK.Lang.PushContext(""plugins."" + PLUGIN_ID.toLowerCase());
			
                    this._info.SetName(lang("".name""));
                    this._info.SetDescription(lang("".description""));
                    this._info.SetVersion(PLUGIN_VERSION);
                    this._info.SetCategory(PLUGIN_CATEGORY);
                    this._info.SetAuthor(""{author}"");
                    this._info.SetHelpUrl(lang("".help-url""));
                    this._info.SetIsSingleGlobal({singleglobal});
                    this._info.SetPluginType(""{pluginType}"");
                    this._info.SetIsResizable({nosize});			    // allow to be resized
			        this._info.SetIsRotatable({rotate});	            // allow to be rotated
			        this._info.SetHasImage({hasImage});
			        this._info.SetSupportsEffects({effectAllowed});		// allow effects
			        this._info.SetMustPreDraw({predraw});
                    this._info.SetIsTiled({isTiled});{acePos}{aceAngle}{aceApp}{aceSize}{aceZor}

                    this._info.SetSupportedRuntimes([""c3""]);
			
                    SDK.Lang.PushContext("".properties"");
	
                    this._info.SetProperties([

                    ]);
			
                    SDK.Lang.PopContext(); //.properties
                    SDK.Lang.PopContext();
                }}
            }};
	
            PLUGIN_CLASS.Register(PLUGIN_ID, PLUGIN_CLASS);
 }}";

            return template;
        }

        public static string GenerateBehaviorJs(C2Addon addon)
        {
            //
            var id = addon.Properties["id"];
            var name = addon.Properties["name"];
            var author = addon.Properties["author"];
            var onlyOne = addon.Properties["flags"].Contains("bf_onlyone").ToString().ToLower();


            var template = $@"""use strict"";
{{
            const BEHAVIOR_ID = ""{id}"";	
	        const BEHAVIOR_VERSION = ""1.0.0.0"";
	        const BEHAVIOR_CATEGORY = ""{addon.Properties["category"]}"";
	        
	        const BEHAVIOR_CLASS = SDK.Behaviors.{id} = class {name}Behavior extends SDK.IBehaviorBase
	        {{
	        	constructor()
	        	{{
			        super(BEHAVIOR_ID);
			        
			        SDK.Lang.PushContext(""behaviors."" + BEHAVIOR_ID.toLowerCase());
			        
			        this._info.SetName(lang("".name""));
			        this._info.SetDescription(lang("".description""));
			        this._info.SetVersion(BEHAVIOR_VERSION);
			        this._info.SetCategory(BEHAVIOR_CATEGORY);
			        this._info.SetAuthor(""{author}"");
			        this._info.SetHelpUrl(lang("".help-url""));
			        this._info.SetIsOnlyOneAllowed({onlyOne});

                    this._info.SetSupportedRuntimes([""c3""]);
			
                    SDK.Lang.PushContext("".properties"");
	
                    this._info.SetProperties([

                    ]);
			
                    SDK.Lang.PopContext(); //.properties
                    SDK.Lang.PopContext();
                }}
            }};
	
            PLUGIN_CLASS.Register(PLUGIN_ID, PLUGIN_CLASS);
 }}";

            return template;
        }
    }
}
