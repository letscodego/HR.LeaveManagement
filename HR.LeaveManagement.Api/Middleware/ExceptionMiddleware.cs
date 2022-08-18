using HR.LeaveManagement.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HR.LeaveManagement.Api.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    public ExceptionMiddleware(RequestDelegate request)
    {
        _requestDelegate = request;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {
            await HandelExceptionAsync(context, ex);
        }
    }

    private Task HandelExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        string result = JsonConvert.SerializeObject(
            new ErrorDetails
            {
                ErrorMessage = ex.Message,
                ErrorType = "Failure"
            });
        switch (ex)
        {
            case BadRequestException httpRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonConvert.SerializeObject(validationException.Errors);
                break;
            case NotFoundException notFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            default:
                break;
        }
        context.Response.StatusCode = (int) httpStatusCode;
        return context.Response.WriteAsync(result);
    }
}
