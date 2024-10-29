using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;

namespace Ticket_Hub.Services.IServices;

public interface IAppLogoService
{
    Task<ResponseDto> GetAppLogos();
    Task<ResponseDto> GetAppLogo(Guid appLogoId);
    Task<ResponseDto> CreateAppLogo(CreateAppLogoDto createAppLogoDto);
    Task<ResponseDto> UpdateAppLogo(UpdateAppLogoDto updateAppLogoDto);
    Task<ResponseDto> DeleteAppLogo(Guid appLogoId);
    Task<ResponseDto> UploadAppLogoImage(Guid appLogoId, string folder, UploadAppLogoDto uploadMobileAppLogo);
    Task<ResponseDto> GetAppLogoImage(Guid appLogoId);
    Task<ResponseDto> DeleteAppLogoImage(Guid appLogoId);
}