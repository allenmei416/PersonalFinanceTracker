using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Core.Interfaces;

namespace PersonalFinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var cats = await _service.GetAllAsync();

            return Ok(cats);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post(CreateCategoryDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = result.CategoryId }, result);
        }
    }
}
