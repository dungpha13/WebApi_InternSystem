using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Models.Request.InternInfo;
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
            _kiThucTapRepository = kiThucTapRepository;
        }

        public async Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId)
        {
            List<InternInfo> list = ReadFile(file);

            var kiThucTap = _kiThucTapRepository.GetKiThucTap(kiThucTapId);

            if (kiThucTap is null)
            {
                return new BadRequestObjectResult("Loi ki thuc tap null");
            }

            foreach (var item in list)
            {
                // Tao tai khoan cho Intern
                var account = new RegisterUserRequestDTO
                {
                    HoVaTen = item.HoTen,
                    Username = item.EmailTruong,
                    Email = item.EmailCaNhan,
                    Password = "0123456789ooo",
                    PhoneNumber = item.Sdt,
                };

                User user = await RegisterIntern(account);

                if (user == null)
                {
                    return new BadRequestObjectResult("Loi user null");
                }

                item.User = user;
                item.KiThucTapId = kiThucTap.Id;
            }

            await _internRepo.AddListInternInfoAsync(list);

            return new OkResult();
        }

        public async Task<User> RegisterIntern(RegisterUserRequestDTO registerUserRequestDTO)
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

                return identityUser;
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

        public List<InternInfo> ReadFile(IFormFile file)
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

        public async Task<IActionResult> GetInternInfo(string id)
        {
            var intern = await _internRepo.GetInternInfo(id);
            return new OkObjectResult(intern);
        }
    }
}
