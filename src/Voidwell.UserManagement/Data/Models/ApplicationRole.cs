using Microsoft.AspNetCore.Identity;
using System;

namespace Voidwell.UserManagement.Data.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name)
        {
            Name = name;
        }
    }
}
