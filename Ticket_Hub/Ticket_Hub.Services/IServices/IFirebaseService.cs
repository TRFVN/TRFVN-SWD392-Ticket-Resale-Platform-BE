using Microsoft.AspNetCore.Http;
using Ticket_Hub.Models.DTO;

namespace Ticket_Hub.Services.IServices;

public interface IFirebaseService
{
    Task<ResponseDto> UploadImage(IFormFile file, string folder);
    Task<MemoryStream> GetImage(string filePath);
}