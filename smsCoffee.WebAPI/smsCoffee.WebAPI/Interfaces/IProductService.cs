using smsCoffee.WebAPI.DTOs.ProductDtos;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface IProductService
    {
        public Task<Product?> CreateProductAsync(AddingProductDto addingProductDto);
        public Task<Product?> UpdateProductByIdAsync(int id,UpdatingProductDto updatingProductDto);
        public Task<Product?> GetProductByIdAsync(int id);
        public Task<List<Product>?> GetProductsAsync();
        public Task<Product?> DeleteProductByIdAsync(int id);
    }
}
