using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Models
{
    public class SimpleHttpRequestFeature
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
            _body = request.Body;
        }

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
