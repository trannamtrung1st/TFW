using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models.Identity
{
    public class ClientInfo
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public ClientType Type { get; set; }
    }
}
