using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;

namespace Ticket_Hub.Services.IServices;

public interface IPrivacyService
{
    Task<ResponseDto> GetPrivacies();
    Task<ResponseDto> GetPrivacy(Guid id);
    Task<ResponseDto> CreatePrivacy(CreatePrivacyDto privacyDto);
    Task<ResponseDto> UpdatePrivacy(UpdatePrivacyDto privacyDto);
    Task<ResponseDto> DeletePrivacy(Guid id);
}