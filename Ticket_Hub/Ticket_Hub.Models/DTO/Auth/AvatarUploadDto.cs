using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Ticket_Hub.Utility.ValidationAttribute;

namespace Ticket_Hub.Models.DTO.Auth;

public class AvatarUploadDto
{
    [Required]
    [MaxFileSize(1)]
    [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
    public IFormFile File { get; set; }
}