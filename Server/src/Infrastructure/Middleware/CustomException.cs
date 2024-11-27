namespace WebShop.src.Infrastructure.Middleware;

public class CustomException(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = new {
            error = ex.Message,
            stackTrace = ex.StackTrace
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}