using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.PluginModels
{
    public class C3ModelBase
    {
        public virtual Dictionary<string, string> GetPropertyDictionary()
        {
            try
            {
                var dictionary = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name.ToLower(), prop => prop.GetValue(this, null)?.ToString() ?? string.Empty);

                return dictionary;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
