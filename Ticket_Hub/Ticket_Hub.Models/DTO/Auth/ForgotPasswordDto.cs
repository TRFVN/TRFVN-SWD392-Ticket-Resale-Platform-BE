using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Auth;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Please enter email. ")]
    public string Email { get; set; } = null!;
}