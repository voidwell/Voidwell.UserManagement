using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Voidwell.UserManagement.Exceptions;
using Voidwell.UserManagement.Data.Models;
using Voidwell.UserManagement.Models;
using System.Linq;
using System.Security.Claims;
using System;

namespace Voidwell.UserManagement.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public AuthenticationService(SignInManager<ApplicationUser> signinManager,
            UserManager<ApplicationUser> userManager, IUserService userService,
            ILogger<AuthenticationService> logger)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
        }

        public async Task<AuthenticationResult> Authenticate(AuthenticationRequest authRequest)
        {
            var username = authRequest.Username;
            var password = authRequest.Password;

            var user = await _userService.GetUserByEmail(username);

            _logger.LogDebug($"Attempting to login user: {username}");

            if (user == null)
            {
                _logger.LogWarning($"Failed to find user: {username}");
                throw new UserNotFoundException();
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning($"Login attempt for locked out user: {username}");
                throw new UserLockedOutException();
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                await _userManager.AccessFailedAsync(user);
                _logger.LogWarning($"Failed to authenticate for user: {username}");
                throw new InvalidPasswordException();
            }

            _logger.LogDebug($"Successfully authenticated user: {username}");

            user.LastLoginDate = DateTimeOffset.UtcNow;
            await _userService.UpdateUser(user);

            var claims = (await _userManager.GetClaimsAsync(user)).ToList();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            return new AuthenticationResult
            {
                UserId = user.Id,
                Claims = claims
            };
        }
    }
}
