namespace Ticket_Hub.Models.DTO.Auth;

public class SignResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}