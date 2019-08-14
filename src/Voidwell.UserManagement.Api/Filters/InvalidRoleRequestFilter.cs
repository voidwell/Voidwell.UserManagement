using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Voidwell.UserManagement.Exceptions;

namespace Voidwell.UserManageement.Api.Filters
{
    public class InvalidRoleRequestFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidRoleRequestException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = context.Exception?.Message ?? "A modified role is invalid",
                    ContentType = "text/plain"
                };
            }
        }
    }
}
