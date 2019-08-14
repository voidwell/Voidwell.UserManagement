using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Voidwell.UserManagement.Exceptions;
using Voidwell.UserManagement.Models;
using Voidwell.UserManagement.Services;

namespace Voidwell.UserManagement.Controllers
{
    [Route("authenticate")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult> PostAuthenticate([FromBody]AuthenticationRequest authRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var authResults = await _authenticationService.Authenticate(authRequest);
                return Ok(authResults);
            }
            catch(InvalidPasswordException)
            {
                return BadRequest("Password is invalid");
            }
            catch(UserNotFoundException)
            {
                return NotFound("User with that username does not exist");
            }
        }
    }
}
