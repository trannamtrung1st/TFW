using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceAPI.Entities;
using TAuth.ResourceAPI.Models.User;

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

        [HttpGet("profile/{subject}")]
        public async Task<IActionResult> GetUserProfileAsync(int subject)
        {
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
