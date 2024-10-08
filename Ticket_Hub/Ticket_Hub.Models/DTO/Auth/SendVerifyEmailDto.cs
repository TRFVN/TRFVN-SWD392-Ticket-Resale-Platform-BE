using System.ComponentModel.DataAnnotations;

namespace Ticket_Hub.Models.DTO.Auth;

public class SendVerifyEmailDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
}