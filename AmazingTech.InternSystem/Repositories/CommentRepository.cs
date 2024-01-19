using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddCommentIntern(Comment comment, string IdCommentor, string mssv)
        {
            var intern = await _context.InternInfos.Where(x => x.MSSV == mssv).FirstOrDefaultAsync();
            //var commentor = user!.Id;

            comment.IdNguoiComment = IdCommentor;
            comment.IdNguoiDuocComment = intern!.Id!;

            _context.Comments.Add(comment);
            return await _context.SaveChangesAsync();
        }

       
    }
}
