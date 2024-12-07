using RealTimeChatAPI.Exceptions;

namespace RealTimeChatAPI.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (UsernameAlreadyUsedException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (InvalidLoginException ex)
        {
            logger.LogWarning(ex.Message);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}
