using System;

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
