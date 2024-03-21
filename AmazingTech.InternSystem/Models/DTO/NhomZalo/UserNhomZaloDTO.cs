namespace AmazingTech.InternSystem.Models.DTO.NhomZalo
{
    public class UserNhomZaloDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsMentor { get; set; }
        public string NhomZalo { get; set; }
        public DateTime? JoinedTime { get; set; }
        public DateTime? LeftTime { get; set; }
    }
}
