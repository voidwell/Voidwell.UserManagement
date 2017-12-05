using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Voidwell.UserManagement.Models;
using Voidwell.UserManagement.Services;

namespace Voidwell.UserManagement.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult> GetUser(Guid userId)
        {
            var details = await _userService.GetUserDetails(userId);
            return Ok(details);
        }

        [HttpGet("{userId:guid}/name")]
        public async Task<ActionResult> GetDisplayName(Guid userId)
        {
            var displayName = await _userService.GetDisplayName(userId);
            return Ok(displayName);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult> DeleteUser(Guid userId)
        {
            await _userService.DeleteUser(userId);
            return NoContent();
        }

        [HttpPost("byemail")]
        public async Task<ActionResult> GetUserByEmail([FromBody]EmailAddressRequest emailAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByEmail(emailAddress.EmailAddress);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("{userId:guid}/role/{roleName}")]
        public async Task<ActionResult> AddRoleToUser(Guid userId, string roleName)
        {
            var roles = await _userService.AddRole(userId, roleName);
            return Created("user/role", roles);
        }

        [HttpDelete("{userId:guid}/role/{roleName}")]
        public async Task<ActionResult> RemoveRole(Guid userId, string roleName)
        {
            await _userService.RemoveRole(userId, roleName);

            return NoContent();
        }
    }
}
