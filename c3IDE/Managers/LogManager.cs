using System;
using System.Collections.Generic;
using c3IDE.Compiler;
using c3IDE.Models;


namespace c3IDE.Managers
{
    public static class LogManager
    {
        public static List<LogMessage> Logs = new List<LogMessage>();
        public static List<Exception> Exceptions = new List<Exception>();
        public static List<LogMessage> ImportLog = new List<LogMessage>();
        public static CompilerLog CompilerLog = new CompilerLog();
        private static readonly List<Action<string>> _logCallbacks = new List<Action<string>>();
        private static readonly List<Action<string>> _errorCallbacks = new List<Action<string>>();

        /// <summary>
        /// adds a callback when ever a log message is added
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int AddLogCallback(Action<string> callback)
        {
            var index = _logCallbacks.Count;
            _logCallbacks.Add(callback);
            return index;
        }

        /// <summary>
        /// adds a callback whenever a log error is added
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static int AddErrorCallback(Action<string> callback)
        {
            var index = _errorCallbacks.Count;
            _errorCallbacks.Add(callback);
            return index;
        }

        /// <summary>
        /// adds a log and executes all the log callbacks that have subscribed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        public static void AddLogMessage(string message, string source = "Info")
        {
            var log = new LogMessage{Date = DateTime.Now, Message = message, Type = source};
            Logs.Add(log);

            foreach (var logCallback in _logCallbacks)
            {
                logCallback?.Invoke(log.ToString());
            }
        }

        /// <summary>
        /// adds an error log and executes all error callbacks
        /// </summary>
        /// <param name="ex"></param>
        public static void AddErrorLog(Exception ex)
        {
            Exceptions.Add(ex);
            var log = new LogMessage {Date = DateTime.Now, Message = $"{ex.Source} => \n{ex.Message}\n{ex.StackTrace}", Type = "Error"};
            Logs.Add(log);

            foreach (var errorCallback in _errorCallbacks)
            {
                errorCallback?.Invoke(log.ToString());
            }
        }

        public static void AddImportLogMessage(string message, string source = "IMPORT")
        {
            var log = new LogMessage { Date = DateTime.Now, Message = message, Type = source };
            ImportLog.Add(log);
        }

        public static void ClearImportLogMessage()
        {
            ImportLog.Clear();
        }
    }
}
