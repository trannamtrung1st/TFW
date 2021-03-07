using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.WebAPI.Models
{
    public class ApiSettings
    {
        private static ApiSettings _instance;
        public static ApiSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ApiSettings();

                return _instance;
            }
        }
    }
}
