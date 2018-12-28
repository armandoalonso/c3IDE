using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Templates;
using c3IDE.Utilities;

namespace c3IDE.Models
{
    public class C3PluginFactory : Singleton<C3PluginFactory>
    {
        public C3Plugin Create(ITemplate template, ApplicationOptions options = null)
        {
            var plugin = new C3Plugin
            {
                Plugin = new Plugin
                {
                    Name = "New Plugin",
                    Category = "general",
                    Author = options?.Author ?? "c3IDE",
                    Version = "0.0.0.1",
                    Company = options?.Company ?? "MyCompany",
                    Description = "This plugin was created using c3IDE",
                    Documentation = options?.Documentation ?? "https://github.com/armandoalonso/c3IDE",
                    Website = options?.Website ?? "https://github.com/armandoalonso/c3IDE",
                    Base64Icon = template.Base64Icon,
                    Properties = new List<Property>
                    {
                        new Property
                        {
                            Description = "This is a test property",
                            Id = "test-property",
                            Type = "string",
                            Value = "TEST"
                        }
                    }
                }
            };

            return plugin;
        }
    }
}
