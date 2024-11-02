using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.CategoryDtos;
using smsCoffee.WebAPI.DTOs.Common;
using smsCoffee.WebAPI.DTOs.ProductDtos;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Services.CategoryServices;
using smsCoffee.WebAPI.Services.CommonService;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct(AddingProductDto addingProductDto)
        {
            try
            {
                var product = await _productService.CreateProductAsync(addingProductDto);
                if (product == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,ResponseFactory.Error<ProductDto>(Request.Path,"Invalid input",StatusCodes.Status400BadRequest));
                }
                var data = _mapper.Map<ProductDto>(product);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, "Create product successfully", StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exceptional occured while creating a product");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<ProductDto>(Request.Path, "An exceptional occured while creating a product", StatusCodes.Status500InternalServerError));
            }
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                var data = _mapper.Map<List<ProductDto>>(products);
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path,data,"Get products success",StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An exceptional occured while getting products");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<List<ProductDto>>(Request.Path, "An exception occured while getting products"));
            }
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse<ProductDto?>>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if(product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<ProductDto>(Request.Path,$"Product by id: {id} not found",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<ProductDto>(product);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, $"Get product by id: {id} success", StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exception occured while getting product by id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<ProductDto?>(Request.Path, $"An exception occured while getting product by id: {id}", StatusCodes.Status500InternalServerError));
            }
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse<ProductDto?>>> UpdateProductById(int id,UpdatingProductDto updatingProductDto)
        {
            try
            {
                var product = await _productService.UpdateProductByIdAsync(id, updatingProductDto);
                if(product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<ProductDto>(Request.Path,$"Updating product by id: {id} fail",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<ProductDto>(product) ;
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, $"Updating product by id: {id} success", StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exception occured while updating product by id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<ProductDto?>(Request.Path, $"An exception occured while updating product by id: {id}", StatusCodes.Status500InternalServerError));
            }
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse<ProductDto?>>> DeleteProductById(int id)
        {
            try
            {
                var product = await _productService.DeleteProductByIdAsync(id);
                if(product == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFactory.Error<ProductDto>(Request.Path,$"Deleting product by id: {id} fail",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<ProductDto>(product);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, $"Delete product by id: {id} success", StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured while deleting by id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<ProductDto>(Request.Path, $"An exception occured while deleting by id: {id}", StatusCodes.Status500InternalServerError));
            }
        }
    }
}
