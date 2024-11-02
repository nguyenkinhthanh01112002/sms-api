using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.ProductDtos
{
    public class AddingProductDto
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int? Available { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public bool? IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}
