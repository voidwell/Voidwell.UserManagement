using System;
using System.Collections.Generic;

namespace Voidwell.UserManagement.Models
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public DateTimeOffset? PasswordSetDate { get; set; }
        public DateTimeOffset? LockoutEndDate { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
