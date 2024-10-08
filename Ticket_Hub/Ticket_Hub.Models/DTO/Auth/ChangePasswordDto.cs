using System.ComponentModel.DataAnnotations;
using Ticket_Hub.Utility.ValidationAttribute;

namespace Ticket_Hub.Models.DTO.Auth;

public class ChangePasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    [Password]
    public string OldPassword { get; set; } = null!;
    
    [Required]
    [DataType(DataType.Password)]
    [Password]
    public string NewPassword { get; set; } = null!;
    
    [Required]
    [DataType(DataType.Password)]
    [ConfirmPassword("NewPassword")]
    public string ConfirmNewPassword { get; set; } = null!;
}