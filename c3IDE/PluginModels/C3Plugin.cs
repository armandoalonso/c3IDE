﻿using c3IDE.PluginTemplates;
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

        public static C3Plugin CreatePlugin(ITemplate template)
        {
            //TODO: take into consideration the language properties in the model
            var data = new C3Plugin();

            data.Plugin = new Plugin
            {
                EditTimeTemplate = template.EditTimePluginJs,
                RunTimeTemplate = template.RunTimePluginJs,
                Version = "1.0.0.0",
                Author = "c3IDE",
                Category = "general",
                Name = "New Plugin",
                Company = "MyCompany",
                Description = "This is a new Single Global Plugin",
            };

            //setup icon from base64 string
            using (var ms = new MemoryStream(Convert.FromBase64String(template.IconBase64)))
            {
                data.Plugin.Icon = Image.FromStream(ms);
            }

            //compile data
            data.Plugin.EditTimeFile = TextCompiler.Insatnce.CompileTemplates(template.EditTimePluginJs, data);
            data.Plugin.RunTimeFile = TextCompiler.Insatnce.CompileTemplates(template.RunTimePluginJs, data);

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
