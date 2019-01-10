using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiteDB;

namespace c3IDE.Models
{
    public class Options
    {
        [BsonId]
        public Guid Key { get; set; } = Guid.Parse("e0cddcac-e99d-4338-ac91-b56b0db58ed0");
        public string CompilePath { get; set; }
    }
}
