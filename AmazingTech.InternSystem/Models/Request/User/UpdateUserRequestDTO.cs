using System.ComponentModel.DataAnnotations;

namespace swp391_be.API.Models.Request.User
{
    public class UpdateUserRequestDTO
    {
        public string? HoVaTen { get; set; }
       
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public Boolean? Gender { get; set; }
        public string? Bio { get; set; }
    }
}
