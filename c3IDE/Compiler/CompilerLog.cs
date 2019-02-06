using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c3IDE.Utilities;
using c3IDE.Utilities.Logging;
using ICSharpCode.AvalonEdit.Snippets;

namespace c3IDE.Compiler
{
    public class CompilerLog
    {
        public List<LogMessage>  Logs = new List<LogMessage>();
        private readonly List<Action<string>> _insertCallbacks = new List<Action<string>>();

        public CompilerLog()
        {
        }

        public int AddUpdateCallback(Action<string> callback)
        {
            var index = _insertCallbacks.Count;
            _insertCallbacks.Add(callback);
            return index;
        }

        public void Insert(string message, string type = "INFO")
        {
            var log = new LogMessage {Date = DateTime.Now, Message = message, Type = type};
            Logs.Add(log);

            foreach (var callback in _insertCallbacks)
            {
                callback?.Invoke(log.ToString());
            }
        }

        public string WrapLogger(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                LogManager.Insatnce.Exceptions.Add(ex);
                Insert($"error message => {ex.Message}");
                throw;
            }
        }

        public void RemoveCallback(int callbackIndex)
        {
            _insertCallbacks.RemoveAt(callbackIndex);
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
