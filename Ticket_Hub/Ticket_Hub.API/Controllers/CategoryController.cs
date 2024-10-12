using Microsoft.AspNetCore.Mvc;
using Ticket_Hub.Models.DTO;
using Ticket_Hub.Models.DTO.Category;
using Ticket_Hub.Services.IServices;

namespace Ticket_Hub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetCategories
        (
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var responseDto =
                await _categoryService.GetCategories(User, filterOn, filterQuery, sortBy, pageNumber, pageSize);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<ResponseDto>> GetCategory
        (
            [FromRoute] Guid categoryId
        )
        {
            var responseDto = await _categoryService.GetCategory(User, categoryId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateCategory
        (
            [FromBody] CreateCategoryDto createCategoryDto
        )
        {
            var responseDto = await _categoryService.CreateCategory(User, createCategoryDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateCategory
        (
            [FromBody] UpdateCategoryDto updateCategoryDto
        )
        {
            var responseDto = await _categoryService.UpdateCategory(User, updateCategoryDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult<ResponseDto>> DeleteCategory
        (
            [FromRoute] Guid categoryId
        )
        {
            var responseDto = await _categoryService.DeleteCategory(User, categoryId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}