using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Models.Response;
using AmazingTech.InternSystem.Repositories;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Services
{
    public interface IGuiLichPhongVanService
    {
        public void AddLichPhongVan(LichPhongVanRequestModel model);
        public List<LichPhongVanResponseModel> getmyInterviewSchedule();
        public LichPhongVanResponseModel UpdateSchedule(LichPhongVanRequestModel request);
        public void deleteSchedudle(string ScheduleId);
        public bool ConfirmEmail(string id);
        public List<User> GetInternWithoutInternView(DateTime startDate, DateTime endDate);
        public List<User> GetHrOrMentorWithoutInternView(DateTime startDate, DateTime endDate);
        public void AutoCreateSchedule(DateTime startTime, DateTime endTime, string DiaDiemPhongVan, InterviewForm interviewForm);
    }
    public class LichPhongVanService : IGuiLichPhongVanService
    {
    
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILichPhongVanRepository _lichPhongVanRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public LichPhongVanService(ILichPhongVanRepository lichPhongVanRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEmailService emailService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _userRepository = userRepository;
            _lichPhongVanRepository = lichPhongVanRepository;
            _httpContextAccessor = httpContextAccessor;

        }

        public bool ConfirmEmail(string id)
        {
            var lichPhongVan = _lichPhongVanRepository.GetScheduleByIntervieweeId(id);

            if (lichPhongVan == null)
            {
                return false;
            }

            // Update user status (e.g., set DaXacNhanMail to true)
            lichPhongVan.DaXacNhanMail = true;
            _lichPhongVanRepository.UpdateLichPhongVan(lichPhongVan);

            return true;
        }

        public void AddLichPhongVan(LichPhongVanRequestModel model)
        {
            if(model.ThoiGianPhongVan < DateTime.Now)
            {
                throw new BadHttpRequestException("The date of interview must be in the future");
            }
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            // string accountId = "148ee64c-0ba2-47a1-abee-e83010944149";
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");

            }

            if (model.ThoiGianPhongVan == null || model.DiaDiemPhongVan.Length == 0 || model.Email == null || TimeSpan.FromMinutes(model.TimeDuration) <= new TimeSpan(0, 0, 0))
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
            var Intern = _userRepository.GetUserById(InternId);
            var InternRole = _userManager.GetRolesAsync(Intern).Result;
            int countRole = 0;
            foreach (var item in InternRole)
            {
                if (item.ToUpper() != Roles.INTERN.ToUpper())
                {
                    countRole++;
                }
            }
            if (countRole > 0)
            {
                throw new BadHttpRequestException("This interviewee isn't intern so we can't create schedule");
            }
            var ScheduleisExist = _lichPhongVanRepository.GetScheduleByInterviewerIdAndIntervieweeId(accountId, InternId);
            if (ScheduleisExist != null)
            {
                throw new BadHttpRequestException("This intern already has interview schedule");
            }
            var Interviewer = _userRepository.GetUserByName(model.HoVaTenNgPhongVan);
            if (Interviewer == null)
            {
                throw new BadHttpRequestException("Can't find this interviewer, please write her/his name correctly");
            }
            if (!(accountRole.Equals(Roles.HR.ToUpper()) || accountRole.Equals(Roles.ADMIN))) // không phải là HR hay Admin thì không lịch đc tạo 
            {
                throw new BadHttpRequestException("You don't have permission to create schedule");
            }
            var InterViewListRole = _userManager.GetRolesAsync(Interviewer).Result;
            var count = 0;
            // Người Phỏng Vấn phải là HR hoặc Mentor
            foreach (var item in InterViewListRole)
            {
                if (!(item.ToUpper() == Roles.HR.ToUpper() || item.ToUpper() == Roles.MENTOR.ToUpper()))
                {
                    count++;
                }
            }
            if (count != 0)
            {
                throw new BadHttpRequestException("This interviewer has no right to be the interviewer");
            }
            // Nếu Người đang tạo lịch là HR thì ng phỏng vấn cũng phải là ng login
            if (accountRole.Equals(Roles.HR.ToUpper()) && Interviewer.Id != accountId)
            {
                throw new BadHttpRequestException("You don't have permission to create schedule for this interviewer");
            }

            var NewLichPhongVan = new LichPhongVan()
            {
                Id = Guid.NewGuid().ToString("N"),
                CreatedBy = _userRepository.GetUserById(accountId).HoVaTen,
                IdNguoiPhongVan = Interviewer.Id,
                IdNguoiDuocPhongVan = InternId,
                DiaDiemPhongVan = model.DiaDiemPhongVan,
                ThoiGianPhongVan = model.ThoiGianPhongVan,
                TrangThai = Data.Enum.Status.Not_Yet,
                InterviewForm = model.interviewForm,
                DaXacNhanMail = false,
                LastUpdatedBy = _userRepository.GetUserById(accountId).HoVaTen,
                LastUpdatedTime = DateTime.Now,
                CreatedTime = DateTime.Now,
                TimeDuration = TimeSpan.FromMinutes(model.TimeDuration),

            };

            _lichPhongVanRepository.addNewLichPhongVan(NewLichPhongVan);
            string context = "Gửi bạn ứng viên,\r\n\r\nĐại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi xin chân thành ghi nhận sự quan tâm của bạn đối với cơ hội thực tập tại Công ty chúng tôi." +
                "\r\n\r\nChúng tôi muốn mời bạn tham gia phỏng vấn để tìm hiểu và xem xét sự phù hợp của bạn với vị trí bạn muốn ứng tuyển tại công ty chúng tôi. Chúng tôi gửi đến bạn một số thông tin và tài liệu cần thiết:\r\n\r\n" +
                "Đây là lịch phỏng vấn của bạn\r\n\r\n " +
                model.ThoiGianPhongVan + "\r\n\r\n Khoảng thời gian phỏng vấn dự kiến \r\n\r\n" +
                +model.TimeDuration +
                "\r\n\r\n Đây là địa chỉ phỏng vấn\r\n\r\n" +
                model.DiaDiemPhongVan + "\r\n\r\n Hình thức phỏng vấn\r\n\r\n" +
                model.interviewForm.ToString()
                ;
            string subject = "[AMAZINGTECH - HR] THƯ GHI NHẬN THÔNG TIN THỰC TẬP SINH";
            _emailService.SendMail(context, model.Email, subject, InternId);


        }
        public List<LichPhongVanResponseModel> getmyInterviewSchedule()

        {

            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //string accountId = "1";
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }

            var lichphongvan = _lichPhongVanRepository.GetLichPhongVansByIdNgPhongVan(accountId);
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
                    NguoiPhongVan = _userRepository.GetUserById(item.IdNguoiPhongVan).HoVaTen,
                    TimeDuration = item.TimeDuration
                };
                lichphongvanList.Add(lichphongvanrespone);
            }
            return lichphongvanList;
        }
        public LichPhongVanResponseModel UpdateSchedule(LichPhongVanRequestModel request)
        {
            if(request.ThoiGianPhongVan < DateTime.Now)
            {
                throw new BadHttpRequestException("The date of interview must be in the future");
            }
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (request.ThoiGianPhongVan.TimeOfDay > new TimeSpan(17, 0, 0) || request.ThoiGianPhongVan.TimeOfDay < new TimeSpan(9, 0, 0))
            {
                throw new BadHttpRequestException("Interview time is from 9:00 a.m. to 5:00 p.m");
            }
            var Interviewer = _userRepository.GetUserByName(request.HoVaTenNgPhongVan);
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var accountLogin = _userRepository.GetUserById(accountId);
            ; string InternId = _userRepository.GetUserIdByEmail(request.Email);
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to update schedule");
            }
            if (InternId == null)
            {
                throw new BadHttpRequestException("This intern doesn't exist in data");
            }
            var Intern = _userRepository.GetUserById(InternId);
            var InternRole = _userManager.GetRolesAsync(Intern).Result;
            int countRole = 0;
            foreach (var item in InternRole)
            {
                if (item.ToUpper() != Roles.INTERN.ToUpper())
                {
                    countRole++;
                }
            }
            if (countRole > 0)
            {
                throw new BadHttpRequestException("This interviewee isn't intern so we can't create schedule");
            }
            var lichphongvan = _lichPhongVanRepository.GetScheduleByIntervieweeId(InternId);
            if (lichphongvan == null)
            {
                throw new BadHttpRequestException("This intern doesn't have any interview schedule");
            }

            if (request.ThoiGianPhongVan == null || request.DiaDiemPhongVan.Length == 0 || request.Email == null || TimeSpan.FromMinutes(request.TimeDuration) <= new TimeSpan(0, 0, 0))
            {
                throw new BadHttpRequestException("You need to fill all information");
            }
            if (Interviewer == null)
            {
                throw new BadHttpRequestException("Can't find this interviewer, please write her/his name correctly");
            }
            int count = 0;
            var InterViewListRole = _userManager.GetRolesAsync(Interviewer).Result;
            foreach (var item in InterViewListRole)
            {
                if (!(item.ToUpper() == Roles.HR.ToUpper() || item.ToUpper() == Roles.MENTOR))
                {
                    count++;
                }
            }
            if (count != 0)
            {
                throw new BadHttpRequestException("This interviewer has no right to be the interviewer");
            }
            if (accountRole.Equals(Roles.HR.ToUpper()) && Interviewer.Id != accountId)
            {
                throw new BadHttpRequestException("You don't have permission to update schedule for this interviewer");
            }
            if (!(accountRole.Equals(Roles.HR.ToUpper()) || accountRole.Equals(Roles.ADMIN)))
            {
                throw new BadHttpRequestException("You don't have permission to Update schedule");
            }

            lichphongvan.IdNguoiPhongVan = Interviewer.Id;
            lichphongvan.InterviewForm = request.interviewForm;
            lichphongvan.LastUpdatedTime = DateTime.Now;
            lichphongvan.ThoiGianPhongVan = request.ThoiGianPhongVan;
            lichphongvan.DiaDiemPhongVan = request.DiaDiemPhongVan;
            _lichPhongVanRepository.UpdateLichPhongVan(lichphongvan);
            string content = "Kính gửi bạn " + _userRepository.GetUserById(InternId).HoVaTen + ",\r\nĐại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi xin chân thành xin lỗi khi phải thông báo về việc dời lại lịch phỏng vấn. \r\nĐây là lịch phỏng vấn mới của bạn " +
                request.ThoiGianPhongVan +
                "\r\n\r\n Khoảng thời gian phỏng vấn dự kiến \r\n\r\n" + request.TimeDuration +
                "\r\n\r\n Hình thức phỏng vấn " + request.interviewForm.ToString() + "\r\n\r\n Địa điểm phỏng vấn " +
                request.DiaDiemPhongVan +
                "\r\n\r\n Xin cảm ơn sự hiểu biết và sự linh hoạt của bạn trong việc xem xét yêu cầu của tôi. Xin vui lòng cho chúng tôi  biết nếu có bất kỳ điều gì cần được điều chỉnh hoặc có bất kỳ thông tin nào khác chúng tôi cần cung cấp.\r\n\r\nTrân trọng";
            string subject = "[AMAZINGTECH - HR] THƯ THÔNG BÁO DỜI LỊCH PHỎNG VẤN";
            _emailService.SendMail(content, request.Email, subject, InternId);
            var lichphongvannew = new LichPhongVanResponseModel()
            {
                ID = lichphongvan.Id,
                DiaDiemPhongVan = request.DiaDiemPhongVan,
                NguoiDuocPhongVan = _userRepository.GetUserById(InternId).HoVaTen,
                NguoiPhongVan = Interviewer.HoVaTen,
                InterviewForm = request.interviewForm.ToString(),
                ThoiGianPhongVan = request.ThoiGianPhongVan,
                TrangThai = Status.Not_Yet.ToString(),
                TimeDuration = TimeSpan.FromMinutes(request.TimeDuration),
            };
            return lichphongvannew;
        }
        public void deleteSchedudle(string ScheduleId)
        {
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ScheduleId == null)
            {
                throw new BadHttpRequestException("Please, Enter the Id");
            }

            var accountLogin = _userRepository.GetUserById(accountId);
            var schedule = _lichPhongVanRepository.GetScheduleById(ScheduleId);
            if (schedule == null)
            {
                throw new BadHttpRequestException("This schedule is not existed");
            }
            if (accountId != schedule.IdNguoiPhongVan || (accountRole != Roles.ADMIN && (accountRole != Roles.HR.ToUpper())))
            {
                throw new BadHttpRequestException("You don't have permission to delete this schedule");
            }
            _lichPhongVanRepository.DeleteLichPhongVan(schedule);
        }
        public List<User> GetInternWithoutInternView(DateTime startDate, DateTime endDate)
        {
            var listUserWithOutInterview = _userRepository.GetUsersWithoutInterview(startDate, endDate);
            List<User> InternWithoutInterview = new List<User>();
            foreach (var item in listUserWithOutInterview)
            {
                var UserRole = _userManager.GetRolesAsync(item).Result[0];
                if(UserRole == Roles.INTERN)
                {
                    InternWithoutInterview.Add(item);
                }
                else
                {
                    continue;
                }
            }
            return InternWithoutInterview;
        }
        public List<User> GetHrOrMentorWithoutInternView(DateTime startDate, DateTime endDate)
        {
            var listUserWithOutInterview = _userRepository.GetUsersWithoutInterview(startDate, endDate);
            List<User> InternWithoutInterview = new List<User>();
            foreach (var item in listUserWithOutInterview)
            {
                var UserRole = _userManager.GetRolesAsync(item).Result[0];
                if (UserRole == Roles.MENTOR || UserRole == Roles.HR)
                {
                    InternWithoutInterview.Add(item);
                }
                else
                {
                    continue;
                }
            }
            return InternWithoutInterview;
        }

        public void AutoCreateSchedule(DateTime startTime, DateTime endTime, string DiaDiemPhongVan, InterviewForm interviewForm)
        {

            if (startTime > endTime || endTime.Subtract(startTime).TotalMinutes<30)
            {
                throw new BadHttpRequestException("The end time cannot be earlier than the start time and The time from start to end must not be less than 30 minutes.");
            }
            if(startTime.Date != endTime.Date)
            {
                throw new BadHttpRequestException("Start and end times must be the same day");
            }
            if(startTime.TimeOfDay < new TimeSpan(9, 0, 0))
            {
                startTime = startTime.Date.Add(new TimeSpan(9, 0, 0));
            }
            if(endTime.TimeOfDay < new TimeSpan(17,0, 0))
            {
                endTime = endTime.Date.Add(new TimeSpan(17, 0, 0));
            }
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if(accountRole != Roles.ADMIN)
            {
                throw new BadHttpRequestException("Only admin can use this function");
            }
           
            var listHrOrMentorWithoutInterview = GetHrOrMentorWithoutInternView(startTime.AddMinutes(-30), endTime.AddMinutes(30));
            foreach (var item in listHrOrMentorWithoutInterview)
            {
                var startTime2 = startTime;
                var listInternWithoutInterview = GetInternWithoutInternView(startTime,endTime);
                if(listInternWithoutInterview.Count ==0)
                {
                    break;
                }
                foreach (var item1 in listInternWithoutInterview)
                {
                    var lichphongvan = new LichPhongVanRequestModel
                    {
                        HoVaTenNgPhongVan = item.HoVaTen,
                        Email = item1.Email,
                        ThoiGianPhongVan = startTime2,
                        TimeDuration = 15,
                        DiaDiemPhongVan = DiaDiemPhongVan,
                        interviewForm = interviewForm
                    };
                    startTime2 = startTime2.AddMinutes(30);
                    AddLichPhongVan(lichphongvan);
                    if(startTime2.AddMinutes(30) > endTime)
                    {
                        
                        break;
                    }
                }
                
            }
        }
    }
}
