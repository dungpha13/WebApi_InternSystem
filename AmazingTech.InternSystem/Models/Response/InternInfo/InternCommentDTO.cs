using AmazingTech.InternSystem.Data.Entity;
using System.Security;

namespace AmazingTech.InternSystem.Models.Response.InternInfo
{
    public class InternCommentDTO
    {
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public List<CommentDTO> Comment { get; set; }
    }
}
