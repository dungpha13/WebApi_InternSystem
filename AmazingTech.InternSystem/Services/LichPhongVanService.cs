﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.Request.LichPhongVan;
using AmazingTech.InternSystem.Models.Response;
using AmazingTech.InternSystem.Models.Response.User;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Repositories.NhomZaloManagement;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace AmazingTech.InternSystem.Services
{
    public interface IGuiLichPhongVanService
    {
        public List<User> GetInternOnly();
        public void AddLichPhongVan(LichPhongVanRequestModel model);

        public List<LichPhongVanResponseModel> getmyInterviewSchedule();
        public LichPhongVanIntervieweeModel GetIntervieweeLoginSchedule();
        public LichPhongVanResponseModel UpdateSchedule(LichPhongVanRequestModel request);
        public void deleteSchedudle(string ScheduleId);
        public bool ConfirmEmail(string id);
        public void SendResultInterviewEmail(string id);
        public List<User> GetInternWithoutInternView();
        public Task<IActionResult> GetAllUserByKetQua(Result ketqua);
        public void UpdateResult(string idlichphongvan, Result result, string Vitri);
        public List<User> GetHrOrMentorWithoutInternView(DateTime startDate, DateTime endDate);
        public void AutoCreateSchedule([EmailAddress] string mailNgPhongVan, DateTime startTime, DateTime endTime, string DiaDiemPhongVan, InterviewForm interviewForm, int timeDuration);

        public List<LichPhongVanResponseModel> AllLichPhongVan();
        public List<LichPhongVanResponseModel> SendListOfInternsToMentor(string email);
        public List<LichPhongVanResponseModel> GetLichPhongVanByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan);
        public List<GetUserDTO> UserWithConsiderResult();
    }
    public class LichPhongVanService : IGuiLichPhongVanService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILichPhongVanRepository _lichPhongVanRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IViTriRepository _viTriRepository;
        private readonly IUserViTriRepository _userViTriRepository;
        private readonly INhomZaloRepo _nhomZaloRepository;
        private readonly IMapper _mapper;
        public LichPhongVanService(ILichPhongVanRepository lichPhongVanRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IEmailService emailService, UserManager<User> userManager, IViTriRepository viTriRepository, IUserViTriRepository userViTriRepository, IMapper mapper, INhomZaloRepo nhomZaloRepo)
        {
            _userManager = userManager;
            _emailService = emailService;
            _userRepository = userRepository;
            _lichPhongVanRepository = lichPhongVanRepository;
            _httpContextAccessor = httpContextAccessor;
            _viTriRepository = viTriRepository;
            _userViTriRepository = userViTriRepository;
            _mapper = mapper;
            _nhomZaloRepository = nhomZaloRepo;
        }
        public bool ConfirmEmail(string id)
        {
            var lichPhongVan = _lichPhongVanRepository.GetScheduleByIntervieweeId(id);
            if (lichPhongVan == null)
            {
                return false;
            }

            var lichphongvan = _lichPhongVanRepository.GetLichPhongVansByIdNgDuocPhongVan(id);

            var lichphongvanrespone = new LichPhongVanIntervieweeModel
            {
                DiaDiemPhongVan = lichphongvan.DiaDiemPhongVan,
                InterviewForm = lichphongvan.InterviewForm.ToString(),
                KetQua = lichphongvan.KetQua.ToString(),
                NguoiDuocPhongVan = _userRepository.GetUserById(lichphongvan.IdNguoiDuocPhongVan).HoVaTen,
                ThoiGianPhongVan = lichphongvan.ThoiGianPhongVan,
                TrangThai = lichphongvan.TrangThai.ToString(),
                NguoiPhongVan = _userRepository.GetUserById(lichphongvan.IdNguoiPhongVan).HoVaTen,
                TimeDuration = lichphongvan.TimeDuration
            };

            var user = _userRepository.GetUserById(id);

            string context = "Kính gửi bạn " + _userRepository.GetUserById(id).HoVaTen + ",<br><br>Đại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi gửi bạn thông báo về buổi phỏng vấn như sau: <br>" +
                "<br>Thời gian phỏng vấn: " + lichphongvanrespone.ThoiGianPhongVan +
                "<br>Khoảng thời gian phỏng vấn dự kiến :" + lichphongvanrespone.TimeDuration +
                "<br> Hình thức phỏng vấn: " + lichphongvanrespone.InterviewForm.ToString() +
                "<br>Địa điểm phỏng vấn: " + lichphongvanrespone.DiaDiemPhongVan +
                "<br><br>Xin vui lòng cho chúng tôi  biết nếu có bất kỳ điều gì cần được điều chỉnh hoặc có bất kỳ thông tin nào khác chúng tôi cần cung cấp.<br><br>Trân trọng";

            string subject = "[AMAZINGTECH - HR] THƯ THÔNG BÁO LỊCH PHỎNG VẤN";

            _emailService.SendInterviewScheduleMail(context, user.Email, subject);

            lichPhongVan.DaXacNhanMail = true;
            _lichPhongVanRepository.UpdateLichPhongVan(lichPhongVan);

            return true;
        }

        public void SendResultInterviewEmail(string id)
        {
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to create an interview schedule");
            }

            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new BadHttpRequestException("This Mail is not exist in database");
            }

            if (!(accountRole.Equals(Roles.HR.ToUpper()) || accountRole.Equals(Roles.ADMIN))) // không phải là HR hay Admin thì không lịch đc tạo 
            {
                throw new BadHttpRequestException("You don't have permission to send email");
            }

            var resultIntern = _lichPhongVanRepository.GetScheduleByIntervieweeId(id);
            string email = user.Email;

            if(resultIntern == null)
            {
                throw new BadHttpRequestException("This intern has been not interviewed yet");
            }

            if (resultIntern.KetQua == Result.Consider)
            {
                throw new BadHttpRequestException("Intern with Consider Result cannot be received result interview email");
            }
            string resultContext = "";
            string context = "";

            var userNhomZalo = _nhomZaloRepository.GetUserNhomZaloByUserId(id);

            if(userNhomZalo == null)
            {
                throw new BadHttpRequestException("This Intern is not been add ZaloGroup");
            }

            var nhomZaloRiengLink = _nhomZaloRepository.GetNhomZaloById(userNhomZalo.IdNhomZaloRieng).LinkNhom;
            var nhomZaloChungLink = _nhomZaloRepository.GetNhomZaloById(userNhomZalo.IdNhomZaloChung).LinkNhom;

            if(nhomZaloChungLink == null)
            {
                throw new BadHttpRequestException("This intern does not have NhomZaloChung");
            }

            if (nhomZaloRiengLink == null)
            {
                throw new BadHttpRequestException("This intern does not have NhomZaloRieng");
            }

            if (resultIntern.KetQua == 0)
            {
                resultContext = "<span style = \"color:red; text-decoration:underline; font-weight: bold\">CHƯA ĐẠT YÊU CẦU THAM GIA THỰC TẬP</span>";

                context = "<br>Chào bạn ứng viên,<br><br>Đại diện bộ phận Nhân sự (HR) tại<span style = \"font-weight: bold\"> Công Ty TNHH Giải Pháp và Công nghệ Amazing</span>. Trước hết chúng tôi chân thành cảm ơn vì sự quan tâm và tham gia tích cực của bạn trong buổi phỏng vấn vị trí thực tập tại Công ty chúng tôi<br>" +
                          "<br>Sau quá trình xem xét kỹ lưỡng, chúng tôi rất tiếc phải thông báo rằng<span style = \"font-weight: bold; font-style: italic\"> kết quả sau buổi phỏng vấn của bạn là</span>: " + resultContext + "<br>" +
                          "<br>Đây là một quyết định khó khăn, chúng tôi hiểu rằng có thể nó sẽ mang đến sự thất vọng. Cảm ơn bạn đã dành thời gian và nổ lực cho quá trình phỏng vấn đối với <span style=\"font-weight: bold\">Công Ty TNHH Giải Pháp và Công Nghệ Amazing</span><br>" +
                          "<br>Chúng tôi mong rằng bạn sẽ tìm được cơ hội phát triển tốt nhất cho sự nghiệp của mình trong tương lai.<br>" +
                          "<br>Trân trọng";
            }
            else
            {
                resultContext = "<span style = \"color:green; text-decoration:underline; font-weight: bold\">ĐẠT YÊU CẦU THAM GIA THỰC TẬP</span>";

                context = "Chào <span style = \"font-weight: bold\">bạn ứng viên</span>,<br><br>Đại diện bộ phận Nhân sự (HR) tại <span style = \"font-weight: bold\">Công Ty TNHH Giải Pháp và Công nghệ Amazing</span>, chúng tôi rất hân hạnh khi bạn đã quyết định tham gia chương trình thực tập tại Công ty của chúng tôi.<br>" +
                    "<br>Đây là kết quả sau buổi phỏng vấn của bạn: " + resultContext + "<br>" +
                    "<br><h3 style = \"font-weight: bold\">A. THÔNG TIN VỀ KỲ THỰC TẬP:</h3>" +
                    "&nbsp;&nbsp;&nbsp; <span style = \"font-weight: bold\">1. Hình thức thực tập:</span> Linh động giữa Online và Offline<br>" +
                    "&nbsp;&nbsp;&nbsp; <span style = \"font-weight: bold\">2. Thời gian thực tập:</span> Giờ hành chính (Thứ 2 - thứ 6, từ 8h00 - 17h00).<br>" +
                    "&nbsp;&nbsp;&nbsp; <span style = \"font-weight: bold\">3. Group Zalo tham gia vào thực tập</span>: <br>" +
                    $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &#x2022; <a href=\"{nhomZaloChungLink}\">Link Nhóm Zalo Chung</a>" +
                    $"<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &#x2022; <a href=\"{nhomZaloRiengLink}\">Link Nhóm Zalo Chuyên Ngành</a>" +
                    "<br>&nbsp;&nbsp;&nbsp; <span style = \"font-weight: bold\">4. Hỗ trợ:</span> <br>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &#x2022; Chấm điểm gửi báo cáo hàng tuần (nếu trường yêu cầu) <br>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &#x2022; Đóng mộc xác nhận cuối kỳ thực tập <br>" +
                    "<br><span style=\"font-style: italic;font-weight: bold; text-decoration: underline;\">Lưu ý</span>: <span style=\"font-style: italic;\">Chúng tôi muốn bạn biết rằng vị trí bạn mong muốn ban đầu và vị trí bạn thực tập không cố định, có thể được điều chỉnh dựa trên năng lực của bạn và yêu cầu dự án của công ty trong quá trình thực tập. </span><br>" +
                    "<br>Chúng tôi tin rằng bạn sẽ mang đến một giá trị đặc biệt cho công ty của chúng tôi và hy vọng rằng bạn sẽ có một trải nghiệm thực tập thú vị, bổ ích tại <span style = \"font-weight: bold\">Amazing Tech</span>.<br>" +
                    "<br>Trân trọng.<br>" +
                    "<div dir=\"ltr\" class=\"gmail_signature\" data-smartmail=\"gmail_signature\"><div dir=\"ltr\"><div style=\"color:rgb(34,34,34)\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"background:transparent;font-size:14px;border-spacing:0px;border-collapse:collapse;font-family:Arial,sans-serif;color:rgb(46,154,164);max-width:500px\"><tbody><tr><td colspan=\"2\" width=\"500\" style=\"padding:0px 0px 4px;border-bottom:1px solid rgb(47,154,163)\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:18pt\"><span style=\"font-weight:700\"><font color=\"#0b5394\">--<br>Amazing Tech</font></span></span><br></font><span style=\"color:rgb(58,67,69);font-family:Tahoma,sans-serif;font-size:13.3333px;font-weight:700\">Software Development Company</span>&nbsp;&nbsp;<font face=\"tahoma, sans-serif\"><span style=\"font-size:10pt\">&nbsp;&nbsp;<font color=\"#6fa8dc\">&nbsp;</font><font color=\"#0b5394\">|</font>&nbsp;</span><a href=\"http://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(17,85,204);background-color:transparent;font-size:10pt;font-weight:bold\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=http://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw1vaYI2qEOK2T13SAzEBs2K\"><span style=\"font-size:10pt\"><font color=\"#0b5394\">amazingtech.vn</font></span></a></font></td></tr><tr><td width=\"120\" style=\"padding:22px 0px 0px\"><p style=\"margin:0px;padding:0px\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Photo\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NYIQZghSrU6IZdMz9suNlE9PjPuyZ1EdHCVmzj1vU2jZHotFhiSOIh7oGk9mHRBWejqWtG8D56PF6Q3q59IV4nwG1DVQ2MRcXQl_NltCnkIt-h6k3jG4aAtPnP9QkKcWec1tfc=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/logo-amazing-tech-v2.png\" style=\"border:0px;vertical-align:middle;max-width:90px\" class=\"CToWUd\" data-bit=\"iit\"></font></p></td><td style=\"padding:25px 0px 0px\"><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">T:</span>&nbsp;</font>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(028) 9999 - 7939</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">M:</font></span>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(+84) 888 181 100</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">E:</font></span><font color=\"#3d85c6\">&nbsp;</font><a href=\"mailto:amazingtech.hr@gmail.com\" style=\"color:rgb(16,16,16);background-color:transparent;font-size:9pt;line-height:10pt\" target=\"_blank\"><span style=\"font-size:9pt;line-height:10pt\">amazingtech.hr@gmail.com</span></a></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">A:</span>&nbsp;</font><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">74 Nguyễn Cửu Đàm, Phường Tân Sơn Nhì, Quận Tân Phú, TP.HCM</span></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\"><font face=\"tahoma, sans-serif\">&nbsp; &nbsp; &nbsp;S9.02A Vinhome Grand Park, Nguyễn Xiển, Phường Phước Thiện, Quận Thủ Đức, TP.HCM</font></span></p><p style=\"margin:4px 0px 0px;padding-bottom:0px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><a href=\"https://www.facebook.com/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0PsTtrffG8GO6TFt1mZkDe\"><img border=\"0\" width=\"20\" alt=\"facebook icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Nb-H8iXOIGiIIOjCq60PcYJdwAiL3JI9ca8Zvfz7PzPeRO7EP5yLfmQlXiaR8gk8shP-__vXefTiEmbGIHRo-lRY8PhPrnocnAIhlpItBw65ruLKdU7nElsW1pxywyN9YiYA_O7zIbih2al=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/fb.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.youtube.com/@amazingtechvietnam\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.youtube.com/@amazingtechvietnam&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0Zu2_6T3U_WB0AOHMaEeUq\"><img border=\"0\" width=\"20\" alt=\"youtube icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NaAgTDtnkTEwIcx8mxmO4-t06QPEl5sXLKF7gnOvFVQ8c31ca2W8BQmyAIlOBlSAjvOrXh_A452L86DfYtVK0dug2BbivN6r8XOyEEjnuvCDGtihKh_bNird28dMxannXIagpI_NkDRXrF4=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/yt.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.linkedin.com/company/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.linkedin.com/company/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw2Tb7NjZTwhK_taupiJr9iw\"><img border=\"0\" width=\"20\" alt=\"linkedin icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Na22lJt_btcmFPUMmTXijHqUbMnhZF2iNtgvomlgswJg-7vB888Zz5mYQcCEZUJa3yv8bugefIkEYkRQaxxeNYbBGXdytU5BAgd5UyfBRUSkr4Whm9q6UjET04sVtiekVBDhGzB-6L_JUwn=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/ln.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;</font></p></td></tr><tr><td colspan=\"2\" style=\"padding:25px 0px 0px\"><a href=\"https://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw11h_SNDADPSAg0WiKKnBHk\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Banner\" width=\"527\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NZT7FwFWtXD-LTbufKOqpFfhg4b1eEPlZOVdgqqEYnGh-4T6jGP9ZYxGLs3OM5QSw2rwPVXTRBWooBDxsi466XySJNe9bvDYVVSqpql6UIqKEITfpN3U68wh91TZUyh=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/icon-1-1024x761.png\" height=\"392\" style=\"border:0px;vertical-align:middle;margin-right:0px\" class=\"CToWUd\" data-bit=\"iit\"></font></a></td></tr></tbody></table></div></div></div>";
            }



            string subject = "[AMAZINGTECH - HR] THƯ THÔNG BÁO KẾT QUẢ PHỎNG VẤN";
            _emailService.SendResultInterviewEmail(context, email, subject);

        }
        public void AddLichPhongVan(LichPhongVanRequestModel model)
        {
            if (model.ThoiGianPhongVan < DateTime.Now)
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
            if (model.ThoiGianPhongVan == null || model.DiaDiemPhongVan.Length == 0 || model.Email == null || TimeSpan.FromMinutes(model.TimeDuration) <= new TimeSpan(0, 0, 0) || model.MailNgPhongVan == null || model.interviewForm == null)
            {
                throw new BadHttpRequestException("You need to fill all information");
            }
            if (model.ThoiGianPhongVan.Minute % 15 != 0)
            {
                throw new BadHttpRequestException("The number of minutes of interview time must be a multiple of 15");
            }
            if (!Enum.IsDefined(typeof(InterviewForm), model.interviewForm))
            {
                throw new BadHttpRequestException("This InterviewForm is invalid");
            }
            if (model.TimeDuration % 15 != 0)
            {
                throw new BadHttpRequestException("Interview time must be a multiple of 15");
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
            var InternRole = _userManager.GetRolesAsync(Intern).Result[0];

            if (InternRole.ToUpper() != Roles.INTERN.ToUpper())
            {
                throw new BadHttpRequestException("This interviewee isn't intern so we can't create schedule");
            }
            var ScheduleisExist = _lichPhongVanRepository.GetScheduleByIntervieweeId(InternId);
            if (ScheduleisExist != null)
            {
                throw new BadHttpRequestException("This intern already has interview schedule");
            }

            var Interviewer = _userRepository.GetUserByEmail(model.MailNgPhongVan);
            if (Interviewer == null)
            {
                throw new BadHttpRequestException("Can't find this interviewer, please write her/his mail correctly");
            }
            var timeEnd = model.ThoiGianPhongVan + TimeSpan.FromMinutes(model.TimeDuration);
            timeEnd = timeEnd.AddMinutes(15);
            var ScheduleOfInterviewerExist = _lichPhongVanRepository.GetScheduleOfInterviewerInPeriodTime(Interviewer.Id, model.ThoiGianPhongVan, timeEnd);
            if (ScheduleOfInterviewerExist.Count != 0)
            {
                throw new BadHttpRequestException("This interviewer has an interview scheduled from :" + model.ThoiGianPhongVan + "to" + timeEnd);
            }
            if (!(accountRole.Equals(Roles.HR.ToUpper()) || accountRole.Equals(Roles.ADMIN))) // không phải là HR hay Admin thì không lịch đc tạo 
            {
                throw new BadHttpRequestException("You don't have permission to create schedule");
            }
            var InterViewListRole = _userManager.GetRolesAsync(Interviewer).Result[0];

            // Người Phỏng Vấn phải là HR hoặc Mentor


            if (!(InterViewListRole.ToUpper() == Roles.HR.ToUpper() || InterViewListRole.ToUpper() == Roles.MENTOR.ToUpper()))
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
            string context = "Gửi bạn ứng viên,<br><br>Đại diện bộ phận Nhân sự (HR) tại <span style= \"font-weight: bold\">Công Ty TNHH Giải Pháp và Công nghệ Amazing</span>, chúng tôi xin chân thành ghi nhận sự quan tâm của bạn đối với cơ hội thực tập tại Công ty chúng tôi.<br>" +
                "<br>Chúng tôi muốn mời bạn tham gia phỏng vấn để tìm hiểu và xem xét sự phù hợp của bạn với vị trí bạn muốn ứng tuyển tại công ty chúng tôi.<br>" +
                "<br>Vui lòng xác nhận phỏng vấn bằng link dưới đây để chúng tôi có thể gửi lịch phỏng vấn cho bạn trên website: ";

            string subject = "[AMAZINGTECH - HR] THƯ GHI NHẬN THÔNG TIN THỰC TẬP SINH";
            _emailService.SendMail(context, model.Email, subject, InternId);
        }
        public List<LichPhongVanResponseModel> getmyInterviewSchedule()
        {
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //string accountId = "1";
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to get an interview schedule");
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

        public LichPhongVanIntervieweeModel GetIntervieweeLoginSchedule()
        {
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //string accountId = "1";
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to get an interview schedule");
            }

            var lichphongvan = _lichPhongVanRepository.GetLichPhongVansByIdNgDuocPhongVan(accountId);

            if (lichphongvan == null)
                return null;

            if (lichphongvan.DaXacNhanMail == false)
            {
                throw new BadHttpRequestException("You need to confirm interview link in email to get schedule");
            }
            var lichphongvanrespone = new LichPhongVanIntervieweeModel
            {
                DiaDiemPhongVan = lichphongvan.DiaDiemPhongVan,
                InterviewForm = lichphongvan.InterviewForm.ToString(),
                KetQua = lichphongvan.KetQua.ToString(),
                NguoiDuocPhongVan = _userRepository.GetUserById(lichphongvan.IdNguoiDuocPhongVan).HoVaTen,
                ThoiGianPhongVan = lichphongvan.ThoiGianPhongVan,
                TrangThai = lichphongvan.TrangThai.ToString(),
                NguoiPhongVan = _userRepository.GetUserById(lichphongvan.IdNguoiPhongVan).HoVaTen,
                TimeDuration = lichphongvan.TimeDuration
            };

            return lichphongvanrespone;
        }
        public LichPhongVanResponseModel UpdateSchedule(LichPhongVanRequestModel request)
        {
            if (request.ThoiGianPhongVan < DateTime.Now)
            {
                throw new BadHttpRequestException("The date of interview must be in the future");
            }
            if (request.TimeDuration % 15 != 0)
            {
                throw new BadHttpRequestException("Interview time must be a multiple of 15");
            }
            if (request.ThoiGianPhongVan.Minute % 15 != 0)
            {
                throw new BadHttpRequestException("The number of minutes of interview time must be a multiple of 15");
            }
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (request.ThoiGianPhongVan.TimeOfDay > new TimeSpan(17, 0, 0) || request.ThoiGianPhongVan.TimeOfDay < new TimeSpan(9, 0, 0))
            {
                throw new BadHttpRequestException("Interview time is from 9:00 a.m. to 5:00 p.m");
            }
            if (!Enum.IsDefined(typeof(InterviewForm), request.interviewForm))
            {
                throw new BadHttpRequestException("This InterviewForm is invalid");
            }
            var Interviewer = _userRepository.GetUserByEmail(request.MailNgPhongVan);
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var accountLogin = _userRepository.GetUserById(accountId);
            string InternId = _userRepository.GetUserIdByEmail(request.Email);
            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to update schedule");
            }
            if (InternId == null)
            {
                throw new BadHttpRequestException("This intern doesn't exist in data");
            }
            var Intern = _userRepository.GetUserById(InternId);
            var InternRole = _userManager.GetRolesAsync(Intern).Result[0];

            if (InternRole.ToUpper() != Roles.INTERN.ToUpper())
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
                throw new BadHttpRequestException("Can't find this interviewer, please write her/his mail correctly");
            }
            int count = 0;
            var InterViewListRole = _userManager.GetRolesAsync(Interviewer).Result[0];
            if (!(InterViewListRole.ToUpper() == Roles.HR.ToUpper() || InterViewListRole.ToUpper() == Roles.MENTOR.ToUpper()))
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
            string content = "Kính gửi bạn " + _userRepository.GetUserById(InternId).HoVaTen + ",<br>Đại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi xin chân thành xin lỗi khi phải thông báo về việc dời lại lịch phỏng vấn. <br>Đây là lịch phỏng vấn mới của bạn " +
                request.ThoiGianPhongVan +
                "<br>Khoảng thời gian phỏng vấn dự kiến :" + request.TimeDuration +
                "<br> Hình thức phỏng vấn: " + request.interviewForm.ToString() + "<br>Địa điểm phỏng vấn: " +
                request.DiaDiemPhongVan +
                "<br>Xin cảm ơn sự hiểu biết và sự linh hoạt của bạn trong việc xem xét yêu cầu của tôi. Xin vui lòng cho chúng tôi  biết nếu có bất kỳ điều gì cần được điều chỉnh hoặc có bất kỳ thông tin nào khác chúng tôi cần cung cấp.\r\n\r\nTrân trọng";
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
            if (accountId != schedule.IdNguoiPhongVan && accountRole == Roles.HR.ToUpper())
            {

                throw new BadHttpRequestException("You don't have permission to delete this schedule");
            }
            if (!(accountRole == Roles.HR.ToUpper() || accountRole == Roles.ADMIN))
            {
                throw new BadHttpRequestException("You don't have permission to delete this schedule");
            }
            schedule.DeletedBy = accountLogin.HoVaTen;
            schedule.DeletedTime = DateTime.Now;
            _lichPhongVanRepository.UpdateLichPhongVan(schedule);
        }
        public List<User> GetInternWithoutInternView()
        {
            var listUserWithOutInterview = _userRepository.GetInternsWithoutInterview();
            var InternWithoutInterview = listUserWithOutInterview.Where(listUserWithOutInterview => _userManager.IsInRoleAsync(listUserWithOutInterview, Roles.INTERN).Result).ToList();
            return InternWithoutInterview;
        }
        public List<User> GetHrOrMentorWithoutInternView(DateTime startDate, DateTime endDate)
        {
            var listUserWithOutInterview = _userRepository.GetHrOrMentorWithoutInterview(startDate, endDate);
            List<User> HrOrMentorWithoutInterview = new List<User>();
            var HrList = listUserWithOutInterview
               .Where(user => _userManager.IsInRoleAsync(user, Roles.HR).Result || _userManager.IsInRoleAsync(user, Roles.MENTOR).Result)
               .ToList();
            return HrOrMentorWithoutInterview;
        }
        public void AutoCreateSchedule([EmailAddress] string mailNgPhongVan, DateTime startTime, DateTime endTime, string DiaDiemPhongVan, InterviewForm interviewForm, int timeDuration)
        {

            if (startTime > endTime || endTime.Subtract(startTime).TotalMinutes < timeDuration)
            {
                throw new BadHttpRequestException("The end time cannot be earlier than the start time and The time from start to end must not be less than 30 minutes.");
            }
            if (timeDuration % 15 != 0)
            {
                throw new BadHttpRequestException("Interview time must be a multiple of 15");
            }
            if (startTime.Minute % 15 != 0)
            {
                throw new BadHttpRequestException("The number of minutes of interview time must be a multiple of 15");
            }
            //if (startTime.Date != endTime.Date)
            //{
            //    throw new BadHttpRequestException("Start and end times must be the same day");
            //}
            if (startTime.TimeOfDay < new TimeSpan(9, 0, 0))
            {
                startTime = startTime.Date.Add(new TimeSpan(9, 0, 0));
            }
            //if (endTime.TimeOfDay > new TimeSpan(17, 0, 0))
            //{
            //    endTime = endTime.Date.Add(new TimeSpan(17, 0, 0));
            //}
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (accountRole != Roles.ADMIN)
            {
                throw new BadHttpRequestException("Only admin can use this function");
            }
            var listInternWithoutInterview = GetInternWithoutInternView();
            for (int i = listInternWithoutInterview.Count - 1; i >= 0; i--)
            {
                var item = listInternWithoutInterview[i];
                var lichphongvan = _lichPhongVanRepository.GetScheduleByIntervieweeId(item.Id);
                if (lichphongvan != null)
                {
                    listInternWithoutInterview.RemoveAt(i);
                }
            }
            if (listInternWithoutInterview.Count == 0)
            {
                throw new BadHttpRequestException("All interns have an interview schedule");
            }

            foreach (var item1 in listInternWithoutInterview)
            {
                var lichphongvan = new LichPhongVanRequestModel
                {
                    MailNgPhongVan = mailNgPhongVan,
                    Email = item1.Email,
                    ThoiGianPhongVan = startTime,
                    TimeDuration = timeDuration,
                    DiaDiemPhongVan = DiaDiemPhongVan,
                    interviewForm = interviewForm,
                };
                startTime = startTime + TimeSpan.FromMinutes(timeDuration);
                AddLichPhongVan(lichphongvan);
                if (startTime >= endTime)
                {
                    break;
                }
            }
        }

        public List<LichPhongVanResponseModel> AllLichPhongVan()
        {
            /*List<LichPhongVan> lichPhongVans = _lichPhongVanRepository.GetAllLichPhongVan();
            return new ObjectResult(lichPhongVans);*/

            /*List<LichPhongVan> lichPhongVans = _lichPhongVanRepository.GetAllLichPhongVan();

            List<LichPhongVanResponseModel> lichPhongVanResponseModels = lichPhongVans.Select(item => new LichPhongVanResponseModel
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


            }
                ).ToList();
            return lichPhongVanResponseModels;*/



            var lichphongvan = _lichPhongVanRepository.GetAllLichPhongVan();
            var lichphongvanlist = new List<LichPhongVanResponseModel>();
            foreach (var item in lichphongvan)
            {
                var lichphongvanrepone = new LichPhongVanResponseModel
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
                lichphongvanlist.Add(lichphongvanrepone);
            }
            return lichphongvanlist;


        }



        public List<LichPhongVanResponseModel> SendListOfInternsToMentor(string email)
        {
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (accountId == null)
            {
                throw new BadHttpRequestException("You need to login to send list of interns to Mentor");
            }

            if (email == null)
            {
                throw new BadHttpRequestException("You need to fill all information");
            }

            var mentorId = _userRepository.GetUserIdByEmail(email);
            var hr = _userRepository.GetUserById(accountId);

            if (mentorId == null)
            {
                throw new BadHttpRequestException("This Mail is not exist in database");
            }

            var hrRole = _userManager.GetRolesAsync(hr).Result[0];

            if (!(accountRole.Equals(Roles.HR.ToUpper()) || accountRole.Equals(Roles.ADMIN)))
            {
                throw new BadHttpRequestException("Your role must be HR or Admin to do this");
            }

            var lichphongvan = _lichPhongVanRepository.GetLichPhongVansByIdNgPhongVan(mentorId);

            if (lichphongvan.Count == 0)
            {
                throw new BadHttpRequestException("This mentor has no interview schedule");
            }

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

        public List<LichPhongVanResponseModel> GetLichPhongVanByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan)
        {
            var lichPhongVan = _lichPhongVanRepository.GetLichPhongVanByIdNguoiDuocPhongVan(idNguoiDuocPhongVan);
            var lichPhongVanList = new List<LichPhongVanResponseModel>();
            foreach (var item in lichPhongVan)
            {
                var lichphongvanrepone = new LichPhongVanResponseModel
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
                lichPhongVanList.Add(lichphongvanrepone);
            }
            return lichPhongVanList;

        }
        public List<User> GetInternOnly()
        {
            //var internUsers = _userManager.GetUsersInRoleAsync("Intern").Result;
            //if (internUsers == null)
            //{
            //    throw new Exception("Failed to retrieve intern users.");
            //}
            //return internUsers.ToList();
            var user = _userRepository.GetInternsWithoutInterview();
            var intern = user.Where(user => _userManager.IsInRoleAsync(user, Roles.INTERN).Result).ToList();
            return intern;
        }
        public void UpdateResult(string idlichphongvan, Result result, string Vitri)
        {
            if (idlichphongvan == null || result == null || Vitri == null)
            {
                throw new BadHttpRequestException("You need to fill all information");
            }
            var schedule = _lichPhongVanRepository.GetScheduleById(idlichphongvan);
            if (schedule == null)
            {
                throw new BadHttpRequestException("Wrong Id");
            }
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string accountId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!(accountRole == Roles.ADMIN || accountRole == Roles.HR.ToUpper()))
            {
                throw new BadHttpRequestException("You don't have permission to do this function");
            }
            if (accountRole == Roles.HR.ToUpper() && accountId != schedule.IdNguoiPhongVan)
            {
                throw new BadHttpRequestException("You don't have permission to do this function");
            }
            schedule.KetQua = result;
            schedule.LastUpdatedBy = _userRepository.GetUserById(accountId).HoVaTen;
            schedule.LastUpdatedTime = DateTime.Now;
            schedule.TrangThai = Status.Done;
            var intern = _userRepository.GetUserById(schedule.IdNguoiDuocPhongVan);
            var vitriCheck = _viTriRepository.GetViTriByName(Vitri);
            if (vitriCheck == null)
            {
                throw new BadHttpRequestException("This position does not exist");
            }
            var UserViTri = new UserViTri()
            {
                UsersId = schedule.IdNguoiDuocPhongVan,
                ViTrisId = vitriCheck.Id
            };
            _userViTriRepository.AddUserViTriRepository(UserViTri);
            _lichPhongVanRepository.UpdateLichPhongVan(schedule);

        }

        public async Task<IActionResult> GetAllUserByKetQua(Result ketqua)
        {
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (!(accountRole == Roles.HR || accountRole == Roles.MENTOR))
            {
                throw new BadHttpRequestException("You don't have permission to do this function");
            }
            var lichphongvan = _lichPhongVanRepository.GetLichPhongVanByKetQua(ketqua);

            List<User> resultUserList = new List<User>();

            for (int i = 0; i < lichphongvan.Count; i++)
            {
                var user = await _userManager.Users.Where(x => x.Id == lichphongvan[i].IdNguoiDuocPhongVan && x.DeletedTime == null).SingleOrDefaultAsync();
                resultUserList.Add(user);
            }

            var mappedUserList = _mapper.Map<List<GetUserDTO>>(resultUserList);

            var result = new List<GetUserDTO>();
            foreach (var user in mappedUserList)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(identityUser);
                user.Roles = roles.ToList();
                result.Add(user);
            }

            return new OkObjectResult(result);
        }
        public List<GetUserDTO> UserWithConsiderResult()
        {
            string accountRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (!(accountRole.ToUpper() == Roles.HR || accountRole == Roles.MENTOR  || accountRole == Roles.ADMIN))
            {
                throw new BadHttpRequestException("You don't have permission to do this function");
            }
            var lichphongvan = _lichPhongVanRepository.GetLichPhongVanByKetQua(Result.Consider);
            var ListUser = new List<User>();
            foreach (var item in lichphongvan)
            {
                var user = _userRepository.GetUserById(item.IdNguoiDuocPhongVan);
                ListUser.Add(user);
            }
            var ListUserResponse = new List<GetUserDTO>();
            foreach (var item in ListUser)
            {
                var UserReponse = new GetUserDTO
                {
                    Id = item.Id,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    TrangThaiThucTap = item.TrangThaiThucTap,
                    Username = item.UserName,
                };
                ListUserResponse.Add(UserReponse);
            }
            return ListUserResponse;
        } 
    }
}