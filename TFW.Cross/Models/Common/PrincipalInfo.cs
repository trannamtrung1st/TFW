using System;

namespace TFW.Cross.Models.Common
{
    public class PrincipalInfo
    {
        public string UserId { get; set; }
        public bool IsAuthenticated { get; set; } = false;
    }
}
