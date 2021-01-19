using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public string FullName { get; set; }
    }
}
