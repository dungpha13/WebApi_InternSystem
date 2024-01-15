namespace AmazingTech.InternSystem.Models.Request.InternInfo
{
    public class AddInternInfoDTO
    {
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public string EmailCaNhan { get; set; }
        public string EmailTruong { get; set; }
        public string LinkCV { get; set; }
        public string TrinhDoTiengAnh { get; set; }
        public string[] IdViTris{ get; set; }

        public bool GioiTinh { get; set; }
    }
}
