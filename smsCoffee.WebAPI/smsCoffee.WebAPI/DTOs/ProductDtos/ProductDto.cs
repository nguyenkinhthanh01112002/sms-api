using smsCoffee.WebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using smsCoffee.WebAPI.DTOs.CategoryDtos;

namespace smsCoffee.WebAPI.DTOs.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public int? Available { get; set; }    
        public double Price { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int CategoryId {  get; set; }
        public CategoryDto Category { get; set; }
    }
}
