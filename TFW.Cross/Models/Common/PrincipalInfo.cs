using System;
using System.Web;
using TFW.Cross.Helpers;

namespace TFW.Cross.Models.Common
{
    public class PrincipalInfo
    {
        public string UserId { get; set; }
        public bool IsAuthenticated { get; set; } = false;

        // Use this if the User of HttpContext could be changed
        //public static PrincipalInfo Current => HttpContextProvider.Current.GetPrincipalInfo();

        [ThreadStatic]
        private static PrincipalInfo _current;
        public static PrincipalInfo Current
        {
            get
            {
                if (_current == null)
                    _current = HttpContext.Current.GetPrincipalInfo();

                return _current;
            }
        }
    }
}
