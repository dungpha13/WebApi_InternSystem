using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Repositories;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Services
{
    public interface IGuiLichPhongVanService
    {
        public void AddLichPhongVan(LichPhongVanRequestModel model);
        public List<LichPhongVan> getLichPhongVanByIdNgPhongVan();
        public List<LichPhongVan> getLichPhongVanByIdNguoiDuocPhongVan();
    }
    public class GuiLichPhongVanService : IGuiLichPhongVanService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILichPhongVanRepository _lichPhongVanRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public GuiLichPhongVanService(ILichPhongVanRepository lichPhongVanRepository , IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEmailService emailService)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _lichPhongVanRepository = lichPhongVanRepository;  
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddLichPhongVan(LichPhongVanRequestModel model)
        {

            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }
            if (model.ThoiGianPhongVan == null || model.DiaDiemPhongVan == null || model.Email == null)
            {
                throw new BadHttpRequestException("You need to fill all information");
            }
            var InternId = _userRepository.GetUserIdByEmail(model.Email);
            if (InternId == null)
            {
                throw new BadHttpRequestException("This Mail is not exist in database");
            }
            var NewLichPhongVan = new LichPhongVan()
            {
                IdNguoiPhongVan = accountId,
                IdNguoiDuocPhongVan = InternId,
                DiaDiemPhongVan = model.DiaDiemPhongVan,
                ThoiGianPhongVan = model.ThoiGianPhongVan,
                TrangThai = Data.Enum.Status.Not_Yet,
            };
            _lichPhongVanRepository.addNewLichPhongVan(NewLichPhongVan);
            string context = "Gửi bạn ứng viên,\r\n\r\nĐại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi xin chân thành ghi nhận sự quan tâm của bạn đối với cơ hội thực tập tại Công ty chúng tôi." +
                "\r\n\r\nChúng tôi muốn mời bạn tham gia phỏng vấn để tìm hiểu và xem xét sự phù hợp của bạn với vị trí bạn muốn ứng tuyển tại công ty chúng tôi. Chúng tôi gửi đến bạn một số thông tin và tài liệu cần thiết:\r\n\r\n" +
                "Đây là lịch phỏng vấn của bạn\r\n\r\n " +
                model.ThoiGianPhongVan + "\r\n\r\n Đây là địa chỉ phỏng vấn\r\n\r\n" +
                model.DiaDiemPhongVan + "\r\n\r\n";
            string subject = "[AMAZINGTECH - HR] THƯ GHI NHẬN THÔNG TIN THỰC TẬP SINH";
            _emailService.SendMail(context,model.Email,subject);

        }

        public List<LichPhongVan> getLichPhongVanByIdNgPhongVan()
        {
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }
            return _lichPhongVanRepository.GetLichPhongVanByIdNgPhongVan(accountId);

        }

        public List<LichPhongVan> getLichPhongVanByIdNguoiDuocPhongVan()
        {
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }
            return _lichPhongVanRepository.GetLichPhongVanByIdNguoiDuocPhongVan(accountId);

        }
    }
}
