using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Ticket_Hub.Utility.ValidationAttribute;

namespace Ticket_Hub.Models.DTO.Website;

public class UploadAppLogoDto
{
    [Required]
    [MaxFileSize(10)]
    [AllowedExtensions(new string[] { ".img", ".png", ".jpg" })]
    public List<IFormFile> File { get; set; }
}