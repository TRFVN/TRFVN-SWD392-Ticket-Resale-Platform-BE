using Ticket_Hub.Models.DTO;

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
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var response = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Unauthorized"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            // Nếu là lỗi khác, trả về lỗi 500 với thông báo lỗi chi tiết
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = ex.Message // Hoặc bạn có thể thêm thông tin khác từ exception nếu cần
            };
            await context.Response.WriteAsJsonAsync(response);
            // Log lỗi nếu cần
        }

        // Kiểm tra các mã lỗi 403 và 400
        if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            var response = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status403Forbidden,
                Message = "Forbidden"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        else if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
            var response = new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Bad Request"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}