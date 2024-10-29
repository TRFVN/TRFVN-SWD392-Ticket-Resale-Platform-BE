using Microsoft.AspNetCore.Http;
using Ticket_Hub.Models.DTO;

namespace Ticket_Hub.Services.IServices;

public interface IFirebaseService
{
    Task<ResponseDto> UploadImageTicket(IFormFile file, string folder);
    Task<ResponseDto> UploadImageUser(IFormFile file, string folder);
    Task<ResponseDto> UploadImageChPlay(IFormFile file, string folder);
    Task<ResponseDto> DeleteImage(string filePath);
    Task<MemoryStream> GetImage(string filePath);
}