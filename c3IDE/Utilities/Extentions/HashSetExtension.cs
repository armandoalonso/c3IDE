using System.Collections.Generic;

namespace c3IDE.Utilities.Extentions
{
    public static class HashSetExtension
    {
        public static bool AddRange<T>(this HashSet<T> @this, IEnumerable<T> items)
        {
            bool allAdded = true;
            foreach (T item in items)
            {
                allAdded &= @this.Add(item);
            }
            return allAdded;
        }
    }
}
