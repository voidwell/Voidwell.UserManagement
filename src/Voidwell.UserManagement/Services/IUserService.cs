using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voidwell.UserManagement.Data.Models;
using Voidwell.UserManagement.Models;

namespace Voidwell.UserManagement.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> CreateUser(string displayName, string email, string password);
        Task<ApplicationUser> GetUser(Guid userId);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> UpdateUser(ApplicationUser user);
        Task<IEnumerable<string>> GetRoles(Guid userId);
        Task<IEnumerable<SimpleUser>> GetAllUsers();
        Task<IEnumerable<string>> AddRole(Guid userId, string role);
        Task RemoveRole(Guid userId, string role);
        Task DeleteUser(Guid userId);
        Task<UserDetails> GetUserDetails(Guid userId);
        Task ChangePassword(Guid userId, string oldPassword, string newPassword);
        Task<string> GetPasswordResetToken(Guid userId);
        Task ResetPassword(Guid userId, string token, string newPassword);
        Task<DisplayName> GetDisplayName(Guid userId);
        Task<IEnumerable<DisplayName>> GetDisplayNames(IEnumerable<Guid> userIds);
        Task<IEnumerable<string>> UpdateRoles(Guid userId, IEnumerable<string> roles);
        Task LockUser(Guid userId, int? minutes = 30, bool? permanant = false);
        Task UnlockUser(Guid userId);
    }
}
