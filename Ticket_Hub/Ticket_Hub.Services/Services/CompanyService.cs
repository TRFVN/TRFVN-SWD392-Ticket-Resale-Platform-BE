using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ResponseDto> GetCompanies()
    {
        try
        {
            var companies = await _unitOfWork.CompanyRepository.GetAllAsync();
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Get companies successfully",
                StatusCode = 200,
                Result = companies
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

    public async Task<ResponseDto> GetCompany(Guid id)
    {
        try
        {
            var company = await _unitOfWork.CompanyRepository.GetAsync(c => c.Id == id);
            if (company == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Company not found",
                    StatusCode = 404
                };
            }
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Get company successfully",
                StatusCode = 200,
                Result = company
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
    
    public async Task<ResponseDto> CreateCompany(CreateCompanyDto companyDto)
    {
        try
        {
            var company = _mapper.Map<Company>(companyDto);
            company.Id = Guid.NewGuid();
            company.Name = companyDto.Name;
            company.Address = companyDto.Address;
            company.City = companyDto.City;
            company.State = companyDto.State;
            company.Country = companyDto.Country;
            company.PostalCode = companyDto.PostalCode;
            company.Phone = companyDto.Phone;
            company.Email = companyDto.Email;
            company.Website = companyDto.Website;
            company.FoundedDate = companyDto.FoundedDate;
            company.LogoUrl = companyDto.LogoUrl;
            company.Description = companyDto.Description;

            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Company created successfully",
                StatusCode = 200,
                Result = company
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

    public async Task<ResponseDto> UpdateCompany(UpdateCompanyDto companyDto)
    {
        try
        {
            var company = await _unitOfWork.CompanyRepository.GetAsync(c => c.Id == companyDto.Id);
            if (company == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Company not found",
                    StatusCode = 404
                };
            }

            _mapper.Map(companyDto, company);
            _unitOfWork.CompanyRepository.Update(company);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Company updated successfully",
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
    
    public async Task<ResponseDto> DeleteCompany(Guid id)
    {
        try
        {
            var company = await _unitOfWork.CompanyRepository.GetAsync(p => p.Id == id);
            if (company == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Privacy policy not found",
                    StatusCode = 404
                };
            }

            _unitOfWork.CompanyRepository.Remove(company);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Company deleted successfully",
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