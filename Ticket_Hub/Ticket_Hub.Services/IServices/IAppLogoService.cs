using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;

namespace Ticket_Hub.Services.IServices;

public interface IAppLogoService
{
    Task<ResponseDto> UploadAppLogo(Guid appLogoId, string folder, UploadAppLogoDto uploadMobileAppLogo);
    Task<ResponseDto> GetAppLogo(Guid appLogoId);
    Task<ResponseDto> DeleteAppLogo(Guid appLogoId);
}