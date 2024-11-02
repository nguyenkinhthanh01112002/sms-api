using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Data;

using smsCoffee.WebAPI.DTOs.CategoryDtos;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly CoffeeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(CoffeeDbContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            _logger = logger;
        }
        public async Task<Category?> CreateCategoryAsync(AddingCategoryDto addingCategoryDto)
        {
            try
            {            
                var category = new Category
                {
                    Name = addingCategoryDto.Name.Trim(),
                    Description = addingCategoryDto.Description?.Trim(),
                    IsActive = addingCategoryDto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _context.Category.AddAsync(category);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return category;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                // Log exception
                _logger.LogError(e, "Error creating category");
                throw;
            }
        }
        public async Task<List<Category>?> GetCategoriesAsync()
        {
            try
            {
                return await _context.Category.ToListAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error creating category");
                throw;
            }
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Category.FindAsync(id);
                return category;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error getting category");
                throw;
            }
        }
        public async Task<Category?> UpdateCategoryByIdAsync(int id,UpdatingCategoryDto updatingCategoryDto)
        {
            try
            {
                var category = await _context.Category.FindAsync(id);
                if (category != null)
                {
                    category.Name = String.IsNullOrEmpty(updatingCategoryDto.Name) ? category.Name : updatingCategoryDto.Name;
                    category.Description = String.IsNullOrEmpty(updatingCategoryDto.Description) ? category.Description : updatingCategoryDto.Description;
                    category.IsActive = updatingCategoryDto.IsActive;
                    category.UpdatedAt = DateTime.UtcNow;
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return category;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error updating category by id: {id}");
                throw;
            }
        }
        public async Task<Category?> DeleteCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context.Category.FindAsync(id);
                if (category != null)
                {
                    _context.Remove(category);
                    var result = await _context.SaveChangesAsync();
                    if(result > 0)
                    {
                        return category;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"An exception occured while deleting category by id: {id}");
                throw;
            }
        }
    }
}
