using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Models.Request.Authenticate;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Services
{
    public class InternInfoService : IInternInfoService
    {
        private readonly IInternInfoRepo _internRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IKiThucTapRepository _kiThucTapRepository;

        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InternInfoService(IInternInfoRepo internRepo,
            ICommentRepository commentRepo,
            IMapper mapper, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IKiThucTapRepository kiThucTapRepository,
            AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _internRepo = internRepo;
            _commentRepo = commentRepo;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _kiThucTapRepository = kiThucTapRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        //Get all Intern
        public async Task<IActionResult> GetAllInternInfo()
        {
            List<InternInfo> interns = await _internRepo.GetAllInternsInfoAsync();
            return new OkObjectResult(_mapper.Map<List<InternInfoDTO>>(interns));
        }

        //Get all Deleted Intern
        public async Task<IActionResult> GetAllDeletedInternInfo()
        {
            List<InternInfo> interns = await _internRepo.GetAllDeletedInternsInfoAsync();
            return new OkObjectResult(_mapper.Map<List<InternInfoDTO>>(interns));
        }


        //Get Intern by MSSV
        public async Task<IActionResult> GetInternInfo(string mssv)
        {
            InternInfo intern = await _internRepo.GetInternInfoAsync(mssv);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Intern với mssv: '{mssv}' không tìm thấy hoặc đã bị xóa!");
            }
            return new OkObjectResult(_mapper.Map<InternInfoDTO>(intern));
        }

        //Get Intern by MSSV
        public async Task<IActionResult> GetDeletedInternInfo(string mssv)
        {
            InternInfo intern = await _internRepo.GetDeletedInternInfoAsync(mssv);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Intern với mssv: '{mssv}' không tìm thấy!");
            }
            return new OkObjectResult(_mapper.Map<InternInfoDTO>(intern));
        }

        //Add new Intern
        public async Task<IActionResult> AddInternInfo(string user, AddInternInfoDTO model)
        {
            var entity = _mapper.Map<InternInfo>(model);


            var existIntern = await _internRepo.GetInternInfoAsync(entity.MSSV);
            if (existIntern != null)
            {
                return new BadRequestObjectResult("MSSV đã tồn tại trong danh sách!");
            }

            List<InternInfo> interns = await _internRepo.GetAllInternsInfoAsync();
            if (interns.Any(intern => intern.Sdt == model.Sdt))
            {
                return new BadRequestObjectResult("Sđt này đã được sử dụng!");
            }

            if (interns.Any(intern => intern.EmailCaNhan == model.EmailCaNhan))
            {
                return new BadRequestObjectResult("EmailCaNhan này đã được sử dụng!");
            }

            if (interns.Any(intern => intern.EmailTruong == model.EmailTruong))
            {
                return new BadRequestObjectResult("EmailTruong này đã được sử dụng!");
            }



            //Check input IdTruong
            var isTruongHocExist = await _dbContext.TruongHocs.AnyAsync(th => th.Id == model.IdTruong);

            if (!isTruongHocExist)
            {
                return new BadRequestObjectResult($"Trường học với id '{model.IdTruong}' không tồn tại!");
            }


            //// Tao tai khoan cho Intern
            //var account = new RegisterUserRequestDTO
            //{
            //    HoVaTen = entity.HoTen,
            //    Username = entity.EmailTruong,
            //    Email = entity.EmailCaNhan,
            //    Password = "0123456789ooo",
            //    PhoneNumber = entity.Sdt,
            //};
            //string userId = await RegisterIntern(account);

            //if (userId == null)
            //{
            //    return new BadRequestObjectResult("Tên tài khoản không hợp lệ (Check lại gmail đăng ký)");
            //}


            //entity.UserId = userId;

            //Add UserViTri
            //foreach (var viTriId in model.ViTrisId)
            //{
            //    var userViTri = new UserViTri
            //    {
            //        UsersId = userId,
            //        ViTrisId = viTriId
            //    };

            //    _dbContext.UserViTris.Add(userViTri);
            //}


            ////Add NhomZalo
            //foreach (var nhomZaloId in model.IdNhomZalo)
            //{
            //    var userNhomZalo = new UserNhomZalo
            //    {
            //        UserId = userId,
            //        IdNhomZalo = nhomZaloId
            //    };

            //    _dbContext.UserNhomZalos.Add(userNhomZalo);
            //}


            ////Add UserDuAn
            //foreach (var duAnId in model.IdDuAn)
            //{
            //    var userDuAn = new UserDuAn
            //    {
            //        UserId = userId!,
            //        IdDuAn = duAnId
            //    };

            //    _dbContext.InternDuAns.Add(userDuAn);
            //}

            //Add InternInfo
            int rs = await _internRepo.AddInternInfoAsync(user, entity);

            if (rs == 0)
            {
                return new BadRequestObjectResult("Thêm sinh viên thất bại!");
            }

            return new OkObjectResult($"Thêm mới sinh viên với MSSV '{model.MSSV}' thành công!");
        }

        //Delete Intern
        public async Task<IActionResult> DeleteInternInfo(string userId, string mssv)
        {
            var intern = await _dbContext.InternInfos!.FirstOrDefaultAsync(i => i.MSSV == mssv && i.DeletedBy == null);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Intern với mssv: '{mssv}' không tồn tại hoặc đã bị xóa trước đó!");
            }

            int rs = await _internRepo.DeleteInternInfoAsync(userId, intern);

            if (rs == 0)
            {
                return new BadRequestObjectResult($"Intern mssv: '{mssv}' đã được xóa!");
            }

            return new OkObjectResult($"Xóa thành công Intern mssv: '{mssv}' !");
        }

        //Update Intern
        public async Task<IActionResult> UpdateInternInfo(string user, UpdateInternInfoDTO model, string mssv)
        {

            var intern = await _dbContext.InternInfos.FirstOrDefaultAsync(x => x.MSSV == mssv && x.DeletedBy == null);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Intern với MSSV: '{mssv}' không tồn tại!");
            }

            List<InternInfo> interns = await _internRepo.GetAllInternsInfoAsync();
            if (interns.Any(intern => intern.Sdt == model.Sdt && intern.MSSV != mssv))
            {
                return new BadRequestObjectResult("Sđt này đã có người sử dụng!");
            }
            if (interns.Any(intern => intern.EmailCaNhan == model.EmailCaNhan && intern.MSSV != mssv))
            {
                return new BadRequestObjectResult("EmailCaNhan này đã có người sử dụng!");
            }


            ////Update UserViTri
            //var existUserViTri = await _dbContext.UserViTris
            //       .Where(uv => uv.UsersId == intern.UserId)
            //       .ToListAsync();
            //_dbContext.UserViTris.RemoveRange(existUserViTri);

            //foreach (var viTriId in model.ViTrisId)
            //{
            //    // Kiểm tra nếu viTriId tồn tại trong CSDL
            //    var isViTriExist = await _dbContext.ViTris.AnyAsync(vt => vt.Id == viTriId);

            //    if (isViTriExist)
            //    {

            //        var userViTri = new UserViTri
            //        {
            //            UsersId = intern.UserId!,
            //            ViTrisId = viTriId
            //        };

            //        _dbContext.UserViTris.Add(userViTri);
            //    }
            //    else
            //    {
            //        return new BadRequestObjectResult($"Vị trí với id: '{viTriId}' không tồn tại!");
            //    }
            //}


            ////Update UserNhomZalo
            //var existUserNhomZalo = await _dbContext.UserNhomZalos
            //      .Where(unz => unz.UserId == intern.UserId)
            //      .ToListAsync();
            //_dbContext.UserNhomZalos.RemoveRange(existUserNhomZalo);

            //foreach (var nhomZaloId in model.IdNhomZalo)
            //{
            //    var isNhomZaloExist = await _dbContext.NhomZalos.AnyAsync(nz => nz.Id == nhomZaloId);

            //    if (isNhomZaloExist)
            //    {
            //        var userNhomZalo = new UserNhomZalo
            //        {
            //            UserId = intern.UserId!,
            //            IdNhomZalo = nhomZaloId
            //        };

            //        _dbContext.UserNhomZalos.Add(userNhomZalo);
            //    }
            //    else
            //    {
            //        return new BadRequestObjectResult($"Nhóm Zalo với id '{nhomZaloId}' không tồn tại!");
            //    }
            //}


            ////Update UserDuAn
            //var existUserDuAn = await _dbContext.InternDuAns
            //        .Where(uda => uda.UserId == intern.UserId)
            //        .ToListAsync();
            //_dbContext.InternDuAns.RemoveRange(existUserDuAn);

            //foreach (var duAnId in model.IdDuAn)
            //{
            //    var isDuAnExist = await _dbContext.DuAns.AnyAsync(da => da.Id == duAnId);

            //    if (isDuAnExist)
            //    {
            //        var userDuAn = new UserDuAn
            //        {
            //            UserId = intern.UserId!,
            //            IdDuAn = duAnId
            //        };

            //        _dbContext.InternDuAns.Add(userDuAn);
            //    }
            //    else
            //    {
            //        return new BadRequestObjectResult($"Dự án với id '{duAnId}' không tồn tại!");
            //    }
            //}

            ////Check input IdTruong
            //var isTruongHocExist = await _dbContext.TruongHocs.AnyAsync(th => th.Id == model.IdTruong);

            //if (!isTruongHocExist)
            //{
            //    return new BadRequestObjectResult($"Trường học với id '{model.IdTruong}' không tồn tại!");
            //}

            ////Update User
            //var isUserExist = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == intern.UserId);
            //isUserExist!.Email = model.EmailCaNhan;
            //isUserExist.HoVaTen = model.HoTen; 

            //_dbContext.Users.Update(isUserExist);
            //await _dbContext.SaveChangesAsync();

            //if (model.Round == 0)
            //{
            //    intern.Status = "Chờ xét duyệt CV";
            //}
            //else if (model.Round == 1)
            //{
            //    intern.Status = "Chờ được phỏng vấn";
            //}
            //else
            //{
            //    intern.Status = "Đã đậu phỏng vấn";
            //}

            var entity = _mapper.Map(model, intern);

            var updateIntern = await _internRepo.UpdateInternInfoAsync(user, entity);


            if (updateIntern == 0)
            {
                return new BadRequestObjectResult($"Intern mssv: '{mssv}' cập nhật thất bại!");
            }

            return new OkObjectResult($"Cập nhật thành công Intern với mssv: '{mssv}' !");

        }


        //Comment on Intern
        public async Task<IActionResult> AddCommentInternInfo(CommentInternInfoDTO comment, string idCommentor, string mssv)
        {


            var entity = _mapper.Map<Comment>(comment);
            int rs = await _commentRepo.AddCommentIntern(entity, idCommentor, mssv);

            if (rs == 0)
            {
                return new BadRequestObjectResult("Thêm Comment thất bại!");
            }

            return new OkObjectResult($"Thêm mới Comment cho MSSV: '{mssv}' thành công!");
        }

        //Get Comments of Intern by MSSV
        public async Task<IActionResult> GetCommentsByMssv(string mssv)
        {
            InternInfo intern = await _internRepo.GetCommentByMssv(mssv);
            return new OkObjectResult(_mapper.Map<InternCommentDTO>(intern));
        }


        public async Task<string> RegisterIntern(RegisterUserRequestDTO registerUserRequestDTO)
        {
            var identityUser = CreateUserFromRequest(registerUserRequestDTO);
            var identityResult = await _userManager.CreateAsync(identityUser, registerUserRequestDTO.Password);

            var result = await _userManager.GetUsersInRoleAsync(Roles.INTERN);

            if (identityResult.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync(Roles.INTERN);

                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(Roles.INTERN));
                }

                identityResult = await _userManager.AddToRoleAsync(identityUser, Roles.INTERN);

                await SaveUserToken(identityUser);

                return identityUser.Id;
            }

            // Return null or throw an exception in case of failure
            return null;
        }


        private User CreateUserFromRequest(RegisterUserRequestDTO registerUserRequestDTO)
        {
            return new User
            {
                UserName = registerUserRequestDTO.Username,
                HoVaTen = registerUserRequestDTO.HoVaTen,
                PhoneNumber = registerUserRequestDTO.PhoneNumber,
                Email = registerUserRequestDTO.Email,
            };
        }

        private async Task SaveUserToken(User user)
        {
            var userToken = await _dbContext.UserTokens
                .FirstOrDefaultAsync(t => t.UserId == user.Id && t.LoginProvider == ConfirmAction.CONFIRM && t.Name == ConfirmAction.CONFIRM);

            if (userToken == null)
            {
                userToken = new IdentityUserToken<string>
                {
                    UserId = user.Id,
                    LoginProvider = ConfirmAction.CONFIRM,
                    Name = ConfirmAction.CONFIRM,
                    Value = string.Empty,
                };
                _dbContext.UserTokens.Add(userToken);
            }
            else
            {
                userToken.Value = string.Empty;
            }

            await _dbContext.SaveChangesAsync();
        }

        private List<InternInfo> ReadFile(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        var startRow = worksheet.Dimension.Start.Row;
                        var startCol = worksheet.Dimension.Start.Column;
                        var endRow = worksheet.Dimension.End.Row;
                        var endCol = worksheet.Dimension.End.Column;

                        var range = worksheet.Cells[startRow, startCol, endRow, endCol];

                        List<InternInfo> internList = range.ToCollectionWithMappings<InternInfo>(row =>
                        {
                            if (!string.IsNullOrEmpty(row.GetText("STT")))
                            {
                                object ngaySinh = row.GetValue<object>("NgaySinh");
                                DateTime dt;

                                if (ngaySinh is double)
                                {
                                    dt = DateTime.FromOADate((double)ngaySinh);
                                }
                                else
                                {
                                    DateTime.TryParse((string)ngaySinh, out dt);
                                }

                                var intern = new InternInfo
                                {
                                    HoTen = row.GetText("HoVaTen"),
                                    NgaySinh = dt,
                                    GioiTinh = row.GetText("GioiTinh").ToUpper().Equals("NAM") ? true : false,
                                    MSSV = row.GetText("MSSV"),
                                    EmailTruong = row.GetText("EmailTruong"),
                                    EmailCaNhan = row.GetText("EmailCaNhan"),
                                    Sdt = row.GetText("SDT"),
                                    DiaChi = row.GetText("DiaChi"),
                                    SdtNguoiThan = row.GetText("SdtNguoiThan"),
                                    GPA = row.GetValue<decimal>("GPA"),
                                    TrinhDoTiengAnh = row.GetText("TrinhDoTiengAnh"),
                                    NganhHoc = row.GetText("NganhHoc"),
                                    ViTriMongMuon = row.GetText("ViTriMongMuon"),
                                    LinkFacebook = row.GetText("LinkFacebook"),
                                    LinkCV = row.GetText("LinkCV"),
                                    Round = 0,
                                    Status = "Chờ xét duyệt CV",
                                };

                                return intern;
                            }
                            else
                            {
                                return null;
                            }
                        }, options => options.HeaderRow = 0);

                        return internList;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<InternInfo>();
            }
        }

        public async Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId)
        {
            // var uId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {

                List<InternInfo> interns = ReadFile(file);
                var existingKi = _kiThucTapRepository.GetKiThucTap(kiThucTapId);

                if (existingKi is null)
                    return new BadRequestObjectResult("Khong tim thay ki thuc tap voi id: " + kiThucTapId);

                foreach (var intern in interns)
                {
                    intern.KiThucTapId = existingKi.Id;
                    // intern.CreatedBy = uId;
                    intern.IdTruong = existingKi.IdTruong;
                    intern.StartDate = existingKi.NgayBatDau;
                    intern.EndDate = existingKi.NgayKetThuc;
                }

                var result = await _internRepo.AddListInternInfoAsync(interns);

                if (result == 0)
                {
                    return new BadRequestObjectResult("Something went wrong!");
                }

                return new OkObjectResult("Success!");
            }
            catch (System.Exception)
            {
                return new BadRequestObjectResult("Import fail!");
            }
        }


        //Hàm dùng để sendMail
        public void SendMail(string content, string ReceiveAddress, String Subject)
        {
            try
            {
               
                string htmlCode = "<div dir=\"ltr\" class=\"gmail_signature\" data-smartmail=\"gmail_signature\"><div dir=\"ltr\"><div style=\"color:rgb(34,34,34)\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"background:transparent;font-size:14px;border-spacing:0px;border-collapse:collapse;font-family:Arial,sans-serif;color:rgb(46,154,164);max-width:500px\"><tbody><tr><td colspan=\"2\" width=\"500\" style=\"padding:0px 0px 4px;border-bottom:1px solid rgb(47,154,163)\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:18pt\"><span style=\"font-weight:700\"><font color=\"#0b5394\">--<br>Amazing Tech</font></span></span><br></font><span style=\"color:rgb(58,67,69);font-family:Tahoma,sans-serif;font-size:13.3333px;font-weight:700\">Software Development Company</span>&nbsp;&nbsp;<font face=\"tahoma, sans-serif\"><span style=\"font-size:10pt\">&nbsp;&nbsp;<font color=\"#6fa8dc\">&nbsp;</font><font color=\"#0b5394\">|</font>&nbsp;</span><a href=\"http://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(17,85,204);background-color:transparent;font-size:10pt;font-weight:bold\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=http://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw1vaYI2qEOK2T13SAzEBs2K\"><span style=\"font-size:10pt\"><font color=\"#0b5394\">amazingtech.vn</font></span></a></font></td></tr><tr><td width=\"120\" style=\"padding:22px 0px 0px\"><p style=\"margin:0px;padding:0px\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Photo\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NYIQZghSrU6IZdMz9suNlE9PjPuyZ1EdHCVmzj1vU2jZHotFhiSOIh7oGk9mHRBWejqWtG8D56PF6Q3q59IV4nwG1DVQ2MRcXQl_NltCnkIt-h6k3jG4aAtPnP9QkKcWec1tfc=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/logo-amazing-tech-v2.png\" style=\"border:0px;vertical-align:middle;max-width:90px\" class=\"CToWUd\" data-bit=\"iit\"></font></p></td><td style=\"padding:25px 0px 0px\"><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">T:</span>&nbsp;</font>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(028) 9999 - 7939</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">M:</font></span>&nbsp;<span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">(+84) 888 181 100</span></font></p><p style=\"margin:0px;padding-bottom:2px;padding-top:0px;line-height:10pt\"><font face=\"tahoma, sans-serif\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\"><font color=\"#0b5394\">E:</font></span><font color=\"#3d85c6\">&nbsp;</font><a href=\"mailto:amazingtech.hr@gmail.com\" style=\"color:rgb(16,16,16);background-color:transparent;font-size:9pt;line-height:10pt\" target=\"_blank\"><span style=\"font-size:9pt;line-height:10pt\">amazingtech.hr@gmail.com</span></a></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><font color=\"#0b5394\"><span style=\"font-size:9pt;line-height:10pt;font-weight:bold\">A:</span>&nbsp;</font><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\">74 Nguyễn Cửu Đàm, Phường Tân Sơn Nhì, Quận Tân Phú, TP.HCM</span></font></p><p style=\"margin:0px;line-height:10pt;padding-bottom:2px;padding-top:0px\"><span style=\"font-size:9pt;line-height:10pt;color:rgb(16,16,16)\"><font face=\"tahoma, sans-serif\">&nbsp; &nbsp; &nbsp;S9.02A Vinhome Grand Park, Nguyễn Xiển, Phường Phước Thiện, Quận Thủ Đức, TP.HCM</font></span></p><p style=\"margin:4px 0px 0px;padding-bottom:0px;padding-top:0px\"><font face=\"tahoma, sans-serif\"><a href=\"https://www.facebook.com/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.facebook.com/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0PsTtrffG8GO6TFt1mZkDe\"><img border=\"0\" width=\"20\" alt=\"facebook icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Nb-H8iXOIGiIIOjCq60PcYJdwAiL3JI9ca8Zvfz7PzPeRO7EP5yLfmQlXiaR8gk8shP-__vXefTiEmbGIHRo-lRY8PhPrnocnAIhlpItBw65ruLKdU7nElsW1pxywyN9YiYA_O7zIbih2al=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/fb.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.youtube.com/@amazingtechvietnam\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.youtube.com/@amazingtechvietnam&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw0Zu2_6T3U_WB0AOHMaEeUq\"><img border=\"0\" width=\"20\" alt=\"youtube icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NaAgTDtnkTEwIcx8mxmO4-t06QPEl5sXLKF7gnOvFVQ8c31ca2W8BQmyAIlOBlSAjvOrXh_A452L86DfYtVK0dug2BbivN6r8XOyEEjnuvCDGtihKh_bNird28dMxannXIagpI_NkDRXrF4=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/yt.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;&nbsp;<a href=\"https://www.linkedin.com/company/amazingtech74\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://www.linkedin.com/company/amazingtech74&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw2Tb7NjZTwhK_taupiJr9iw\"><img border=\"0\" width=\"20\" alt=\"linkedin icon\" src=\"https://ci3.googleusercontent.com/meips/ADKq_Na22lJt_btcmFPUMmTXijHqUbMnhZF2iNtgvomlgswJg-7vB888Zz5mYQcCEZUJa3yv8bugefIkEYkRQaxxeNYbBGXdytU5BAgd5UyfBRUSkr4Whm9q6UjET04sVtiekVBDhGzB-6L_JUwn=s0-d-e1-ft#https://www.mail-signatures.com/signature-generator/img/templates/brands-voice/ln.png\" style=\"border:0px;vertical-align:middle;height:20px;width:20px\" class=\"CToWUd\" data-bit=\"iit\"></a>&nbsp;</font></p></td></tr><tr><td colspan=\"2\" style=\"padding:25px 0px 0px\"><a href=\"https://amazingtech.vn/\" rel=\"noopener\" style=\"color:rgb(51,122,183);background-color:transparent\" target=\"_blank\" data-saferedirecturl=\"https://www.google.com/url?q=https://amazingtech.vn/&amp;source=gmail&amp;ust=1706004238915000&amp;usg=AOvVaw11h_SNDADPSAg0WiKKnBHk\"><font face=\"tahoma, sans-serif\"><img border=\"0\" alt=\"Banner\" width=\"527\" src=\"https://ci3.googleusercontent.com/meips/ADKq_NZT7FwFWtXD-LTbufKOqpFfhg4b1eEPlZOVdgqqEYnGh-4T6jGP9ZYxGLs3OM5QSw2rwPVXTRBWooBDxsi466XySJNe9bvDYVVSqpql6UIqKEITfpN3U68wh91TZUyh=s0-d-e1-ft#https://amazingtech.vn/Content/amazingtech/assets/img/icon-1-1024x761.png\" height=\"392\" style=\"border:0px;vertical-align:middle;margin-right:0px\" class=\"CToWUd\" data-bit=\"iit\"></font></a></td></tr></tbody></table></div></div></div>";
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = Subject,
                    Body = content + htmlCode,
                    IsBodyHtml = true

                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(ReceiveAddress);

                var smtp = new SmtpClient()
                {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;
                smtp.DeliveryFormat = SmtpDeliveryFormat.International;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //Hàm dùng để sendEmail cho tất cả Intern có trong kỳ thực tập
        public async Task<IActionResult> SendMailForIntern(string idKyThucTap)
        {
            try
            {
                var kyThucTap = _kiThucTapRepository.GetKiThucTap(idKyThucTap);


                return new OkObjectResult("Gửi email thành công!");
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult("Gửi email thất bại!");
            }
            
        }
    }
}
