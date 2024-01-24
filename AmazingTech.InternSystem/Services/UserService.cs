using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using swp391_be.API.Models.Request.Authenticate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmazingTech.InternSystem.Services
{
    public interface IUserService
    {
        Task<IActionResult> Register(RegisterUserRequestDTO registerUser);
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

        public async Task<IActionResult> Register(RegisterUserRequestDTO registerUser)
        {
            var existingUser = await _userManager.FindByNameAsync(registerUser.Username);
            if (existingUser != null)
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = "Username da ton tai." });
            }

            var existingUserMail = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existingUserMail != null)
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = "Email da ton tai." });
            }

            var newUser = new User
            {
                HoVaTen = registerUser.HoVaTen,
                PhoneNumber = registerUser.PhoneNumber,
                Email = registerUser.Email,
                UserName = registerUser.Username,
            };

            var roleExists = await _roleManager.RoleExistsAsync(registerUser.Role);
            var roleName = registerUser.Role;

            if (registerUser.Role.Equals("Intern") || registerUser.Role.Equals("School"))
            {
                newUser.isConfirmed = true;
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            } 
            else 
            if (registerUser.Role.Equals("Admin") 
                || registerUser.Role.Equals("HR") 
                || registerUser.Role.Equals("Mentor")) 
            {
                newUser.isConfirmed = false;
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            }

            if (!roleExists)
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = "Role khong ton tai." });
            }

            if (registerUser.Role.Equals("Intern"))
            {
                var truongExist = _truongRepository.GetAllTruongs().Where(t => t.Ten.Equals(registerUser.Truong));

                if (truongExist == null || !truongExist.Any())
                {
                    return new BadRequestObjectResult(new { Succeeded = false, Errors = "Truong khong ton tai." });
                }
            }

            var createUserResult = await _userManager.CreateAsync(newUser, registerUser.Password);

            if (!createUserResult.Succeeded)
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = createUserResult.Errors });
            }

            var addUserToRoleResult = await _userManager.AddToRoleAsync(newUser, registerUser.Role);

            if (!addUserToRoleResult.Succeeded)
            {
                return new BadRequestObjectResult(new { Succeeded = false, Errors = addUserToRoleResult.Errors });
            }

            // tạo intern info nếu như user là intern
            if (registerUser.Role.Equals("Intern"))
            {
                var createdUser = await _userManager.FindByNameAsync(newUser.UserName);

                int result = await _internInfoRepo.AddInternInfoAsync(createdUser.Id, new InternInfo
                {
                    MSSV = registerUser.Mssv,
                    
                });
            }

            await SaveUserToken(newUser);

            _emailService.SendRegistrationSuccessfulMail(newUser);

            return new OkObjectResult(new { Succeeded = true, Message = "Registration successful." });
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

    public static class JwtGenerator
    {
        public static string GenerateToken(User user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("de455d3d7f83bf393eea5aef43f474f4aac57e3e8d75f9118e60d526453002dc");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("username", user.UserName),
                    new Claim(ClaimTypes.Role, roles[0]),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<InvalidToken> InvalidTokens = new List<InvalidToken>();

        public static void InvalidateToken(string token)
        {
            InvalidTokens.Add(new InvalidToken { Token = token, InvalidatedAt = DateTime.UtcNow });
        }
        public static bool IsTokenValid(string token)
        {
            // Clear expired tokens before checking validity
            ClearExpiredTokens();

            return !InvalidTokens.Any(t => t.Token == token);
        }

        private static void ClearExpiredTokens()
        {
            var expirationThreshold = DateTime.UtcNow.AddHours(-1); // Set your expiration duration

            InvalidTokens.RemoveAll(t => t.InvalidatedAt < expirationThreshold);
        }

        private class InvalidToken
        {
            public string Token { get; set; }
            public DateTime InvalidatedAt { get; set; }
        }
    }
}
