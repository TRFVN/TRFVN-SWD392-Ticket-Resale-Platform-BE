using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.SubCategory;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetEvents
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var responseDto =
                await _subCategoryService.GetSubCategories(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{subCategoryId}")]
        public async Task<ActionResult<ResponseDto>> GetEvent
        (
            [FromRoute] Guid subCategoryId
        )
        {
            var responseDto = await _subCategoryService.GetSubCategory(User, subCategoryId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateSubCategory
        (
            [FromBody] CreateSubCategoryDto createLocationDto
        )
        {
            var responseDto = await _subCategoryService.CreateSubCategory(User, createLocationDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateSubCategory
        (
            [FromBody] UpdateSubCategoryDto updateLocationDto
        )
        {
            var responseDto = await _subCategoryService.UpdateSubCategory(User, updateLocationDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpDelete("{subCategoryId}")]
        public async Task<ActionResult<ResponseDto>> DeleteSubCategory
        (
            [FromRoute] Guid subCategoryId
        )
        {
            var responseDto = await _subCategoryService.DeleteSubCategory(User, subCategoryId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}