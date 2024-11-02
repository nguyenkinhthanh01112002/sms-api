using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.CategoryDtos;
using smsCoffee.WebAPI.DTOs.Common;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Services.CommonService;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService category, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryService = category;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory(AddingCategoryDto addingCategoryDto)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(addingCategoryDto);
                if(category == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFactory.Error<CategoryDto>(Request.Path, "Invalid input", StatusCodes.Status400BadRequest));
                }
                var data = _mapper.Map<CategoryDto>(category);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path,data,"Creat a new category successfully",StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exceptional occured while creating a category");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<CategoryDto>(Request.Path, "An exceptional occured while creating a category",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                var data = _mapper.Map<List<CategoryDto>>(categories);
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path,data,"Get all categories success",StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An exception occured while getting categories");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<CategoryDto>(Request.Path, "An exception occured while getting categories",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                var data = _mapper.Map<CategoryDto>(category);
                if (category == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<CategoryDto>(Request.Path,$"Get category by id: {id} not found",StatusCodes.Status404NotFound));
                } 
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path,data,$"Get category by id: {id} success",StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exception occured while getting by {id}");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<CategoryDto>(Request.Path,$"Get category by id: {id} fail",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategoryById(int id,UpdatingCategoryDto updatingCategoryDto)
        {
            try
            {
                var category = await _categoryService.UpdateCategoryByIdAsync(id, updatingCategoryDto);
                var data = _mapper.Map<CategoryDto>(category);
                if(category == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Success(Request.Path,data,$"Can't find id: {id}",StatusCodes.Status404NotFound));
                }
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data,$"Updating category by id: {id} success",StatusCodes.Status200OK ));
            }
            catch(Exception e )
            {
                _logger.LogError(e,$"An exceptional occured while updating by id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<CategoryDto>(Request.Path,$"Updating category by id: {id} fail",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> DeleteCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.DeleteCategoryByIdAsync(id);
                var data = _mapper.Map<CategoryDto>(category);
                if( category == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<CategoryDto>(Request.Path,$"Deleting category by id: {id} fail",StatusCodes.Status404NotFound));
                }
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, $"Delete category by id: {id} success", StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"An exception occured while deleting category by id: {id} ");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<CategoryDto>(Request.Path, $"An exception occured while deleting category by id: {id} ",StatusCodes.Status500InternalServerError));
            }
        }
 
    }
}
