using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Voidwell.UserManagement.Services;
using Voidwell.UserManagement.Models;

namespace Voidwell.UserManagement.Controllers
{
    [Route("password")]
    public class PasswordController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserHelper _userHelper;

        public PasswordController(IUserService userService, IUserHelper userHelper)
        {
            _userService = userService;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<ActionResult> PostChangePassword([FromBody]PasswordChangeRequest changeRequest)
        {
            var userId = _userHelper.GetUserIdFromContext();

            await _userService.ChangePassword(userId, changeRequest.OldPassword, changeRequest.NewPassword);

            return Ok();
        }
    }
}
