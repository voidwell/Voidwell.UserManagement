using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Voidwell.UserManagement.Services;

namespace Voidwell.UserManagement.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId:guid}")]
        public async Task<IEnumerable<Claim>> GetUserClaims(Guid userId)
        {
            var user = await _userService.GetUser(userId);
            var roles = await _userService.GetRoles(userId);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, userId.ToString()),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
