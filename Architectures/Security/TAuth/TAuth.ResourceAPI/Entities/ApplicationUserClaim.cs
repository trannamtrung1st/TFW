using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.ResourceAPI.Entities
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public ApplicationUserClaim()
        {
        }
    }
}
