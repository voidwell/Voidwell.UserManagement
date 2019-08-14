using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Voidwell.UserManagement.Exceptions;

namespace Voidwell.UserManageement.Api.Filters
{
    public class InvalidSecurityQuestionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidSecurityQuestionException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = "A security question provided does not match with any permitted questions",
                    ContentType = "text/plain"
                };
            }
        }
    }
}
