using System.Threading.Tasks;
using Voidwell.UserManagement.Models;
using Voidwell.UserManagement.Data.Models;

namespace Voidwell.UserManagement.Services
{
    public interface IRegistrationService
    {
        Task<ApplicationUser> RegisterNewUserAsync(RegistrationForm registrationForm);
    }
}
