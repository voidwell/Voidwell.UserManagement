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

        [HttpPost("names")]
        public async Task<ActionResult> GetDisplayNames([FromBody] BatchUserRequest request)
        {
            var displayNames = await _userService.GetDisplayNames(request.UserIds);
            return Ok(displayNames);
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

        [HttpPut("{userId:guid}/roles")]
        public async Task<ActionResult> AddRolesToUser(Guid userId, [FromBody]UserRolesRequest userRoles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roles = await _userService.UpdateRoles(userId, userRoles.Roles);
            if (roles == null)
            {
                return NotFound("User not found");
            }
            return Created("user/roles", roles);
        }

        [HttpPost("{userId:guid}/lock")]
        public async Task<ActionResult> PostLockUser(Guid userId, [FromBody] UserLockRequest request)
        {
            await _userService.LockUser(userId, request?.LockLength, request?.IsPermanant);
            return NoContent();
        }

        [HttpPost("{userId:guid}/unlock")]
        public async Task<ActionResult> PostUnkockUser(Guid userId)
        {
            await _userService.UnlockUser(userId);
            return NoContent();
        }
    }
}
