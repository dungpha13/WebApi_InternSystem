using AmazingTech.InternSystem.Data.Enum;

namespace AmazingTech.InternSystem.Models.Response.User
{
    public class GetUserDTO
    {
        public string Id { get; set; }
        public string FullNameOrSchoolName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TrangThaiThucTap TrangThaiThucTap { get; set; }
        public List<string>? ViTris { get; set; }
        public List<string>? Roles { get; set; }
    }
}
