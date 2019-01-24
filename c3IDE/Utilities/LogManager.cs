using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Utilities
{
    public class LogManager : Singleton<LogManager>
    {
        public List<Exception> Exceptions = new List<Exception>();
    }
}
