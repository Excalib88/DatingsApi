using Datings.Api.Common.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Datings.Api.Middlewares;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var error = "Internal server error";

        var statusCode = true switch
        {
            { } when exception is UnauthorizedException => 401,
            _ => 400
        };

        context.Result = new JsonResult(new {error}) { StatusCode = statusCode };
    }
}