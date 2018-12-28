using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities;
using LiteDB;

namespace c3IDE.Models
{
    public class ApplicationOptions
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Company { get; set; } = "MyCompany";
        public string Author { get; set; } = "c3IDE";
        public string Website { get; set; } = "https://github.com/armandoalonso/c3IDE";
        public string Documentation { get; set; } = "https://github.com/armandoalonso/c3IDE";
    }
}
