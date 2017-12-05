using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using Voidwell.UserManagement.Exceptions;

namespace Voidwell.UserManagement
{
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserIdFromContext()
        {
            var claimUserId = _httpContextAccessor.HttpContext.User.FindFirst(JwtClaimTypes.Subject)?.Value;

            Guid userId;
            if (!Guid.TryParse(claimUserId, out userId))
            {
                throw new InvalidUserIdException();
            }

            return userId;
        }
    }
}
