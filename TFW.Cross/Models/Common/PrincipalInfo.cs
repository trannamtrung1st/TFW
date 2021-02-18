using System;

namespace TFW.Cross.Models.Common
{
    public class PrincipalInfo
    {
        public string UserId { get; set; }
        public bool IsAuthenticated { get; set; } = false;

        [ThreadStatic]
        private static PrincipalInfo _current;
        public static PrincipalInfo Current
        {
            get
            {
                return _current ?? new PrincipalInfo();
            }
            set
            {
                _current = value;
            }
        }
    }
}
