using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Utilities
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(CreateInstance);

        public static T Insatnce => _instance.Value;

        private static T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}
