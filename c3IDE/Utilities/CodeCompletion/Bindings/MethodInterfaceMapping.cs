using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace c3IDE.Utilities.CodeCompletion.Bindings
{
    public class MethodInterfaceMapping
    {
        public List<MethodInterface> Interfaces { get; set; }

        public MethodInterfaceMapping()
        {
            var returnJson = ResourceReader.Insatnce.GetResourceText("c3IDE.Utilities.CodeCompletion.Bindings.editor_return_type.json");
            Interfaces = JsonConvert.DeserializeObject<List<MethodInterface>>(returnJson).ToList();
        }
    }

    //this returns all the possible interfaces from a methods call, return type, param types
    public class MethodInterface
    {
        public string Method { get; set; }
        public List<string> Interface { get; set; }
    }
}
