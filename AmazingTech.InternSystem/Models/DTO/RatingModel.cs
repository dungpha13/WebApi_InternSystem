namespace AmazingTech.InternSystem.Models.DTO
{
    public class RatingModel
    {
        public string Id { get; set; }
        public int Rank { get; set; }
        public DateTime? RankDate { get; set; } = DateTime.Now;
    }
}
