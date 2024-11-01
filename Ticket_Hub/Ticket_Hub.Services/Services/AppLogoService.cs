﻿using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;
using Ticket_Hub.Utility.Constants;

namespace Ticket_Hub.Services.Services;

public class AppLogoService : IAppLogoService
{
    private readonly IFirebaseService _firebaseService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppLogoService(IFirebaseService firebaseService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _firebaseService = firebaseService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ResponseDto> GetAppLogos()
    {
        try
        {
            var appLogos = await _unitOfWork.AppLogoRepository.GetAllAsync();
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Get app logos successfully",
                StatusCode = 200,
                Result = appLogos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }

    public async Task<ResponseDto> GetAppLogo(Guid appLogoId)
    {
        try
        {
            var appLogo = await _unitOfWork.AppLogoRepository.GetAsync(c => c.AppLogoId == appLogoId);
            if (appLogo == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "App logo not found",
                    StatusCode = 404
                };
            }
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Get app logo successfully",
                StatusCode = 200,
                Result = appLogo
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }
    
    public async Task<ResponseDto> CreateAppLogo(CreateAppLogoDto appLogoDto)
    {
        try
        {
            var appLogo = _mapper.Map<AppLogo>(appLogoDto);
            appLogo.AppLogoId = Guid.NewGuid();
            appLogo.WebLogo = appLogoDto.WebLogo;
            appLogo.ChPlayLogo = appLogoDto.ChPlayLogo;
            appLogo.AppStoreLogo = appLogoDto.AppStoreLogo;
            appLogo.QrCodeLogo = appLogoDto.QrCodeLogo;

            await _unitOfWork.AppLogoRepository.AddAsync(appLogo);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "App logo created successfully",
                StatusCode = 200,
                Result = appLogo
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }

    public async Task<ResponseDto> UpdateAppLogo(UpdateAppLogoDto appLogoDto)
    {
        try
        {
            var appLogo = await _unitOfWork.AppLogoRepository.GetAsync(c => c.AppLogoId == appLogoDto.AppLogoId);
            if (appLogo == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "App logo not found",
                    StatusCode = 404
                };
            }

            _mapper.Map(appLogoDto, appLogo);
            _unitOfWork.AppLogoRepository.Update(appLogo);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "App logo updated successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }
    
    public async Task<ResponseDto> DeleteAppLogo(Guid appLogoId)
    {
        try
        {
            var appLogo = await _unitOfWork.AppLogoRepository.GetAsync(p => p.AppLogoId == appLogoId);
            if (appLogo == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "App logo not found",
                    StatusCode = 404
                };
            }

            _unitOfWork.AppLogoRepository.Remove(appLogo);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "App logo deleted successfully",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }

    public async Task<ResponseDto> UploadAppLogoImage(Guid appLogoId, string folder, UploadAppLogoDto uploadMobileAppLogo)
    {
        try
        {
            if (uploadMobileAppLogo.File == null)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "No file uploaded."
                };
            }
            
            var appLogo = await _unitOfWork.AppLogoRepository.GetAsync(x => x.AppLogoId == appLogoId);
            if (appLogo == null)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "App logo not found."
                };
            }

            var responseDtoList = new List<string>();
            foreach (var image in uploadMobileAppLogo.File)
            {
                var filePath = $"{folder}";
                var responseDto = await _firebaseService.UploadImageChPlay(image, filePath);
                responseDtoList.Add(responseDto.Result.ToString());
            }

            switch (folder)
            {
                case StaticFirebaseFolders.WebLogo:
                    appLogo.WebLogo = responseDtoList.Any(x => x != null) ? responseDtoList.ToArray() : Array.Empty<string>();
                    break;
                case StaticFirebaseFolders.ChPlayLogo:
                    appLogo.ChPlayLogo = responseDtoList.Any(x => x != null) ? responseDtoList.ToArray() : Array.Empty<string>();
                    break;
                case StaticFirebaseFolders.AppStoreLogo:
                    appLogo.AppStoreLogo = responseDtoList.Any(x => x != null) ? responseDtoList.ToArray() : Array.Empty<string>();
                    break;
                case StaticFirebaseFolders.QrCodeLogo:
                    appLogo.QrCodeLogo = responseDtoList.Any(x => x != null) ? responseDtoList.ToArray() : Array.Empty<string>();
                    break;
                default:
                    throw new ArgumentException("Invalid folder name", nameof(folder));
            }

            _unitOfWork.AppLogoRepository.Update(appLogo);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Result = responseDtoList,
                Message = "Upload file successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = 500,
                Result = null,
                Message = e.Message
            };
        }
    }

    public async Task<ResponseDto> GetAppLogoImage(Guid appLogoId)
    {
        try
        {
            var appLogo = await _unitOfWork.AppLogoRepository.GetAsync(x => x.AppLogoId == appLogoId);

            if (appLogo != null && appLogo.ChPlayLogo.IsNullOrEmpty())
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null,
                    Message = "No background image found"
                };
            }

            return new ResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Result = appLogo.ChPlayLogo,
                Message = "Get background images successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = 500,
                Result = null,
                Message = "Internal server error"
            };
        }   
    }

    public async Task<ResponseDto> DeleteAppLogoImage(Guid appLogoId)
    {
        try
        {
            var appLogo = await _unitOfWork.AppLogoRepository.GetAsync(x => x.AppLogoId == appLogoId);

            if (appLogo == null)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null,
                    Message = "App logo not found"
                };
            }

            var responseDtoList = new List<string>();
            foreach (var imageUrl in appLogo.ChPlayLogo)
            {
                var responseDto = await _firebaseService.DeleteImage(imageUrl);
                responseDtoList.Add(responseDto.Message);
            }

            appLogo.ChPlayLogo = Array.Empty<string>();
            _unitOfWork.AppLogoRepository.Update(appLogo);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Result = responseDtoList,
                Message = "Delete app logo images successfully"
            };
        }
        catch (Exception e)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = 500,
                Result = null,
                Message = "Internal server error"
            };
        }
    }
}