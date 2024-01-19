using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.InternInfo;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ICommentRepository
    {
    
        public Task<int> AddCommentIntern(Comment comment, string IdCommentor, string mssv);
    }
}
