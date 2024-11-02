using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.DTOs.CategoryDtos;
using smsCoffee.WebAPI.DTOs.ProductDtos;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;
using smsCoffee.WebAPI.Services.CategoryServices;

namespace smsCoffee.WebAPI.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly CoffeeDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        public ProductService(CoffeeDbContext context,IMapper  mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Product?> CreateProductAsync(AddingProductDto addingProductDto)
        {

            try
            {
                var category = await _context.Category.FindAsync(addingProductDto.CategoryId);
                if (category == null)
                {
                    return null;
                }
                var product = new Product
                {
                    Name = addingProductDto.Name.Trim(),
                    Available = addingProductDto.Available,
                    Price = addingProductDto.Price,
                    IsActive = addingProductDto.IsActive,
                    CategoryId = addingProductDto.CategoryId,
                    Category = category,
                    CreatedOn = DateTime.UtcNow
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _context.Product.AddAsync(product);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return product;
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
        public async Task<List<Product>?> GetProductsAsync()
        {
            try
            {
                return await _context.Product.ToListAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An exception occured while getting products");
                throw;
            }         
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Product.FindAsync(id);
                return product;
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exception occured while getting product by id: {id}");
                throw;
            }
        }
        public async Task<Product?> DeleteProductByIdAsync(int id)
        {
            try
            {
                var product = await _context.Product.FindAsync(id);
                if(product != null)
                {
                    _context.Product.Remove(product);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return product;
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exception occured while deleting product by id: {id}");
                throw;
            }
        }
        public async Task<Product?> UpdateProductByIdAsync(int id, UpdatingProductDto updatingProductDto)
        {
            try
            {
                var product = await _context.Product.FindAsync(id);
                if(product != null)
                {
                    product.Name = String.IsNullOrEmpty(updatingProductDto.Name) ? product.Name : updatingProductDto.Name;
                    product.Price = updatingProductDto.Price;
                    product.Available = updatingProductDto.Available;
                    product.CategoryId = updatingProductDto.CategoryId;
                    product.UpdatedOn = DateTime.UtcNow;
                    product.IsActive = updatingProductDto.IsActive;
                    var result = await _context.SaveChangesAsync();
                    if(result> 0)
                    {
                        return product;
                    }                   
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured while updating product by id: {id}");
                throw;
            }
        }
    }
}
