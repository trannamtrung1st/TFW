using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
