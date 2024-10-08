namespace Ticket_Hub.Models.DTO.Auth;

public class ResetPasswordDto
{

    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;

}