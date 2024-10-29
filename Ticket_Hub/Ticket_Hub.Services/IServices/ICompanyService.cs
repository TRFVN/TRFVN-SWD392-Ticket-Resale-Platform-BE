using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Website;

namespace Ticket_Hub.Services.IServices;

public interface ICompanyService
{
    Task<ResponseDto> GetCompanies();
    Task<ResponseDto> GetCompany(Guid id);
    Task<ResponseDto> CreateCompany(CreateCompanyDto companyDto);
    Task<ResponseDto> UpdateCompany(UpdateCompanyDto companyDto);
    Task<ResponseDto> DeleteCompany(Guid id);
}