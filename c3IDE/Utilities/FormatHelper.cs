using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Compiler;
using c3IDE.Utilities.JsBeautifier;
using Newtonsoft.Json;

namespace c3IDE.Utilities
{
    public class FormatHelper : Singleton<FormatHelper>
    {
        private Beautifier jsBeautifier = new Beautifier();

        public string Json(string json, string name, CompilerLog log)
        {
            try
            {
                return Json(json);
            }
            catch (Exception ex)
            {
                log.Insert($"invalid json => {name}");
                log.Insert($"error message => {ex.Message}");
                throw;
            }
        }

        public string Json(string json)
        {
            var data = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public string Javascript(string js)
        {
            return jsBeautifier.Beautify(js);
        }
    }
}
