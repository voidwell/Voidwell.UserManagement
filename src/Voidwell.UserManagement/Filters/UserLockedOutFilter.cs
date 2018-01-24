using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Voidwell.UserManagement.Exceptions;

namespace Voidwell.UserManagement.Filters
{
    public class UserLockedOutFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UserLockedOutException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Content = "Account has been locked. Please try again later.",
                    ContentType = "text/plain"
                };
            }
        }
    }
}
