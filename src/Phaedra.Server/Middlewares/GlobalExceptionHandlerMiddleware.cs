
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
namespace Phaedra.Server.Middlewares;
public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    private readonly IDictionary<Type, HttpStatusCode> _statusCodeMap = new Dictionary<Type, HttpStatusCode>
    {
        {typeof(ArgumentNullException), HttpStatusCode.BadRequest},
        {typeof(ArgumentException), HttpStatusCode.BadRequest},
        {typeof(FormatException), HttpStatusCode.BadRequest},
        {typeof(InvalidOperationException), HttpStatusCode.BadRequest},
        {typeof(ValidationException), HttpStatusCode.BadRequest},
        {typeof(KeyNotFoundException), HttpStatusCode.NotFound}
    };

    public async Task InvokeAsync(HttpContext context) 
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) 
        {
            logger.LogError(ex, "A exception was thrown.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (_statusCodeMap.TryGetValue(ex.GetType(), out var statusCode))
        {
            context.Response.StatusCode = (int)statusCode;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = ex.Message }));
    }
}

