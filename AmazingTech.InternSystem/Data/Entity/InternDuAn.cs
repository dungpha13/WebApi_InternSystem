namespace AmazingTech.InternSystem.Data
{
    public class InternDuAn : Entity
    {
        public string IdIntern { get; set; }
        public string IdDuAn { get; set; }
        public string ViTri { get; set; }
        public InternInfo InternInfo { get; set; }
        public DuAn DuAn { get; set; }
    }
}