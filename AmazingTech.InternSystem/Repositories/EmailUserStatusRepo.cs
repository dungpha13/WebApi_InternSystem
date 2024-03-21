using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{

    public interface IEmailUserStatusRepo
    {
        Task<int> AddEmailUserStatus(EmailUserStatus email);
        Task<bool> HasSentEmail(string idNguoiNhan);
       
        Task<List<EmailUserStatus>> GetEmailUserStatusList();
    }

    public class EmailUserStatusRepo : IEmailUserStatusRepo
    {
        private readonly AppDbContext _context;

        public EmailUserStatusRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<EmailUserStatus>> GetEmailUserStatusList()
        {
            var listEmailUserStatus = await _context.EmailUserStatus.ToListAsync();
            return listEmailUserStatus;
        }

        public async Task<int> AddEmailUserStatus(EmailUserStatus email)
        {
            _context.EmailUserStatus.Add(email);
            return await _context.SaveChangesAsync();
        }

        //Hàm tránh spam Mail
        public async Task<bool> HasSentEmail(string idNguoiNhan)
        {
            // Kiểm tra xem trong bảng EmailUserStatus có dữ liệu chưa
            var hasData = await _context.EmailUserStatus.AnyAsync();

            // Nếu không có dữ liệu trong bảng, trả về false
            if (!hasData)
            {
                return false;
            }

            var hasSent = await _context.EmailUserStatus.AnyAsync(e => e.idNguoiNhan == idNguoiNhan && e.EmailLoai1 == true);
            //True: Nếu mail được gửi rồi
            //False: Nếu mail chưa được gửi
            
            return hasSent;

        }

        
    }
}
