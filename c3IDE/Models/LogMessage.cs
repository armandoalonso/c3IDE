using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Models
{
    //simple log structure
    public class LogMessage
    {
        public DateTime Date;
        public string Message;
        public string Type;

        public override string ToString()
        {
            return $"{Date} : {Type} => {Message}\n";
        }
    }
}
