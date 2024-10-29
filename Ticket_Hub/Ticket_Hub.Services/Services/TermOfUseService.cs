using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class TermOfUseService : ITermOfUseService
{
    private readonly IUnitOfWork _unitOfWork;
private readonly IMapper _mapper;

public TermOfUseService(IUnitOfWork unitOfWork, IMapper mapper)
{
    _unitOfWork = unitOfWork;
    _mapper = mapper;
}

public async Task<ResponseDto> GetTermOfUses()
{
    try
    {
        var termOfUses = await _unitOfWork.TermOfUseRepository.GetAllAsync();
        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Get terms of use successfully",
            StatusCode = 200,
            Result = termOfUses
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

public async Task<ResponseDto> GetTermOfUse(Guid id)
{
    try
    {
        var termOfUse = await _unitOfWork.TermOfUseRepository.GetAsync(t => t.Id == id);
        if (termOfUse == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Term of use not found",
                StatusCode = 404
            };
        }
        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Get term of use successfully",
            StatusCode = 200,
            Result = termOfUse
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

public async Task<ResponseDto> CreateTermOfUse(CreateTermOfUseDto termOfUseDto)
{
    try
    {
        var termOfUse = _mapper.Map<TermOfUse>(termOfUseDto);
        termOfUse.Id = Guid.NewGuid();
        termOfUse.LastUpdated = DateTime.Now; // Set last updated time

        await _unitOfWork.TermOfUseRepository.AddAsync(termOfUse);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Term of use created successfully",
            StatusCode = 201,
            Result = termOfUse.Id // Return the ID of the newly created term of use
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

public async Task<ResponseDto> UpdateTermOfUse(UpdateTermOfUseDto termOfUseDto)
{
    try
    {
        var termOfUse = await _unitOfWork.TermOfUseRepository.GetAsync(t => t.Id == termOfUseDto.Id);
        if (termOfUse == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Term of use not found",
                StatusCode = 404
            };
        }

        _mapper.Map(termOfUseDto, termOfUse);
        termOfUse.LastUpdated = DateTime.Now; // Update last updated time

        _unitOfWork.TermOfUseRepository.Update(termOfUse);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Term of use updated successfully",
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

public async Task<ResponseDto> DeleteTermOfUse(Guid id)
{
    try
    {
        var termOfUse = await _unitOfWork.TermOfUseRepository.GetAsync(t => t.Id == id);
        if (termOfUse == null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Term of use not found",
                StatusCode = 404
            };
        }

        termOfUse.IsActive = false;
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Term of use deleted successfully",
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