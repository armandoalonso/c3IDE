using System;
using System.Collections.Generic;

namespace c3IDE.Utilities.Logging
{
    public class LogManager : Singleton<LogManager>
    {
        public List<Exception> Exceptions = new List<Exception>();
    }
}
