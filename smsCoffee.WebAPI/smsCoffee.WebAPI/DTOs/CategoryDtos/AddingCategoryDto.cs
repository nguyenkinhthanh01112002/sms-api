using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.CategoryDtos
{
    public class AddingCategoryDto
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
