using PersonalFinanceTracker.Core.Dto;

namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    }
}
