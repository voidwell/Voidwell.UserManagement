using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Voidwell.UserManagement.Exceptions;

namespace Voidwell.UserManagement.Filters
{
    public class InvalidPasswordFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidPasswordException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = "The password entered is incorrect",
                    ContentType = "text/plain"
                };
            }
        }
    }
}
