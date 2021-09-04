using System;
using System.Net.Http;

namespace TFW.Docs.ApiClient.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(HttpResponseMessage response)
        {
            Response = response;
        }

        public HttpResponseMessage Response { get; }
    }
}
