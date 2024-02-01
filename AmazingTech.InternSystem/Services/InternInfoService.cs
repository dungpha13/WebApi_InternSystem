using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
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
using swp391_be.API.Models.Request.Authenticate;
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
            if (interns.Any(intern => intern.EmailTruong == model.EmailTruong))
            {
                return new BadRequestObjectResult("Email này đã được sử dụng!");
            }

            //Check input IdViTri
            foreach (var viTriId in model.ViTrisId)
            {
                var isViTriExist = await _dbContext.ViTris.AnyAsync(vt => vt.Id == viTriId);

                if (!isViTriExist)
                {
                    return new BadRequestObjectResult($"Vị trí với id: '{viTriId}' không tồn tại!");
                }

            }

            //Check input IdDuAn
            foreach (var duAnId in model.IdDuAn)
            {
                var isDuAnExist = await _dbContext.DuAns.AnyAsync(da => da.Id == duAnId);

                if (!isDuAnExist)
                {
                    return new BadRequestObjectResult($"Dự án với id '{duAnId}' không tồn tại!");
                }

            }

            //Check input IdNhomZalo
            foreach (var nhomZaloId in model.IdNhomZalo)
            {
                var isNhomZaloExist = await _dbContext.NhomZalos.AnyAsync(nz => nz.Id == nhomZaloId);

                if (!isNhomZaloExist)
                {
                    return new BadRequestObjectResult($"Nhóm Zalo với id '{nhomZaloId}' không tồn tại!");
                }

            }

            //Check input IdTruong
            var isTruongHocExist = await _dbContext.TruongHocs.AnyAsync(th => th.Id == model.IdTruong);

            if (!isTruongHocExist)
            {
                return new BadRequestObjectResult($"Trường học với id '{model.IdTruong}' không tồn tại!");
            }


            // Tao tai khoan cho Intern
            var account = new RegisterUserRequestDTO
            {
                HoVaTen = entity.HoTen,
                Username = entity.EmailTruong,
                Email = entity.EmailCaNhan,
                Password = "0123456789ooo",
                PhoneNumber = entity.Sdt,
            };
            string userId = await RegisterIntern(account);

            if (userId == null)
            {
                return new BadRequestObjectResult("Tên tài khoản không hợp lệ (Check lại gmail đăng ký)");
            }


            entity.UserId = userId;

            //Add UserViTri
            foreach (var viTriId in model.ViTrisId)
            {
                var userViTri = new UserViTri
                {
                    UsersId = userId,
                    ViTrisId = viTriId
                };

                _dbContext.UserViTris.Add(userViTri);
            }


            //Add NhomZalo
            foreach (var nhomZaloId in model.IdNhomZalo)
            {
                var userNhomZalo = new UserNhomZalo
                {
                    UserId = userId,
                    IdNhomZalo = nhomZaloId
                };

                _dbContext.UserNhomZalos.Add(userNhomZalo);
            }


            //Add UserDuAn
            foreach (var duAnId in model.IdDuAn)
            {
                var userDuAn = new UserDuAn
                {
                    UserId = userId!,
                    IdDuAn = duAnId
                };

                _dbContext.InternDuAns.Add(userDuAn);
            }

            //Add InternInfo
            int rs = await _internRepo.AddInternInfoAsync(user, entity);

            if (rs == 0)
            {
                return new BadRequestObjectResult("Thêm sinh viên thất bại!");
            }

            return new OkObjectResult(entity);
        }

        //Delete Intern
        public async Task<IActionResult> DeleteInternInfo(string mssv)
        {
            var intern = await _dbContext.InternInfos!.FirstOrDefaultAsync(i => i.MSSV == mssv);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Không tìm thấy Intern với mssv: {mssv} !");
            }

            int rs = await _internRepo.DeleteInternInfoAsync(intern);

            if (rs == 0)
            {
                return new BadRequestObjectResult($"Intern mssv: {mssv} đã được xóa!");
            }

            return new OkObjectResult($"Xóa thành công Intern mssv: {mssv} !");
        }

        //Update Intern
        public async Task<IActionResult> UpdateInternInfo(string user, UpdateInternInfoDTO model, string mssv)
        {

            var intern = await _dbContext.InternInfos.FirstOrDefaultAsync(x => x.MSSV == mssv && x.DeletedBy == null);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Intern với MSSV: '{mssv}' không tồn tại!");
            }


            //Update UserViTri
            var existUserViTri = await _dbContext.UserViTris
                   .Where(uv => uv.UsersId == intern.UserId)
                   .ToListAsync();
            _dbContext.UserViTris.RemoveRange(existUserViTri);

            foreach (var viTriId in model.ViTrisId)
            {
                // Kiểm tra nếu viTriId tồn tại trong CSDL
                var isViTriExist = await _dbContext.ViTris.AnyAsync(vt => vt.Id == viTriId);

                if (isViTriExist)
                {

                    var userViTri = new UserViTri
                    {
                        UsersId = intern.UserId!,
                        ViTrisId = viTriId
                    };

                    _dbContext.UserViTris.Add(userViTri);
                }
                else
                {
                    return new BadRequestObjectResult($"Vị trí với id: '{viTriId}' không tồn tại!");
                }
            }


            //Update UserNhomZalo
            var existUserNhomZalo = await _dbContext.UserNhomZalos
                  .Where(unz => unz.UserId == intern.UserId)
                  .ToListAsync();
            _dbContext.UserNhomZalos.RemoveRange(existUserNhomZalo);

            foreach (var nhomZaloId in model.IdNhomZalo)
            {
                var isNhomZaloExist = await _dbContext.NhomZalos.AnyAsync(nz => nz.Id == nhomZaloId);

                if (isNhomZaloExist)
                {
                    var userNhomZalo = new UserNhomZalo
                    {
                        UserId = intern.UserId!,
                        IdNhomZalo = nhomZaloId
                    };

                    _dbContext.UserNhomZalos.Add(userNhomZalo);
                }
                else
                {
                    return new BadRequestObjectResult($"Nhóm Zalo với id '{nhomZaloId}' không tồn tại!");
                }
            }


            //Update UserDuAn
            var existUserDuAn = await _dbContext.InternDuAns
                    .Where(uda => uda.UserId == intern.UserId)
                    .ToListAsync();
            _dbContext.InternDuAns.RemoveRange(existUserDuAn);

            foreach (var duAnId in model.IdDuAn)
            {
                var isDuAnExist = await _dbContext.DuAns.AnyAsync(da => da.Id == duAnId);

                if (isDuAnExist)
                {
                    var userDuAn = new UserDuAn
                    {
                        UserId = intern.UserId!,
                        IdDuAn = duAnId
                    };

                    _dbContext.InternDuAns.Add(userDuAn);
                }
                else
                {
                    return new BadRequestObjectResult($"Dự án với id '{duAnId}' không tồn tại!");
                }
            }

            //Check input IdTruong
            var isTruongHocExist = await _dbContext.TruongHocs.AnyAsync(th => th.Id == model.IdTruong);

            if (!isTruongHocExist)
            {
                return new BadRequestObjectResult($"Trường học với id '{model.IdTruong}' không tồn tại!");
            }

            //Update User
            var isUserExist = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == intern.UserId);
            isUserExist!.Email = model.EmailCaNhan;
            isUserExist.HoVaTen = model.HoTen; 

            _dbContext.Users.Update(isUserExist);
            await _dbContext.SaveChangesAsync();

            var entity = _mapper.Map(model, intern);

            var updateIntern = await _internRepo.UpdateInternInfoAsync(user, entity);


            if (updateIntern == 0)
            {
                return new BadRequestObjectResult($"Intern mssv: {mssv} cập nhật thất bại!");
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
                                    LinkFacebook = row.GetText("LinkFacebook"),
                                    LinkCV = row.GetText("LinkCV"),
                                    Round = 0,
                                    Status = "true",
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
            var uId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<InternInfo> interns = ReadFile(file);
            var existingKi = _kiThucTapRepository.GetKiThucTap(kiThucTapId);

            if (existingKi is null)
                return new BadRequestObjectResult("Khong tim thay ki thuc tap voi id: " + kiThucTapId);

            foreach (var intern in interns)
            {
                var user = new RegisterUserRequestDTO
                {
                    Username = intern.EmailTruong!,
                    HoVaTen = intern.HoTen,
                    PhoneNumber = intern.Sdt!,
                    Email = intern.EmailCaNhan!,
                    Password = "1111111111ooo"
                };

                var userId = await RegisterIntern(user);

                if (userId is null)
                {
                    return new BadRequestObjectResult("User already exist!");
                }

                intern.KiThucTapId = existingKi.Id;
                intern.UserId = userId;
                intern.CreatedBy = uId;
                intern.IdTruong = existingKi.IdTruong;
            }

            var result = await _internRepo.AddListInternInfoAsync(interns);

            if (result == 0)
            {
                return new BadRequestObjectResult("Something went wrong!");
            }

            return new OkResult();
        }
    }
}
