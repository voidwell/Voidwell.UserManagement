using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Voidwell.UserManagement.Services;
using Voidwell.UserManagement.Models;
using System.Collections.Generic;

namespace Voidwell.UserManagement.Controllers
{
    [Route("roles")]
    public class RolesController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserHelper _userHelper;
        private readonly ILogger _logger;

        public RolesController(IUserService userService, IRoleService roleService, IUserHelper userHelper, ILogger<RolesController> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _userHelper = userHelper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetRoles()
        {
            var userId = _userHelper.GetUserIdFromContext();

            var roles = await _userService.GetRoles(userId);
            if (roles == null)
                roles = new string[0];

            return Ok(roles);
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult> GetRolesForUser(Guid userId)
        {
            var roles = await _userService.GetRoles(userId);
            if (roles == null)
                roles = new string[0];

            return Ok(roles);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody]RoleRequest roleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _roleService.CreateRole(roleRequest.Name);
            return Created("account/roles", role);
        }

        [HttpDelete("{roleId:guid}")]
        public async Task<ActionResult> DeleteRole(Guid roleId)
        {
            await _roleService.DeleteRole(roleId);
            return NoContent();
        }
    }
}
