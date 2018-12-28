using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    public class EditTimeType
    {
        public string Constructor { get; set; } = @"constructor(sdkPlugin, iObjectType)
        {
             super(sdkPlugin, iObjectType);
        }";

        public List<string> Functions { get; set; } = new List<string>();
    }
}
