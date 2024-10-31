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

    public async Task<ResponseDto> GetCategories
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    )
    {
        IEnumerable<Category> allCateogories = null!;

        // Lấy tất cả các sự kiện có trong database
        allCateogories = await _unitOfWork.CategoryRepository.GetAllAsync();

        // Kiểm tra nếu danh sách events là null hoặc rỗng
        if (!allCateogories.Any())
        {
            return new ResponseDto()
            {
                Message = "There are no Category",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var listCateogories = allCateogories.ToList();

        // Filter Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            switch (filterOn.Trim().ToLower())
            {
                case "cateogryname":
                    listCateogories = listCateogories.Where(x =>
                        x.CategoryName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                default:
                    break;
            }
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            var sortParams = sortBy.Trim().ToLower().Split('_'); // Chia chuỗi sortBy theo ký tự '_'
            var sortField = sortParams[0]; // Tên cột cần sắp xếp
            var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc"; // Lấy hướng sắp xếp

            switch (sortField)
            {
                case "cateogryname":
                    listCateogories = sortDirection == "desc"
                        ? listCateogories.OrderByDescending(x => x.CategoryName).ToList()
                        : listCateogories.OrderBy(x => x.CategoryName).ToList();
                    break;
                default:
                    // Sắp xếp mặc định theo ngày gần nhất nếu không có cột phù hợp
                    listCateogories = listCateogories.OrderBy(x => x.CategoryName).ToList();
                    break;
            }
        }
        else
        {
            // Sắp xếp mặc định theo ngày gần nhất nếu không có `sortBy`
            listCateogories = listCateogories.OrderBy(x => x.CategoryName).ToList();
        }

        // Phân trang
        if (pageNumber > 0 && pageSize > 0)
        {
            var skipResult = (pageNumber - 1) * pageSize;
            listCateogories = listCateogories.Skip(skipResult).Take(pageSize).ToList();
        }

        // Chuyển đổi danh sách sự kiện thành DTO
        var categoryDto = listCateogories.Select(categoryItem => new GetCategoryDto()
        {
            CategoryId = categoryItem.CategoryId,
            CategoryName = categoryItem.CategoryName,
            CreatedBy = categoryItem.CreatedBy,
            CreatedTime = categoryItem.CreatedTime,
            UpdatedBy = categoryItem.UpdatedBy,
            UpdatedTime = categoryItem.UpdatedTime,
            Status = categoryItem.Status
            
        }).ToList();

        return new ResponseDto()
        {
            Message = "Get Categories successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = categoryDto
        };
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
            CreatedTime = DateTime.Now,
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
        var categoryId =
            await _unitOfWork.CategoryRepository.GetAsync(x => x.CategoryId == updateCategoryDto.CategoryId);
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