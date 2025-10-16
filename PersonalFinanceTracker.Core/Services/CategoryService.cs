using PersonalFinanceTracker.Core.Dto;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Data.Models;
using PersonalFinanceTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace PersonalFinanceTracker.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToListAsync();
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                throw new ArgumentException("Category name is required");

            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = dto.Name,
                Type = dto.Type
            };
        }
    }
}
