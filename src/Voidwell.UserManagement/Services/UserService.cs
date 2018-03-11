using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Voidwell.UserManagement.Data.Models;
using Voidwell.UserManagement.Models;
using System.Linq;
using Voidwell.UserManagement.Exceptions;
using System.Threading;

namespace Voidwell.UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, ISecurityQuestionService securityQuestionService,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _securityQuestionService = securityQuestionService;
            _logger = logger;
        }

        public async Task<IEnumerable<SimpleUser>> GetAllUsers()
        {
            var users =  await _userManager.Users.ToListAsync();
            return users.Select(a => new SimpleUser {
                Id = a.Id,
                UserName = a.UserName,
                LastLoginDate = a.LastLoginDate
            });
        }

        public async Task<ApplicationUser> CreateUser(string displayName, string email, string password)
        {
            var newUser = new ApplicationUser
            {
                UserName = displayName,
                Email = email,
                LastLoginDate = DateTimeOffset.UtcNow,
                PasswordSetDate = DateTimeOffset.UtcNow,
                CreatedDate = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, password);

            await _userManager.AddToRoleAsync(newUser, UserRole.User.ToString());

            return newUser;
        }

        public Task<ApplicationUser> GetUser(Guid userId)
        {
            return _userManager.Users.SingleOrDefaultAsync(a => a.Id == userId);
        }

        public Task<ApplicationUser> GetUserByEmail(string email)
        {
            return _userManager.Users.SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task<ApplicationUser> UpdateUser(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
            return user;
        }

        public async Task DeleteUser(Guid userId)
        {
            var user = await GetUser(userId);
            if (user == null)
                return;

            await _userManager.DeleteAsync(user);
            await _securityQuestionService.RemoveSecurityQuestions(userId);
        }

        public async Task<IEnumerable<string>> GetRoles(Guid userId)
        {
            var user = await GetUser(userId);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<string>> AddRole(Guid userId, string role)
        {
            var user = await GetUser(userId);
            await _userManager.AddToRoleAsync(user, role);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task RemoveRole(Guid userId, string role)
        {
            var user = await GetUser(userId);
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> UpdateRoles(Guid userId, IEnumerable<string> roles)
        {
            var user = await GetUser(userId);
            if (user == null) {
                return null;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (Enumerable.SequenceEqual(roles.OrderBy(a => a), currentRoles.OrderBy(a => a)))
            {
                return roles;
            }

            var rolesToAdd = roles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(roles).ToList();

            if (rolesToAdd.Union(rolesToRemove).Contains(UserRole.SuperAdmin.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidRoleRequestException("Users cannot modify SuperAdmin!");
            }

            if (rolesToRemove.Count() > 0)
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Count() > 0)
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            return roles;
        }

        public async Task<UserDetails> GetUserDetails(Guid userId)
        {
            var user = await GetUser(userId);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetails
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                TimeZone = user.TimeZone,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                PasswordSetDate = user.PasswordSetDate,
                LockoutEndDate = user.LockoutEnd,
                Roles = roles
            };
        }

        public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await GetUser(userId);

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (result.Errors.Any())
            {
                foreach(var error in result.Errors)
                {
                    _logger.LogInformation($"Failed to change password for user {userId}. {error.Code}: {error.Description}");

                    if (error.Code == "PasswordMismatch")
                    {
                        throw new InvalidPasswordException();
                    }
                }
            }

            user.PasswordSetDate = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation($"Changed password for user {userId}");
        }

        public async Task<string> GetPasswordResetToken(Guid userId)
        {
            var user = await GetUser(userId);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPassword(Guid userId, string token, string password)
        {
            var user = await GetUser(userId);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            user.PasswordSetDate = DateTimeOffset.UtcNow;
            await UpdateUser(user);

            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
            }
        }

        public async Task<DisplayName> GetDisplayName(Guid userId)
        {
            var user = await GetUser(userId);
            return new DisplayName
            {
                UserId = userId,
                Name = user.UserName
            };
        }

        public async Task<IEnumerable<DisplayName>> GetDisplayNames(IEnumerable<Guid> userIds)
        {
            var users = await _userManager.Users
                    .Where(a => userIds.Contains(a.Id))
                    .AsNoTracking()
                    .ToListAsync();

            return users.Select(a =>
            {
                return new DisplayName
                {
                    UserId = a.Id,
                    Name = a.UserName
                };
            });
        }

        public async Task LockUser(Guid userId, int? minutes, bool? permanant)
        {
            var user = await GetUser(userId);

            await _userManager.SetLockoutEnabledAsync(user, true);

            if (permanant != null && permanant.Value)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }
            else
            {
                var lockMinutes = minutes != null ? minutes.Value : 30;
                var lockSpan = DateTimeOffset.UtcNow.AddMinutes(lockMinutes);
                await _userManager.SetLockoutEndDateAsync(user, lockSpan);
            }
        }

        public async Task UnlockUser(Guid userId)
        {
            var user = await GetUser(userId);

            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
        }
    }
}
