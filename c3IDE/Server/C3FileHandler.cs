using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using c3IDE.Managers;
using uhttpsharp;

namespace c3IDE.Server
{
    public class C3FileHandler : IHttpRequestHandler
    {
        public static string DefaultMimeType { get; set; }
        public static string HttpRootDirectory { get; set; }
        public static IDictionary<string, string> MimeTypes { get; private set; }

        /// <summary>
        /// static constructor for c3 middleware
        /// </summary>
        static C3FileHandler()
        {
            DefaultMimeType = "application/json";
            MimeTypes = new Dictionary<string, string>
            {
                {".json", "application/json"},
                {".js", "application/javascript"},
                {".png", "image/png"},
                {".svg", "image/svg+xml" }
            };
        }

        /// <summary>
        /// resolve mime type from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path) ?? string.Empty;
            if (MimeTypes.ContainsKey(extension))
            {
                LogManager.CompilerLog.Insert($"mime type for response => {MimeTypes[extension]}", "C3");
                return MimeTypes[extension];
            }
            LogManager.CompilerLog.Insert($"default mime type for response => {DefaultMimeType}", "C3");
            return DefaultMimeType;
        }

        /// <summary>
        /// handles a request for c3 static file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Handle(IHttpContext context, Func<Task> next)
        {
            var requestPath = context.Request.Uri.OriginalString.TrimStart('/');
            var httpRoot = Path.GetFullPath(HttpRootDirectory ?? ".");
            var path = Path.GetFullPath(Path.Combine(httpRoot, requestPath));

            LogManager.CompilerLog.Insert($"resolved request path = > {path}", "C3");

            if (!File.Exists(path))
            {
                LogManager.CompilerLog.Insert($"file does not exists = > {path}", "ERROR");
                await next().ConfigureAwait(false);
                return;
            }

            //read file content
            var content = File.ReadAllText(path);
            //setup cors header / content type
            var responseHeader = new Dictionary<string, string>
            {
                {"Content-type", GetContentType(path)},
                {"Access-Control-Allow-Origin", "*"},
                {"Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept"},
            };
            //create response
            var response = new HttpResponse(HttpResponseCode.Ok, content, responseHeader, false);
            context.Response = response;
        }
    }
}
