using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Voidwell.UserManagement.Models
{
    public class AuthenticationResult
    {
        public Guid? UserId { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }
}
