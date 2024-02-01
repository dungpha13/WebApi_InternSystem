using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Models.Request.Authenticate;
using AmazingTech.InternSystem.Repositories;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using swp391_be.API.Models.Request.Authenticate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmazingTech.InternSystem.Services
{
    public interface IUserService
    {
        Task<IActionResult> RegisterIntern(RegisterInternDTO registerUser);
        Task<IActionResult> RegisterSchool(RegisterSchoolDTO registerUser);
        Task<IActionResult> Login(SignInUserRequestDTO loginUser);
        Task<IActionResult> ConfirmEmail(string userId, string token);
        Task<IActionResult> GoogleLoginRedirect(string email, string role);
        Task<IActionResult> UpdateTrangThaiThucTap(string id, string trangThaiThucTap);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ITruongRepository _truongRepository;
        private readonly IInternInfoRepo _internInfoRepo;

        public UserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            ITruongRepository truongRepository,
            IInternInfoRepo internInfoRepo,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _truongRepository = truongRepository;
            _internInfoRepo = internInfoRepo;
            _configuration = configuration;
        }

        public async Task<IActionResult> RegisterIntern(RegisterInternDTO registerUser)
        {
            using (var context = new AppDbContext())
            {
                var intern = context.InternInfos
                    .Where(_ => _.EmailCaNhan.Equals(registerUser.Email) || _.EmailTruong.Equals(registerUser.Email)).SingleOrDefault();

                if (intern == null)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Bạn không có trong danh sách intern. Hãy kiểm tra lại email."
                    });
                }

                if (intern.UserId != null)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Email cá nhân hoặc email trường của bạn đã được dùng để tạo tài khoản, vui lòng kiểm tra lại."
                    });
                }

                var existedUser = context.Users.Where(_ => _.NormalizedUserName.Equals(registerUser.Username.ToUpper()) || _.NormalizedEmail.Equals(registerUser.Email.ToUpper())).SingleOrDefault();

                if (existedUser != null)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Username hoặc email đã tồn tại."
                    });
                }

                var user = new User
                {
                    HoVaTen = intern.HoTen,
                    Email = registerUser.Email,
                    UserName = registerUser.Username,
                    PhoneNumber = intern.Sdt,
                    InternInfoId = intern.Id
                };

                return await RegisterUser(user, registerUser.Password, Roles.INTERN);
            }
        }

        public async Task<IActionResult> RegisterSchool(RegisterSchoolDTO registerUser)
        {
            using (var context = new AppDbContext())
            {
                var existedSchools = await _userManager.GetUsersInRoleAsync(Roles.SCHOOL);
                if (existedSchools.Any())
                {
                    var school = existedSchools.Where(_ => _.HoVaTen.Equals(registerUser.SchoolName)).SingleOrDefault();
                    if (school != null)
                    {
                        return new BadRequestObjectResult(new
                        {
                            message = "Tên trường đã tồn tại."
                        });
                    }
                }

                var existedUser = context.Users.Where(_ => _.NormalizedUserName.Equals(registerUser.Username.ToUpper()) || _.NormalizedEmail.Equals(registerUser.Email.ToUpper())).SingleOrDefault();

                if (existedUser != null)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Username hoặc email đã tồn tại."
                    });
                }

                var user = new User
                {
                    HoVaTen = registerUser.SchoolName,
                    Email = registerUser.Email,
                    UserName = registerUser.Username,
                    PhoneNumber = registerUser.PhoneNumber
                };

                return await RegisterUser(user, registerUser.Password, Roles.SCHOOL);
            }
        }

        public async Task<IActionResult> Login(SignInUserRequestDTO loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.Username);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginUser.Username);
            }
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = "Invalid username/email or password." });
            }

            if (!user.EmailConfirmed)
            {
                return new BadRequestObjectResult("Bạn chưa xác thực email.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = "This current user doesn't have a role." });
            }

            var identityUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id
            };
            var jwtToken = JwtGenerator.GenerateToken(identityUser, roles.ToList());

            if (string.IsNullOrEmpty(jwtToken))
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = "Failed to generate JWT token." });
            }

            return new OkObjectResult(new { accessToken = jwtToken });
        }

        private async Task<IActionResult> RegisterUser(User user, string password, string role)
        {
            var registerResult = await _userManager.CreateAsync(user, password);
            if (!registerResult.Succeeded)
            {
                return new BadRequestObjectResult(new
                {
                    message = "Có lỗi xảy ra khi tạo user."
                });
            }

            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = role });

                if (!createRoleResult.Succeeded)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Có lỗi xảy ra khi tạo role."
                    });
                }
            }

            var registeredUser = await _userManager.FindByEmailAsync(user.Email);

            var addRoleResult = await _userManager.AddToRoleAsync(registeredUser, role);
            if (!addRoleResult.Succeeded)
            {
                return new BadRequestObjectResult(new
                {
                    message = "Có lỗi xảy ra khi thêm role cho user."
                });
            }

            if (role.Equals(Roles.INTERN))
            {
                using (var context = new AppDbContext())
                {
                    var internInfo = context.InternInfos
                        .Where(_ => _.EmailCaNhan.Equals(user.Email) || _.EmailTruong.Equals(user.Email)).SingleOrDefault();

                    if (internInfo != null)
                    {
                        internInfo.UserId = user.Id;
                    }

                    context.InternInfos.Update(internInfo);
                    context.SaveChanges();
                }
            }

            // Gui mail voi ma OTP
            SendOTPToEmail(registeredUser);

            return new OkObjectResult(new
            {
                message = "Đăng kí thành công! Hãy kiểm tra email để kích hoạt tài khoản. Nếu bạn đã đăng kí bằng Gmail, hãy đăng nhập bằng Gmail để cập nhật username và password."
            });
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new NotFoundObjectResult(new
                {
                    message = "Không tìm thấy user."
                });
            }

            if (user.EmailConfirmed)
            {
                return new BadRequestObjectResult(new
                {
                    message = "User này đã xác nhận email rồi."
                });
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

            if (!confirmResult.Succeeded)
            {
                return new BadRequestObjectResult(new
                {
                    message = "Không thể xác nhận, hãy kiểm tra lại token."
                });
            }

            try
            {
                _emailService.SendMail2($"Thank you for registering on our website, log in now to use your account!", user.Email, "Welcome to AmazingTech!");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return new OkObjectResult(new
            {
                message = "Xác nhận thành công. Hãy đăng nhập vào tài khoản của bạn."
            });
        }

        public async Task<IActionResult> GoogleLoginRedirect(string email, string role)
        {
            // kiem tra user da ton tai chua?
            var existedUser = await _userManager.FindByEmailAsync(email);

            // da ton tai => login va tra token
            if (existedUser != null)
            {
                var identityUser = new User
                {
                    UserName = existedUser.UserName,
                    Email = existedUser.Email,
                    Id = existedUser.Id
                };

                var roles = await _userManager.GetRolesAsync(existedUser);
                var jwtToken = JwtGenerator.GenerateToken(identityUser, roles.ToList());

                if (string.IsNullOrEmpty(jwtToken))
                {
                    return new BadRequestObjectResult(new { Succeeded = false, Errors = "Failed to generate JWT token." });
                }

                return new OkObjectResult(new { accessToken = jwtToken });
            }
            // chua => tao tai khoan dua tren mail
            switch (role)
            {
                case "Intern":
                    return await RegisterIntern(new RegisterInternDTO
                    {
                        Email = email,
                        Username = "",
                        Password = ""
                    });
                default:
                    return await RegisterSchool(new RegisterSchoolDTO
                    {
                        Email = email,
                        Username = "",
                        Password = ""
                    });
            }

        }

        public async Task<IActionResult> UpdateTrangThaiThucTap(string id, string trangThaiThucTap)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new NotFoundObjectResult(new
                {
                    message = "Không tìm thấy user. Bạn đã nhập sai ID hoặc user đã bị delete."
                });
            }

            Enum.TryParse(trangThaiThucTap, out TrangThaiThucTap trangThai);
            user.TrangThaiThucTap = trangThai;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new BadRequestObjectResult(new
                {
                    message = "Có lỗi xảy ra khi update. Hãy kiểm tra lại tên trạng thái bạn nhập đúng chưa?"
                });
            }

            return new OkObjectResult(new
            {
                message = "Cập nhật trạng thái thành công!"
            });
        }

        private async void SendOTPToEmail(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var uid = user.Id;

            var url = _configuration.GetValue<string>("Url:Backend");
            string link = url + $"api/auth/email-confirmation?userId={uid}&token={token}";

            try
            {
                _emailService.SendMail2($"Please confirm your account by clicking this link: {link}", user.Email, "Confirm your email to complete registration!");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async Task SaveUserToken(User user)
        {
            using (var _dbContext = new AppDbContext())
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
        }
    }
}
