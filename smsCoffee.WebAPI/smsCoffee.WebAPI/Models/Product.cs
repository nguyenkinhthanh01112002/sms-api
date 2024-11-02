using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.Models
{
    public class Product : BaseEntity
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
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
