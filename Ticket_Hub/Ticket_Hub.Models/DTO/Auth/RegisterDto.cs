using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticket_Hub.Utility.ValidationAttribute;

namespace Ticket_Hub.Models.DTO.Auth;

public class RegisterDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Password]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [ConfirmPassword("Password")]
    [NotMapped]
    public string ConfirmPassword { get; set; } = null!;
    
    [Required]
    [Cccd]
    public string Cccd { get; set; } = null!;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime BirthDate { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
}