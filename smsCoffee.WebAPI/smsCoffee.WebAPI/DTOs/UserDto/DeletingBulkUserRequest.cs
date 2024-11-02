using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class DeletingBulkUserRequest
    {
 
        public List<string>? Ids { get; set; }
    }
}
