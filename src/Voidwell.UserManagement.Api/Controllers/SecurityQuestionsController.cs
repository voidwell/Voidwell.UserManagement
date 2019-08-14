using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Voidwell.UserManagement.Services;

namespace Voidwell.UserManagement.Controllers
{
    [Route("questions")]
    public class SecurityQuestionsController : Controller
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionsController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet]
        public IEnumerable<string> GetSecurityQuestions()
        {
            return _securityQuestionService.GetSecurityQuestionsList();
        }
    }
}
