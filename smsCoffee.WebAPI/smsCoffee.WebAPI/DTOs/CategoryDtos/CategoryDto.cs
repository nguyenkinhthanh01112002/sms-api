using smsCoffee.WebAPI.DTOs.ProductDtos;

namespace smsCoffee.WebAPI.DTOs.CategoryDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    //    public List<ProductDto> Products { get; set; }
    }
}
