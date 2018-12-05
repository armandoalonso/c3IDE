using c3IDE.PluginTemplates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Framework;
using LiteDB;

namespace c3IDE.PluginModels
{
    public class C3Plugin : IEquatable<C3Plugin>
    {
        [BsonId]
        public Guid Id { get; set; }

        public Plugin Plugin { get; set; }
        public Type Type { get; set; }

        public static C3Plugin CreatePlugin(ITemplate template)
        {
            var data = new C3Plugin
            {
                Plugin = new Plugin
                {
                    EditTimeTemplate = template.EditTimePluginJs,
                    RunTimeTemplate = template.RunTimePluginJs,
                    Version = "1.0.0.0",
                    Author = "c3IDE",
                    Category = "general",
                    Name = "New Plugin",
                    Company = "MyCompany",
                    Description = "This is a new Single Global Plugin",
                    //setup icon from base64 string
                    Icon = template.IconBase64.Base64ToImage(),
                },
                Type = new Type
                {
                    EditTimeTemplate = template.EditTimeTypeJs,
                    RunTimeTemplate = template.RunTimeTypeJs
                }
            };

            //compile data
            data.Plugin.EditTimeFile = TextCompiler.Insatnce.CompileTemplates(template.EditTimePluginJs, data);
            data.Plugin.RunTimeFile = TextCompiler.Insatnce.CompileTemplates(template.RunTimePluginJs, data);

            data.Type.EditTimeFile = TextCompiler.Insatnce.CompileTemplates(template.EditTimeTypeJs, data);
            data.Type.RunTimeFile = TextCompiler.Insatnce.CompileTemplates(template.RunTimeTypeJs, data);

            return data;
        }

        public bool Equals(C3Plugin other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((C3Plugin) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
