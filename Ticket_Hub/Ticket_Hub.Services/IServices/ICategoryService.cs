using System.Security.Claims;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Models.DTO.Location;

namespace Ticket_Hub.Services.IServices;

public interface ICategoryService
{
    Task<ResponseDto> GetCategories
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    );

    Task<ResponseDto> GetCategory(ClaimsPrincipal user, Guid categoryId);
    Task<ResponseDto> CreateCategory(ClaimsPrincipal user, CreateCategoryDto createCategoryDto);
    Task<ResponseDto> UpdateCategory(ClaimsPrincipal user, UpdateCategoryDto updateCategoryDto);
    Task<ResponseDto> DeleteCategory(ClaimsPrincipal user, Guid categoryId);
}