using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Utilities
{
    public class ResourceReader : Singleton<ResourceReader>
    {
        private readonly Assembly _currentAssmbley;
        private readonly Dictionary<string, string> _resourceCache;

        public ResourceReader()
        {
            _currentAssmbley = Assembly.GetExecutingAssembly();
            _resourceCache = new Dictionary<string, string>();
        }

        public string GetResourceText(string name)
        {
            if (_resourceCache.ContainsKey(name))
            {
                return _resourceCache[name];
            }

            using (var stream = _currentAssmbley.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                var resource = reader.ReadToEnd();
                _resourceCache.Add(name, resource);
                return resource;
            }
        }

        public string GetResourceAsBase64(string name)
        {
            if (_resourceCache.ContainsKey(name))
            {
                return _resourceCache[name];
            }

            using (var stream = _currentAssmbley.GetManifestResourceStream(name))
            {
                if (stream != null)
                {
                    var inArray = new byte[(int)stream.Length];
                    var outArray = new char[(int)(stream.Length * 1.34)];
                    stream.Read(inArray, 0, (int)stream.Length);
                    Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
                    var newStream = new MemoryStream(Encoding.UTF8.GetBytes(outArray));

                    using (var reader = new StreamReader(newStream))
                    {
                        var resource = reader.ReadToEnd();
                        _resourceCache.Add(name, resource);
                        return resource;
                    }
                }
            }

            throw new InvalidOperationException("Failed to read base 64 icon");
        }

        public void LogResourceFiles()
        {
            var resources = _currentAssmbley.GetManifestResourceNames();
            foreach (var resource in resources)
            {
                Console.WriteLine(resource);
            }
        }
    }
}
