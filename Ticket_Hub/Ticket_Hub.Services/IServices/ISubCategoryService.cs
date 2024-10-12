using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Location;
using Ticket_Hub.Models.DTO.SubCategory;
using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Services.IServices;

public interface ISubCategoryService
{
    Task<ResponseDto> GetSubCategories
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetSubCategory(ClaimsPrincipal user, Guid subCategoryId);
    Task<ResponseDto> CreateSubCategory(ClaimsPrincipal user, CreateSubCategoryDto createSubCategoryDto);
    Task<ResponseDto> UpdateSubCategory(ClaimsPrincipal user, UpdateSubCategoryDto updateSubCategoryDto);
    Task<ResponseDto> DeleteSubCategory(ClaimsPrincipal user, Guid subCategoryId);
}