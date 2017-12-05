using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Voidwell.UserManagement.Exceptions;

namespace Voidwell.UserManagement.Filters
{
    public class InvalidUserIdFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidUserIdException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = "User Id in sub claim is invalid",
                    ContentType = "text/plain"
                };
            }
        }
    }
}
