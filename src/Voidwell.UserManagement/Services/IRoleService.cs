using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voidwell.UserManagement.Models;

namespace Voidwell.UserManagement.Services
{
    public interface IRoleService
    {
        Task<SimpleRole> CreateRole(string role);
        Task<IEnumerable<SimpleRole>> GetAllRoles();
        Task DeleteRole(Guid roleId);
    }
}
