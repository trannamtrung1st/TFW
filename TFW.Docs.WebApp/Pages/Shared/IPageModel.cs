using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.WebApp.Pages.Shared
{
    public interface IPageModel
    {
        string Title { get; }
        string Description { get; }
    }
}
