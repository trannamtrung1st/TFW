﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Entities
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser()
        {
            UserRoles = new List<AppUserRole>();
            Notes = new List<Note>();
        }

        public string FullName { get; set; }

        public virtual IList<AppUserRole> UserRoles { get; set; }
        public virtual IList<Note> Notes { get; set; }
    }
}
