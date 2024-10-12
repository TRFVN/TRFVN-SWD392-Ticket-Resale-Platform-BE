using System.Security.Claims;
using AutoMapper;
using Ticket_Hub.DataAccess.Context;
using Ticket_Hub.DataAccess.Repository;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Location;
using Ticket_Hub.Models.Models;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.Services.Services;

public class LocationService : ILocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;

    public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<ResponseDto> GetLocations
    (
        ClaimsPrincipal user,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        int pageNumber = 0,
        int pageSize = 0
    )
    {
        IEnumerable<Location> locations = null!;

        // Lấy tất cả các vé có trong database
        locations = _unitOfWork.LocationRepository.GetAllAsync().GetAwaiter()
            .GetResult()
            .ToList();


        // Kiểm tra nếu danh sách location là null hoặc rỗng
        if (!locations.Any())
        {
            return Task.FromResult(new ResponseDto()
            {
                Message = "There are no location",
                IsSuccess = true,
                StatusCode = 404,
                Result = null
            });
        }

        var listLocations = locations.ToList();

        // Filter Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            switch (filterOn.Trim().ToLower())
            {
                case "city":
                    listLocations = listLocations.Where(x =>
                        x.City.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                case "district":
                    listLocations = listLocations.Where(x =>
                        x.District.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                case "street":
                    listLocations = listLocations.Where(x =>
                        x.Street.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                default:
                    break;
            }
        }

        // Sort Query (có thể truyền hướng sắp xếp trong `sortBy`)
        if (!string.IsNullOrEmpty(sortBy))
        {
            var sortParams = sortBy.Trim().ToLower().Split('_'); // Chia chuỗi sortBy theo ký tự '_'
            var sortField = sortParams[0]; // Tên cột cần sắp xếp
            var sortDirection = sortParams.Length > 1 ? sortParams[1] : "asc"; // Lấy hướng sắp xếp

            switch (sortField)
            {
                case "city":
                    listLocations = sortDirection == "desc"
                        ? listLocations.OrderByDescending(x => x.City).ToList()
                        : listLocations.OrderBy(x => x.City).ToList();
                    break;

                case "district":
                    listLocations = sortDirection == "desc"
                        ? listLocations.OrderByDescending(x => x.District).ToList()
                        : listLocations.OrderBy(x => x.District).ToList();
                    break;

                case "street":
                    listLocations = sortDirection == "desc"
                        ? listLocations.OrderByDescending(x => x.Street).ToList()
                        : listLocations.OrderBy(x => x.Street).ToList();
                    break;

                default:
                    // Sắp xếp theo mặc định nếu không có cột phù hợp
                    listLocations = listLocations.OrderBy(x => x.City).ToList();
                    break;
            }
        }
        else
        {
            // Sắp xếp mặc định nếu không có `sortBy`
            listLocations = listLocations.OrderBy(x => x.City).ToList();
        }


        // Phân trang
        if (pageNumber > 0 && pageSize > 0)
        {
            var skipResult = (pageNumber - 1) * pageSize;
            listLocations = listLocations.Skip(skipResult).Take(pageSize).ToList();
        }

        // Chuyển đổi danh sách bình luận thành DTO
        var locationDto = listLocations.Select(locationsID => new GetLocationDto()
        {
            LocationId = locationsID.LocationId,
            City = locationsID.City,
            District = locationsID.District,
            Street = locationsID.Street
        }).ToList();

        return Task.FromResult(new ResponseDto()
        {
            Message = "Get Tickets successfully",
            IsSuccess = true,
            StatusCode = 201,
            Result = locationDto
        });
    }

    public async Task<ResponseDto> GetLocation(ClaimsPrincipal user, Guid locationId)
    {
        var location = await _unitOfWork.LocationRepository.GetById(locationId);
        if (location == null)
        {
            return new ResponseDto
            {
                Message = "Location not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var locationDto = _mapper.Map<GetLocationDto>(location);

        return new ResponseDto
        {
            Message = "Location found",
            Result = locationDto,
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> CreateLocation(ClaimsPrincipal user, CreateLocationDto createLocationDto)
    {
        Location location = new Location()
        {
            City = createLocationDto.City,
            District = createLocationDto.District,
            Street = createLocationDto.Street,
            CreatedBy = user.Identity.Name,
            UpdatedBy = "",
            CreatedTime = DateTime.UtcNow,
            UpdatedTime = null,
            Status = 1
        };

        await _unitOfWork.LocationRepository.AddAsync(location);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Location created successfully",
            Result = location,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> UpdateLocation(ClaimsPrincipal user, UpdateLocationDto updateLocationDto)
    {
        //Check if location exists
        var location = await _unitOfWork.LocationRepository.GetAsync(x => x.LocationId == updateLocationDto.LocationId);
        if (location == null)
        {
            return new ResponseDto
            {
                Message = "Ticket not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        //update location
        location.City = updateLocationDto.City;
        location.District = updateLocationDto.District;
        location.Street = updateLocationDto.Street;
        location.UpdatedBy = user.Identity.Name;
        location.UpdatedTime = DateTime.UtcNow;


        //save changes
        _unitOfWork.LocationRepository.Update(location);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Message = "Location updated successfully",
            Result = location,
            IsSuccess = true,
            StatusCode = 201
        };
    }

    public async Task<ResponseDto> DeleteLocation(ClaimsPrincipal user, Guid locationId)
    {
        //Check if location exists
        var location = await _unitOfWork.LocationRepository.GetAsync(x => x.LocationId == locationId);
        if (location == null)
        {
            return new ResponseDto
            {
                Message = "Location not found",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        location.Status = 0;
        location.UpdatedBy = user.Identity.Name;
        location.UpdatedTime = DateTime.UtcNow;

        //save changes
        _unitOfWork.LocationRepository.Update(location);
        var save = await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Location delete successfully",
            Result = location,
            IsSuccess = true,
            StatusCode = 201
        };
    }
}