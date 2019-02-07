using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Utilities.Extentions
{
    public static class DictionaryExtentions
    {
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addValue)
        {
            TValue existing;
            if (dict.TryGetValue(key, out existing))
            {
                dict[key] = addValue;
            }
            else
            {
                dict.Add(key, addValue);
            }

            return addValue;
        }

       
    }
}
