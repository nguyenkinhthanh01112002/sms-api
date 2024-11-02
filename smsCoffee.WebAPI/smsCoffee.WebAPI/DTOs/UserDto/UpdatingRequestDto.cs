using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class UpdatingRequestDto
    {
        public string? fullName {  get; set; }

        [DataType(DataType.Date)]
        public DateTime? dateOfBirth { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }
        public bool? isActive { get; set; } 

    }
}
