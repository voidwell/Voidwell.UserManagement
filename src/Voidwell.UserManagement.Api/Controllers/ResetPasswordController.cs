using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Voidwell.UserManagement.Models;
using Voidwell.UserManagement.Services;
using System.Linq;

namespace Voidwell.UserManagement.Controllers
{
    [Route("resetpassword")]
    public class ResetPasswordController : Controller
    {
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly IUserService _userService;

        public ResetPasswordController(ISecurityQuestionService securityQuestionService, IUserService userService)
        {
            _securityQuestionService = securityQuestionService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> PostReset([FromBody]ResetPasswordRequest request)
        {
            var user = await _userService.GetUserByEmail(request.Email);
            await _userService.ResetPassword(user.Id, request.Token, request.NewPassword);

            return Ok();
        }

        [HttpPost("start")]
        public async Task<ActionResult> PostResetStart([FromBody]ResetPasswordStart model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questions = await _securityQuestionService.GetSecurityQuestionsByEmail(model.Email);
            if (questions == null)
                return NotFound("No user was found with this email.");
            
            return Ok(questions.Select(a => a.Question));
        }

        [HttpPost("questions")]
        public async Task<ActionResult> PostResetQuestions([FromBody]ResetPasswordQuestions model)
        {
            var questions = await _securityQuestionService.GetSecurityQuestionsByEmail(model.Email);
            
            foreach(var storedQuestion in questions)
            {
                var match = model.Questions.Any(a => a.Question == storedQuestion.Question && a.Answer.ToLower() == storedQuestion.Answer.ToLower());
                if (!match)
                {
                    return BadRequest("One or more answers were invalid.");
                }
            }

            var user = await _userService.GetUserByEmail(model.Email);
            var resetToken = _userService.GetPasswordResetToken(user.Id);

            return Ok(resetToken);
        }
    }
}
