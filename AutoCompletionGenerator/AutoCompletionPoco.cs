using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AutoCompletionGenerator
{
    public class AutoCompletionPoco
    {
        public string Text { get; set; }
        public int Type { get; set; }
        public string Container { get; set; }
        public string DescriptionText { get; set; }
    }
}
