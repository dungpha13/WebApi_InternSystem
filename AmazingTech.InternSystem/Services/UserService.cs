using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Models.Request.Authenticate;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
        //Task<IActionResult> RegisterSchool(RegisterSchoolDTO registerUser);
        Task<IActionResult> Login(SignInUserRequestDTO loginUser);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ITruongRepository _truongRepository;
        private readonly IInternInfoRepo _internInfoRepo;

        public UserService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            ITruongRepository truongRepository,
            IInternInfoRepo internInfoRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _truongRepository = truongRepository;
            _internInfoRepo = internInfoRepo;
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

                var existedUser = context.Users.Where(_ => _.NormalizedUserName.Equals(registerUser.Username.ToUpper()) || _.NormalizedEmail.Equals(registerUser.Email.ToUpper()));

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
                    PhoneNumber = registerUser.PhoneNumber
                };

                return await RegisterUser(user, registerUser.Password, Roles.INTERN);
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

            if (!user.isConfirmed)
            {
                return new BadRequestObjectResult("Hay cho Admin duyet.");
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

            var roleExist = await _roleManager.RoleExistsAsync(Roles.INTERN);
            if (!roleExist)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = Roles.INTERN });

                if (!createRoleResult.Succeeded)
                {
                    return new BadRequestObjectResult(new
                    {
                        message = "Có lỗi xảy ra khi tạo role."
                    });
                }
            }

            var registeredUser = await _userManager.FindByEmailAsync(user.Email);

            var addRoleResult = await _userManager.AddToRoleAsync(registeredUser, Roles.INTERN);
            if (!addRoleResult.Succeeded)
            {
                return new BadRequestObjectResult(new
                {
                    message = "Có lỗi xảy ra khi thêm role cho user."
                });
            }

            // Gui mail voi ma OTP

            return new OkObjectResult(new
            {
                message = "Đăng kí thành công! Hãy kiểm tra email để kích hoạt tài khoản."
            });
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
