using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voidwell.UserManagement.Models;
using Voidwell.UserManagement.Data.Models;

namespace Voidwell.UserManagement.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _userService;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly ILogger _logger;

        public RegistrationService(IUserService userService, ISecurityQuestionService securityQuestionService, ILogger<RegistrationService> logger)
        {
            _userService = userService;
            _securityQuestionService = securityQuestionService;
            _logger = logger;
        }

        public async Task<ApplicationUser> RegisterNewUserAsync(RegistrationForm registration)
        {
            _securityQuestionService.ValidateSecurityQuestions(registration.SecurityQuestions);

            var user = await _userService.CreateUser(registration.Username, registration.Email, registration.Password);

            _logger.LogInformation($"Registered new user: {user.Id}");

            await _securityQuestionService.CreateSecurityQuestions(user.Id, registration.SecurityQuestions);

            return user;
        }
    }
}
