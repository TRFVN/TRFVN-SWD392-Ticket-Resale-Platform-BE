using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class PrivacyService : IPrivacyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PrivacyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetPrivacies()
    {
        try
        {
            var privacies = await _unitOfWork.PrivacyRepository.GetAllAsync();
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Get privacies successfully",
                StatusCode = 200,
                Result = privacies
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

    public async Task<ResponseDto> GetPrivacy(Guid id)
    {
        try
        {
            var privacy = await _unitOfWork.PrivacyRepository.GetAsync(p => p.Id == id);
            if (privacy == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Privacy policy not found",
                    StatusCode = 404
                };
            }

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Get privacy policy successfully",
                StatusCode = 200,
                Result = privacy
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

    public async Task<ResponseDto> CreatePrivacy(CreatePrivacyDto privacyDto)
    {
        try
        {
            var privacy = _mapper.Map<Privacy>(privacyDto);
            privacy.Id = Guid.NewGuid();
            privacy.LastUpdated = DateTime.Now;

            await _unitOfWork.PrivacyRepository.AddAsync(privacy);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Privacy policy created successfully",
                StatusCode = 200,
                Result = privacy
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

    public async Task<ResponseDto> UpdatePrivacy(UpdatePrivacyDto privacyDto)
    {
        try
        {
            var privacy = await _unitOfWork.PrivacyRepository.GetAsync(p => p.Id == privacyDto.Id);
            if (privacy == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Privacy policy not found",
                    StatusCode = 404
                };
            }

            _mapper.Map(privacyDto, privacy);
            privacy.LastUpdated = DateTime.Now;

            _unitOfWork.PrivacyRepository.Update(privacy);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Privacy policy updated successfully",
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

    public async Task<ResponseDto> DeletePrivacy(Guid id)
    {
        try
        {
            var privacy = await _unitOfWork.PrivacyRepository.GetAsync(p => p.Id == id);
            if (privacy == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Privacy policy not found",
                    StatusCode = 404
                };
            }

            privacy.IsActive = false;
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Privacy policy deleted successfully",
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
}