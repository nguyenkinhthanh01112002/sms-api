
using smsCoffee.WebAPI.DTOs.CategoryDtos;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category?> CreateCategoryAsync(AddingCategoryDto addingCategoryDto);
        public Task<List<Category>?> GetCategoriesAsync();
        public Task<Category?> GetCategoryByIdAsync(int id);
        public Task<Category?> UpdateCategoryByIdAsync(int id,UpdatingCategoryDto updatingCategoryDto);
        public Task<Category?> DeleteCategoryByIdAsync(int id);
    }
}
