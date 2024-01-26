using System.ComponentModel.DataAnnotations;

namespace swp391_be.API.Models.Request.User
{
    public class UpdateUserRequestDTO
    {
        public string? HoVaTen { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
    }
}
