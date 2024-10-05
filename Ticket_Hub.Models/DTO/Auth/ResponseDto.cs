namespace Ticket_Hub.Models.DTO;

public class ResponseDto
{
    public object? Result { get; set; }
    public bool IsSuccess { get; set; } = true;
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = "";
}