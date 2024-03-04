using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Models.VItri
{
    public class VitriUserViewModel
    {
       
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public string? EmailCaNhan { get; set; }
        public TruongHoc Tentruonghoc { get; set; }
        public string LinkCV { get; set; }
        public string[] DuAn { get; set; }

     
            }
}
