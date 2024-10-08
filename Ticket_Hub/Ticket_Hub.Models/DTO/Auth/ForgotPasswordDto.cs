using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Auth;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Please enter email or phone number.")]
    public string EmailOrPhone { get; set; } = null!;
}