namespace AmazingTech.InternSystem.Models.Response.User
{
    public class ProfileResponseDTO
    {
        public string id { get; set; }
        public string HoVaTen { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<string>? Roles { get; set; }
    }
}
