using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;

namespace Ticket_Hub.Services.IServices;

public interface ITermOfUseService
{
    Task<ResponseDto> GetTermOfUses();
    Task<ResponseDto> GetTermOfUse(Guid id);
    Task<ResponseDto> CreateTermOfUse(CreateTermOfUseDto termOfUseDto);
    Task<ResponseDto> UpdateTermOfUse(UpdateTermOfUseDto termOfUseDto);
    Task<ResponseDto> DeleteTermOfUse(Guid id);
}   