using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace c3IDE.Framework
{
    public class ResourceTextReader : Singleton<ResourceTextReader>
    {
        private readonly Assembly _currentAssmbley;
        private readonly Dictionary<string, string> _resourceCache;

        public ResourceTextReader()
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

        public string GetResourceImage(string name)
        {
            if (_resourceCache.ContainsKey(name))
            {
                return _resourceCache[name];
            }

            using (var stream = _currentAssmbley.GetManifestResourceStream(name))
            {
                var img = Image.FromStream(stream ?? throw new InvalidOperationException());
                var base64 = img.ImageToBase64();
                _resourceCache.Add(name, base64);
                return base64;
            }
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
