using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.WebApp.Models
{
    public class WebAppSettings
    {
        public string Author { get; set; }
        public string ApiBase { get; set; }
        public int CookiePersistenceHours { get; set; }
    }
}
