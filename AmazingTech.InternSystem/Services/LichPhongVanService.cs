using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Repositories;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Services
{
    public interface IGuiLichPhongVanService
    {
        public void AddLichPhongVan(LichPhongVanRequestModel model);
       
        public List<LichPhongVanViewModel> getLichPhongVanByIdNguoiDuocPhongVan();
        public List<LichPhongVanResponseModel> getLichPhongVanByIdNgPhongVan();

        public void DeleteLichPhongVanByIdNguoiDuocPhongVan(string id);
        public List<LichPhongVanViewModel> GetLichPhongVanViewModel();
    }
    public class LichPhongVanService : IGuiLichPhongVanService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILichPhongVanRepository _lichPhongVanRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public LichPhongVanService(ILichPhongVanRepository lichPhongVanRepository , IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEmailService emailService)
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
            if (model.ThoiGianPhongVan == null || model.DiaDiemPhongVan.Length == 0 || model.Email == null)
            {
                throw new BadHttpRequestException("You need to fill all information");
            }
            if (model.ThoiGianPhongVan.TimeOfDay > new TimeSpan(17, 0, 0) || model.ThoiGianPhongVan.TimeOfDay < new TimeSpan(9, 0, 0))
            {
                throw new BadHttpRequestException("Interview time is from 9:00 a.m. to 5:00 p.m");
            }
            var InternId = _userRepository.GetUserIdByEmail(model.Email);
            if (InternId == null)
            {
                throw new BadHttpRequestException("This Mail is not exist in database");
            }
            var NewLichPhongVan = new LichPhongVan()
            {
                CreatedBy = _userRepository.GetUserById(accountId).HoVaTen,
                IdNguoiPhongVan = accountId,
                IdNguoiDuocPhongVan = InternId,
                DiaDiemPhongVan = model.DiaDiemPhongVan,
                ThoiGianPhongVan = model.ThoiGianPhongVan,
                TrangThai = Data.Enum.Status.Not_Yet,
                InterviewForm = model.interviewForm,
                DaXacNhanMail = false,
                CreatedTime = DateTime.UtcNow,

            };
            _lichPhongVanRepository.addNewLichPhongVan(NewLichPhongVan);
            string context = "Gửi bạn ứng viên,\r\n\r\nĐại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi xin chân thành ghi nhận sự quan tâm của bạn đối với cơ hội thực tập tại Công ty chúng tôi." +
                "\r\n\r\nChúng tôi muốn mời bạn tham gia phỏng vấn để tìm hiểu và xem xét sự phù hợp của bạn với vị trí bạn muốn ứng tuyển tại công ty chúng tôi. Chúng tôi gửi đến bạn một số thông tin và tài liệu cần thiết:\r\n\r\n" +
                "Đây là lịch phỏng vấn của bạn\r\n\r\n " +
                model.ThoiGianPhongVan + "\r\n\r\n Đây là địa chỉ phỏng vấn\r\n\r\n" +
                model.DiaDiemPhongVan + "\r\n\r\n";
            string subject = "[AMAZINGTECH - HR] THƯ GHI NHẬN THÔNG TIN THỰC TẬP SINH";
            _emailService.SendMail(context, model.Email, subject);

        }

        public List<LichPhongVanResponseModel> getLichPhongVanByIdNgPhongVan()
        {
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //string accountId = "1";
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }
            var lichphongvan = _lichPhongVanRepository.GetLichPhongVanByIdNgPhongVan(accountId);
            var lichphongvanList = new List<LichPhongVanResponseModel>();
            foreach (var item in lichphongvan)
            {
                var lichphongvanrespone = new LichPhongVanResponseModel
                {
                    ID = item.Id,
                    DiaDiemPhongVan = item.DiaDiemPhongVan,
                    InterviewForm = item.InterviewForm.ToString(),
                    KetQua = item.KetQua.ToString(),
                    NguoiDuocPhongVan = _userRepository.GetUserById(item.IdNguoiDuocPhongVan).HoVaTen,
                    ThoiGianPhongVan = item.ThoiGianPhongVan,
                    TrangThai = item.TrangThai.ToString(),
                    NguoiPhongVan = _userRepository.GetUserById(item.IdNguoiPhongVan).HoVaTen
                };
                lichphongvanList.Add(lichphongvanrespone);
            }
            return lichphongvanList;
        }


        public List<LichPhongVanViewModel> getLichPhongVanByIdNguoiDuocPhongVan()
        {
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
           
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }
            var lichphongvan = _lichPhongVanRepository.GetLichPhongVanByIdNguoiDuocPhongVan(accountId);
            var lichphongvanList = new List<LichPhongVanViewModel>();
            foreach (var item in lichphongvan)
            {
                var lichphongvanrespone = new LichPhongVanViewModel
                {
                    ID = item.Id,
                    DiaDiemPhongVan = item.DiaDiemPhongVan,
                    InterviewForm = item.InterviewForm.ToString(),
                    KetQua = item.KetQua.ToString(),
                    NguoiDuocPhongVan = _userRepository.GetUserById(item.IdNguoiDuocPhongVan).HoVaTen,
                    ThoiGianPhongVan = item.ThoiGianPhongVan,
                    TrangThai = item.TrangThai.ToString(),
                    NguoiPhongVan = _userRepository.GetUserById(item.IdNguoiPhongVan).HoVaTen
                };
                lichphongvanList.Add(lichphongvanrespone);
            }
            return lichphongvanList;
        }

       
        public void DeleteLichPhongVanByIdNguoiDuocPhongVan(string id)
        {
            _lichPhongVanRepository.DeleteLichPhongVanByIdNguoiDuocPhongVan(id);
        }

        public List<LichPhongVanViewModel> GetLichPhongVanViewModel()
        {
            var lichPhongVanList = _lichPhongVanRepository.GetLichPhongVan();

            var result = lichPhongVanList.Select(x => new LichPhongVanViewModel
            {
                ID = x.Id,
                NguoiPhongVan = x.NguoiPhongVan.HoVaTen,
                NguoiDuocPhongVan = x.NguoiDuocPhongVan.HoVaTen,
                ThoiGianPhongVan = x.ThoiGianPhongVan,
                DiaDiemPhongVan = x.DiaDiemPhongVan,
                InterviewForm = x.ToString(),
                TrangThai = x.TrangThai.ToString(),
                KetQua = x.KetQua.ToString()
            }).ToList();

            return result;
        }

        

        
    }
}
