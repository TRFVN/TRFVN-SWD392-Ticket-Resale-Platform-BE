using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class FirebaseService : IFirebaseService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName = "tickethub-af919.appspot.com";

    public FirebaseService(StorageClient storageClient)
    {
        _storageClient = storageClient;
    }

    public async Task<ResponseDto> UploadImage(IFormFile file, string folder)
    {
        if (file is null || file.Length == 0)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = 400,
                Message = "File is empty!"
            };
        }

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";

        var filePath = $"{folder}/{fileName}";

        string url;

        await using (var stream = file.OpenReadStream())
        {
            var result = await _storageClient.UploadObjectAsync(_bucketName, filePath, null, stream);
        }

        return new ResponseDto()
        {
            IsSuccess = true,
            StatusCode = 200,
            Result = filePath,
            Message = "Upload image successfully!"
        };
    }


    public async Task<MemoryStream> GetImage(string filePath)
    {
        MemoryStream memoryStream = new MemoryStream();

        await _storageClient.DownloadObjectAsync(_bucketName, filePath, memoryStream);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }
}