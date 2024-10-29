using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Utility.Constants;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/website")]
    [ApiController]
    [Authorize(Roles = StaticUserRoles.Admin)]
    public class WebsiteController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ITermOfUseService _termOfUseService;
        private readonly IPrivacyService _privacyService;
        private readonly IAppLogoService _appLogoService;

        public WebsiteController
        (
            ICompanyService companyService,
            ITermOfUseService termOfUseService,
            IPrivacyService privacyService,
            IAppLogoService appLogoService
        )
        {
            _companyService = companyService;
            _termOfUseService = termOfUseService;
            _privacyService = privacyService;
            _appLogoService = appLogoService;
        }

        #region Company

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet("company")]
        public async Task<ActionResult<ResponseDto>> GetCompanies()
        {
            var responseDto = await _companyService.GetCompanies();
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("company/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> GetCompany(Guid id)
        {
            var responseDto = await _companyService.GetCompany(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost("company")]
        public async Task<ActionResult<ResponseDto>> CreateCompany([FromBody] CreateCompanyDto companyDto)
        {
            var responseDto = await _companyService.CreateCompany(companyDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut("company")]
        public async Task<ActionResult<ResponseDto>> UpdateCompany([FromBody] UpdateCompanyDto companyDto)
        {
            var responseDto = await _companyService.UpdateCompany(companyDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("company/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> DeleteCompany(Guid id)
        {
            var responseDto = await _companyService.DeleteCompany(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        #endregion

        #region Privacy

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet("privacy")]
        public async Task<ActionResult<ResponseDto>> GetPrivacies()
        {
            var responseDto = await _privacyService.GetPrivacies();
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("privacy/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> GetPrivacy(Guid id)
        {
            var responseDto = await _privacyService.GetPrivacy(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("privacy")]
        public async Task<ActionResult<ResponseDto>> CreatePrivacy([FromBody] CreatePrivacyDto privacyDto)
        {
            var responseDto = await _privacyService.CreatePrivacy(privacyDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut("privacy")]
        public async Task<ActionResult<ResponseDto>> UpdatePrivacy([FromBody] UpdatePrivacyDto privacyDto)
        {
            var responseDto = await _privacyService.UpdatePrivacy(privacyDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("privacy/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> DeletePrivacy(Guid id)
        {
            var responseDto = await _privacyService.DeletePrivacy(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        #endregion

        #region TermOfUse

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet("term-of-use")]
        public async Task<ActionResult<ResponseDto>> GetTermOfUses()
        {
            var responseDto = await _termOfUseService.GetTermOfUses();
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("term-of-use/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> GetTermOfUse(Guid id)
        {
            var responseDto = await _termOfUseService.GetTermOfUse(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("term-of-use")]
        public async Task<ActionResult<ResponseDto>> CreateTermOfUse([FromBody] CreateTermOfUseDto termOfUseDto)
        {
            var responseDto = await _termOfUseService.CreateTermOfUse(termOfUseDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut("term-of-use")]
        public async Task<ActionResult<ResponseDto>> UpdateTermOfUse([FromBody] UpdateTermOfUseDto termOfUseDto)
        {
            var responseDto = await _termOfUseService.UpdateTermOfUse(termOfUseDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("term-of-use/{id:guid}")]
        public async Task<ActionResult<ResponseDto>> DeleteTermOfUse(Guid id)
        {
            var responseDto = await _termOfUseService.DeleteTermOfUse(id);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        #endregion
        
        #region MobileAppLogo
        
        [HttpPost]
        [Route("web-logo")]
        public async Task<ActionResult<ResponseDto>> UploadWebLogo(Guid appLogoId, UploadAppLogoDto uploadMobileAppLogo)
        {
            var responseDto = await _appLogoService.UploadAppLogo(appLogoId, StaticFirebaseFolders.WebLogo, uploadMobileAppLogo);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet]
        [Route("web-logo")]
        public async Task<ActionResult<ResponseDto>> GetWebLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.GetAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete]
        [Route("web-logo")]
        public async Task<ActionResult<ResponseDto>> DeleteWebLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.DeleteAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("app-store-logo")]
        public async Task<ActionResult<ResponseDto>> UploadAppStoreLogo(Guid appLogoId, UploadAppLogoDto uploadMobileAppLogo)
        {
            var responseDto = await _appLogoService.UploadAppLogo(appLogoId, StaticFirebaseFolders.AppStoreLogo, uploadMobileAppLogo);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet]
        [Route("app-store-logo")]
        public async Task<ActionResult<ResponseDto>> GetAppStoreLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.GetAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete]
        [Route("app-store-logo")]
        public async Task<ActionResult<ResponseDto>> DeleteAppStoreLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.DeleteAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("ch-play-logo")]
        public async Task<ActionResult<ResponseDto>> UploadCHPlayLogo(Guid appLogoId, UploadAppLogoDto uploadMobileAppLogo)
        {
            var responseDto = await _appLogoService.UploadAppLogo(appLogoId, StaticFirebaseFolders.ChPlayLogo, uploadMobileAppLogo);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet]
        [Route("ch-play-logo")]
        public async Task<ActionResult<ResponseDto>> GetCHPlayLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.GetAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete]
        [Route("ch-play-logo")]
        public async Task<ActionResult<ResponseDto>> DeleteCHPlayLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.DeleteAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpPost]
        [Route("qr-code-logo")]
        public async Task<ActionResult<ResponseDto>> UploadQRCodeLogo(Guid appLogoId, UploadAppLogoDto uploadMobileAppLogo)
        {
            var responseDto = await _appLogoService.UploadAppLogo(appLogoId, StaticFirebaseFolders.QrCodeLogo, uploadMobileAppLogo);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [Authorize(Roles = StaticUserRoles.Admin + "," + StaticUserRoles.Member + "," + StaticUserRoles.Staff)]
        [HttpGet]
        [Route("qr-code-logo")]
        public async Task<ActionResult<ResponseDto>> GetQRCodeLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.GetAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete]
        [Route("qr-code-logo")]
        public async Task<ActionResult<ResponseDto>> DeleteQRCodeLogo(Guid appLogoId)
        {
            var responseDto = await _appLogoService.DeleteAppLogo(appLogoId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        #endregion
    }
}