using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.WebApp.Pages.Shared
{
    public interface IStatusPage : IPageModel
    {
        public string StatusCodeStyle { get; }
        public int Code { get; }
        public string MessageTitle { get; }
        public string Message { get; }
        public string OriginalUrl { get; }
    }
}
