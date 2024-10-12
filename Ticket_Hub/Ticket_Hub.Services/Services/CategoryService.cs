using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public Task<ResponseDto> GetCategories
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    )
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> GetCategory(ClaimsPrincipal user, Guid categoryId)
    {
        var category = await _unitOfWork.CategoryRepository.GetById(categoryId);
        if (category == null)
        {
            return new ResponseDto
            {
                Message = "Category not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var categoryDto = _mapper.Map<GetCategoryDto>(category);

        return new ResponseDto
        {
            Message = "Location found successfully",
            Result = categoryDto,
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> CreateCategory(ClaimsPrincipal user, CreateCategoryDto createCategoryDto)
    {
        Category newCategory = new Category()
        {
            CategoryName = createCategoryDto.CategoryName,
            CreatedBy = user.Identity.Name,
            UpdatedBy = "",
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = null,
            Status = 1,
        };

        await _unitOfWork.CategoryRepository.AddAsync(newCategory);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Category created successfully",
            Result = newCategory,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateCategory(ClaimsPrincipal user, UpdateCategoryDto updateCategoryDto)
    {
        var categoryId = await _unitOfWork.CategoryRepository.GetAsync(x => x.CategoryId == updateCategoryDto.CategoryId);
        if (categoryId == null)
        {
            return new ResponseDto
            {
                Message = "Category not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        //update Category
        categoryId.CategoryName = updateCategoryDto.CategoryName;
        categoryId.UpdatedBy = user.Identity.Name;
        categoryId.UpdatedTime = DateTime.Now;


        //save changes
        _unitOfWork.CategoryRepository.Update(categoryId);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Category updated successfully",
            Result = categoryId,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> DeleteCategory(ClaimsPrincipal user, Guid categoryId)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(x => x.CategoryId == categoryId);
        if (category == null)
        {
            return new ResponseDto
            {
                Message = "Category not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        category.Status = 0;
        category.UpdatedBy = user.Identity.Name;
        category.UpdatedTime = DateTime.UtcNow;

        //save changes
        _unitOfWork.CategoryRepository.Update(category);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Category delete successfully",
            Result = category,
            IsSuccess = true,
            StatusCode = 201
        };
    }
}