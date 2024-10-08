namespace Ticket_Hub.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Tiếp tục với request nếu không có lỗi
            await _next(context);
        }
        catch (UnauthorizedAccessException)
        {
            // Trả về lỗi 401 nếu xảy ra UnauthorizedAccessException
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
        catch (Exception)
        {
            // Nếu là lỗi khác, trả về lỗi 500
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            // Log lỗi ra (nếu cần)
        }

        // Kiểm tra các mã lỗi 403 và 400
        if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            await context.Response.WriteAsync("Forbidden");
        }
        else if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
            await context.Response.WriteAsync("Bad Request");
        }
    }
}