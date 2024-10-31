using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.SubCategory;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class SubCategoryService : ISubCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetSubCategories
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    )
    {
        IEnumerable<SubCategory> allSubCategories = null!;

        // Lấy tất cả các sự kiện có trong database
        allSubCategories = await _unitOfWork.SubCategoryRepository.GetAllAsync();

        // Kiểm tra nếu danh sách events là null hoặc rỗng
        if (!allSubCategories.Any())
        {
            return new ResponseDto()
            {
                Message = "There are no SubCategory",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            };
        }

        var listubCategories = allSubCategories.ToList();

        // Filter Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            switch (filterOn.Trim().ToLower())
            {
                case "subcategoryname":
                    listubCategories = listubCategories.Where(x =>
                        x.SubCategoryName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
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
                case "subcategoryname":
                    listubCategories = sortDirection == "desc"
                        ? listubCategories.OrderByDescending(x => x.SubCategoryName).ToList()
                        : listubCategories.OrderBy(x => x.SubCategoryName).ToList();
                    break;
                default:
                    // Sắp xếp mặc định theo ngày gần nhất nếu không có cột phù hợp
                    listubCategories = listubCategories.OrderBy(x => x.SubCategoryName).ToList();
                    break;
            }
        }
        else
        {
            // Sắp xếp mặc định theo ngày gần nhất nếu không có `sortBy`
            listubCategories = listubCategories.OrderBy(x => x.SubCategoryName).ToList();
        }

        // Phân trang
        if (pageNumber > 0 && pageSize > 0)
        {
            var skipResult = (pageNumber - 1) * pageSize;
            listubCategories = listubCategories.Skip(skipResult).Take(pageSize).ToList();
        }

        // Chuyển đổi danh sách sự kiện thành DTO
        var eventDto = listubCategories.Select(subCategoryItem => new GetSubCategoryDto()
        {
            SubCategoryId = subCategoryItem.SubCategoryId,
            SubCategoryName = subCategoryItem.SubCategoryName,
            CategoryId = subCategoryItem.CategoryId,
            CreatedBy = subCategoryItem.CreatedBy,
            UpdatedBy = subCategoryItem.UpdatedBy,
            CreatedTime = subCategoryItem.CreatedTime,
            UpdatedTime = subCategoryItem.UpdatedTime,
            Status = subCategoryItem.Status
            
        }).ToList();

        return new ResponseDto()
        {
            Message = "Get SubCategories successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = eventDto
        };
    }

    public async Task<ResponseDto> GetSubCategory(ClaimsPrincipal user, Guid subCategoryId)
    {
        var SubCategoryId = await _unitOfWork.SubCategoryRepository.GetById(subCategoryId);
        if (SubCategoryId == null)
        {
            return new ResponseDto
            {
                Message = "SubCategory not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var subCategoryDto = _mapper.Map<GetSubCategoryDto>(SubCategoryId);

        return new ResponseDto
        {
            Message = "SubCategory found successfully",
            Result = subCategoryDto,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> CreateSubCategory(ClaimsPrincipal user, CreateSubCategoryDto createSubCategoryDto)
    {
        SubCategory newSubCategory = new SubCategory()
        {
            SubCategoryName = createSubCategoryDto.SubCategoryName,
            CategoryId = createSubCategoryDto.CategoryId,
            CreatedBy = user.Identity.Name,
            UpdatedBy = "",
            CreatedTime = DateTime.Now,
            UpdatedTime = null,
            Status = 1
        };

        await _unitOfWork.SubCategoryRepository.AddAsync(newSubCategory);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "SubCategory created successfully",
            Result = newSubCategory,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateSubCategory(ClaimsPrincipal user, UpdateSubCategoryDto updateSubCategoryDto)
    {
        var subCategoryId =
            await _unitOfWork.SubCategoryRepository.GetAsync(x =>
                x.SubCategoryId == updateSubCategoryDto.SubCategoryId);
        if (subCategoryId == null)
        {
            return new ResponseDto
            {
                Message = "SubCategory not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        //update location
        subCategoryId.SubCategoryName = updateSubCategoryDto.SubCategoryName;
        subCategoryId.SubCategoryId = updateSubCategoryDto.SubCategoryId;
        subCategoryId.UpdatedBy = user.Identity.Name;
        subCategoryId.UpdatedTime = DateTime.UtcNow;


        //save changes
        _unitOfWork.SubCategoryRepository.Update(subCategoryId);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "SubCategory updated successfully",
            Result = subCategoryId,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> DeleteSubCategory(ClaimsPrincipal user, Guid subCategoryId)
    {
        var subCategoryID = await _unitOfWork.SubCategoryRepository.GetAsync(x => x.SubCategoryId == subCategoryId);
        if (subCategoryID == null)
        {
            return new ResponseDto
            {
                Message = "SubCategory not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        subCategoryID.Status = 0;
        subCategoryID.UpdatedBy = user.Identity.Name;
        subCategoryID.UpdatedTime = DateTime.UtcNow;

        //save changes
        _unitOfWork.SubCategoryRepository.Update(subCategoryID);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Events delete successfully",
            Result = subCategoryID,
            IsSuccess = true,
            StatusCode = 201
        };
    }
}