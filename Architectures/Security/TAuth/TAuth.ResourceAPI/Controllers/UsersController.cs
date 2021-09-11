using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAuth.Resource.Cross.Models.User;
using TAuth.ResourceAPI.Entities;

namespace TAuth.ResourceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ResourceContext _context;

        public UsersController(ResourceContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var subject = int.Parse(User.FindFirst(JwtClaimTypes.Subject).Value);
            var claims = await _context.UserClaims.Where(uc => uc.UserId == subject)
                .Select(c => new UserProfileItem
                {
                    Type = c.ClaimType,
                    Value = c.ClaimValue
                }).ToArrayAsync();

            return Ok(claims);
        }
    }
}
