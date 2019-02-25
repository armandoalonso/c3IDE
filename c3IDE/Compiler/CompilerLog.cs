using System;
using System.Collections.Generic;
using c3IDE.Managers;
using c3IDE.Models;

namespace c3IDE.Compiler
{
    public class CompilerLog
    {
        public List<LogMessage>  Logs = new List<LogMessage>();
        private readonly List<Action<string>> _insertCallbacks = new List<Action<string>>();

        /// <summary>
        /// insert compile log callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public int AddUpdateCallback(Action<string> callback)
        {
            var index = _insertCallbacks.Count;
            _insertCallbacks.Add(callback);
            return index;
        }

        /// <summary>
        /// inserts a compile log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Insert(string message, string type = "Info")
        {
            var log = new LogMessage {Date = DateTime.Now, Message = message, Type = type};
            Logs.Add(log);

            foreach (var callback in _insertCallbacks)
            {
                callback?.Invoke(log.ToString());
            }
        }

        /// <summary>
        /// execute action, if error is thrown log and throw
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string WrapLogger(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                LogManager.AddErrorLog(ex);
                Insert($"error message => {ex.Message}", "Error");
                throw;
            }
        }

        /// <summary>
        /// removes callback from compile log
        /// </summary>
        /// <param name="callbackIndex"></param>
        public void RemoveCallback(int callbackIndex)
        {
            _insertCallbacks.RemoveAt(callbackIndex);
        }
    }
}
