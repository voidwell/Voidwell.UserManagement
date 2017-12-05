using System.Threading.Tasks;
using Voidwell.UserManagement.Models;

namespace Voidwell.UserManagement.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Authenticate(AuthenticationRequest authRequest);
    }
}
