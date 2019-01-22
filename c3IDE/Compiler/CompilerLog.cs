using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Snippets;

namespace c3IDE.Compiler
{
    public class CompilerLog
    {
        public List<LogMessage>  Logs = new List<LogMessage>();
        private readonly Action<string> _insertCallback;

        public CompilerLog(Action<string> callback = null)
        {
            if (callback != null)
            {
                _insertCallback = callback;
            }
        }

        public void Insert(string message, string type = "INFO")
        {
            var log = new LogMessage {Date = DateTime.Now, Message = message, Type = type};
            Logs.Add(log);
            _insertCallback?.Invoke(log.ToString());
        }

        public string WrapLogger(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                Insert($"error message => {ex.Message}");
            }
            return string.Empty;
        }
    }

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
