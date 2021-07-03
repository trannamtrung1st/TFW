﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.ResourceAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser()
        {
            Resources = new HashSet<ResourceEntity>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<ResourceEntity> Resources { get; set; }
    }
}