using Microsoft.EntityFrameworkCore;
using MyForeignCards.Data;
using MyForeignCards.DTOs;
using MyForeignCards.Entities;

namespace MyForeignCards.Services
{
    public class CategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ApplicationContext _context;

        public CategoryService(ILogger<CategoryService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<CategoryResponse> AddCategoryAsync(Category newCategory)
        {
            var category = _context.Categories.Add(newCategory).Entity;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Category {CategoryId} added",
                newCategory.Id);

            _logger.LogDebug("Category {CategoryId} added. Category name: {Name}",
                newCategory.Id,
                newCategory.Name);

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public Task<List<CategoryResponse>> GetAllCategoriesAsync()
        {
            return _context.Categories
                .AsNoTracking()
                .Select(category => new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Category {Id} deleted",
                    category.Id);

                _logger.LogDebug("Category {Id} deleted. Category: {Name}",
                    category.Id,
                    category.Name);

                return true;
            }
            else
            {
                _logger.LogWarning("Category {Id} delete failed: category was not found", id);

                return false;
            }
        }
    }
}
