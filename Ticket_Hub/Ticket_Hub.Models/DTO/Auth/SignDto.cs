using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Auth;

public class SignDto
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}