using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using swp391_be.API.Models.Request.Authenticate;

namespace AmazingTech.InternSystem.Services
{
    public class InternInfoService : IInternInfoService
    {
        private readonly IInternInfoRepo _internRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IKiThucTapRepository _kiThucTapRepository;

        private readonly AppDbContext _dbContext;

        public InternInfoService(IInternInfoRepo internRepo,
            IMapper mapper, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IKiThucTapRepository kiThucTapRepository,
            AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _internRepo = internRepo;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _kiThucTapRepository = kiThucTapRepository;
        }

        //Get all Intern
        public async Task<IActionResult> GetAllInternInfo()
        {
            List<InternInfo> interns = await _internRepo.GetAllInternsInfoAsync();
            return new OkObjectResult(_mapper.Map<List<InternInfoDTO>>(interns));
        }

        //Get Intern by Id
        public async Task<IActionResult> GetInternInfo(string mssv)
        {
            InternInfo intern = await _internRepo.GetInternInfoAsync(mssv);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Khong tim thay Intern voi mssv: {mssv} !");
            }
            return new OkObjectResult(_mapper.Map<InternInfoDTO>(intern));
        }

        //Add new Intern
        public async Task<IActionResult> AddInternInfo(AddInternInfoDTO model)
        {
            var entity = _mapper.Map<InternInfo>(model);

            var existIntern = await _internRepo.GetInternInfoAsync(entity.MSSV);
            if (existIntern != null)
            {
                return new BadRequestObjectResult("MSSV da ton tai trong danh sach!");
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
                return new BadRequestObjectResult("User ID is Null");
            }

            entity.UserId = userId;

            int rs = await _internRepo.AddInternInfoAsync(entity);

            if (rs == 0)
            {
                return new BadRequestObjectResult("Them sinh vien that bai!");
            }

            return new OkObjectResult(entity);
        }

        //Delete Intern
        public async Task<IActionResult> DeleteInternInfo(string mssv)
        {
            var intern = await _dbContext.InternInfos!.FirstOrDefaultAsync(i => i.MSSV == mssv);
            if (intern == null)
            {
                return new BadRequestObjectResult($"Khong tim thay Intern voi mssv: {mssv} !");
            }

            int rs = await _internRepo.DeleteInternInfoAsync(intern);

            if (rs == 0)
            {
                return new BadRequestObjectResult($"Intern mssv: {mssv} da duoc xoa!");
            }

            return new OkObjectResult($"Xoa thanh cong Intern mssv: {mssv} !");
        }

        //Update Intern
        public async Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv)
        {
            var updateIntern = await _internRepo.UpdateInternInfoAsync(mssv, model);


            if (updateIntern == 0)
            {
                return new BadRequestObjectResult($"Intern mssv: {mssv} cap nhat that bai!");
            }

            return new OkObjectResult($"Cap nhat thanh cong Intern mssv: {mssv} !");
        }

        public async Task<string> RegisterIntern(RegisterUserRequestDTO registerUserRequestDTO)
        {
            var identityUser = CreateUserFromRequest(registerUserRequestDTO);
            var identityResult = await _userManager.CreateAsync(identityUser, registerUserRequestDTO.Password);

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

                        // worksheet.Cells["C2:C21"].Style.Numberformat.Format = "d-mmm-yy";

                        var startRow = worksheet.Dimension.Start.Row;
                        var startCol = worksheet.Dimension.Start.Column;
                        var endRow = worksheet.Dimension.End.Row;
                        var endCol = worksheet.Dimension.End.Row;

                        var range = worksheet.Cells["A1:O21"];

                        List<InternInfo> internList = range.ToCollectionWithMappings<InternInfo>(row =>
                        {
                            var intern = new InternInfo
                            {
                                HoTen = row.GetText("HoVaTen"),
                                NgaySinh = DateTime.FromOADate(row.GetValue<double>("NgaySinh")),
                                GioiTinh = row.GetText("GioiTinh") == "Nam" ? true : false,
                                MSSV = row.GetText("MSSV"),
                                EmailTruong = row.GetText("EmailTruong"),
                                EmailCaNhan = row.GetText("EmailCaNhan"),
                                Sdt = row.GetText("SDT"),
                                DiaChi = row.GetText("DiaChi"),
                                SdtNguoiThan = row.GetText("SdtNguoiThan"),
                                GPA = row.GetValue<decimal>("GPA"),
                                TrinhDoTiengAnh = row.GetText("TrinhDoTiengAnh"),
                                NganhHoc = row.GetText("NganhHoc"),
                                ChungChi = "a",
                                LinkFacebook = row.GetText("LinkFacebook"),
                                LinkCV = row.GetText("LinkCV"),
                                Round = 0,
                                Status = "true",
                            };

                            return intern;
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
            List<InternInfo> interns = ReadFile(file);

            var existingKi = _kiThucTapRepository.GetKiThucTap(kiThucTapId);

            if (existingKi is null)
                return new BadRequestObjectResult("Khong tim thay ki thuc tap voi id: " + kiThucTapId);

            foreach (var intern in interns)
            {
                var user = new RegisterUserRequestDTO
                {
                    Username = intern.EmailTruong,
                    HoVaTen = intern.HoTen,
                    PhoneNumber = intern.Sdt,
                    Email = intern.EmailCaNhan,
                    Password = "1111111111ooo"
                };

                var userId = await RegisterIntern(user);

                if (userId is null)
                {
                    return new BadRequestObjectResult("User already exist!");
                }

                intern.KiThucTapId = existingKi.Id;
                intern.UserId = userId;
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
