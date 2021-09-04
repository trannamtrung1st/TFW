using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;

namespace TFW.Framework.Web.Features
{
    public interface ISimpleHttpRequestFeature
    {
        public IDictionary<string, StringValues> Form { get; set; }
        public string QueryString { get; }
        public string Protocol { get; }
        public string Path { get; }
        public string Method { get; }
        public bool IsHttps { get; }
        public string Host { get; }
        public string ContentType { get; }
        public long? ContentLength { get; }
        public string Scheme { get; }

        public Stream GetBody();
    }

    public class SimpleHttpRequestFeature : ISimpleHttpRequestFeature
    {
        public SimpleHttpRequestFeature(HttpRequest request)
        {
            ContentLength = request.ContentLength;
            ContentType = request.ContentType;
            Host = request.Host.Value;
            IsHttps = request.IsHttps;
            Method = request.Method;
            Path = request.Path;
            Protocol = request.Protocol;
            QueryString = request.QueryString.Value;
            Scheme = request.Scheme;

            if (request.HasFormContentType)
                Form = new Dictionary<string, StringValues>(request.Form);

            _body = request.Body;
        }

        public IDictionary<string, StringValues> Form { get; set; }
        public string QueryString { get; }
        public string Protocol { get; }
        public string Path { get; }
        public string Method { get; }
        public bool IsHttps { get; }
        public string Host { get; }
        public string ContentType { get; }
        public long? ContentLength { get; }
        public string Scheme { get; }

        private Stream _body;
        public Stream GetBody()
        {
            return _body;
        }
    }
}
