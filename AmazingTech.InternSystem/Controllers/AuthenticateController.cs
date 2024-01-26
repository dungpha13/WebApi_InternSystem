using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using swp391_be.API.Models.Request.Authenticate;
using swp391_be.API.Services.Token;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AmazingTech.InternSystem.Services;
using AmazingTech.InternSystem.Models.Request.Authenticate;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public AuthenticateController(UserManager<User> userManager,
            AppDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            IUserService userService,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequestDTO registerUserRequestDTO)
        {
            return await _userService.Register(registerUserRequestDTO);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] SignInUserRequestDTO signInUserRequestDTO)
        {
            return await _userService.Login(signInUserRequestDTO);
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

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public IActionResult Logout([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    JwtGenerator.InvalidateToken(token);
                    return Ok(new { message = "Log out successfully" });
                }
            }
            return BadRequest();
        }

        //Login with google
        [HttpGet]
        [Route("login-with-google")]
        public IActionResult LoginWithGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "api/auth/redirect-google",
                Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("redirect-google")] // Redirect
        public async Task<IActionResult> GoogleLoginRedirect()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                // Handle failed authentication
                return BadRequest("Failed to authenticate");
            }

            // Retrieve user information from claims
            var userId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var userName = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var userPhone = result.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;

            // Next
            var existingUser = await _userManager.FindByEmailAsync(userEmail);
            if (existingUser != null)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                if (roles == null || !roles.Any())
                {
                    return BadRequest(new { message = "This current user doesn't have a role." });
                }

                if (!existingUser.isConfirmed)
                {
                    return BadRequest("Hay cho Admin duyet.");
                }

                return new OkObjectResult(new
                {
                    accessToken = JwtGenerator.GenerateToken(existingUser, roles.ToList())
                });
            }

            var identityUser = CreateUserFromRequest(new RegisterUserRequestDTO
            {
                HoVaTen = userName,
                Email = userEmail,
                Username = userEmail,
                PhoneNumber = userPhone,
                Role = Roles.INTERN
            });

            var identityResult = await _userManager.CreateAsync(identityUser);

            if (!identityResult.Succeeded)
            {
                return BadRequest(new { message = identityResult.Errors });
            }

            identityResult = await _userManager.AddToRoleAsync(identityUser, Roles.INTERN);

            if (!identityResult.Succeeded)
            {
                return BadRequest(new { message = identityResult.Errors });
            }

            await SaveUserToken(identityUser);

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || !roles.Any())
                {
                    return BadRequest(new { message = "This current user doesn't have a role." });
                }

                return new OkObjectResult(new
                {
                    accessToken = JwtGenerator.GenerateToken(user, roles.ToList())
                });
            }

            return new BadRequestObjectResult("Error");
        }

        [HttpPost("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO requestDTO, [FromRoute] string id, [FromHeader(Name = "Authentication")] string authenHeader)
        {
            //Check user
            string token = JwtGenerator.ExtractTokenFromHeader(authenHeader);
            string uid = JwtGenerator.ExtractUserIdFromToken(token);
            string role = JwtGenerator.ExtractUserRoleFromToken(token);

            if (!uid.Equals(id) && !role.Equals("Admin"))
            {
                return Forbid("You don't have permission to change this user's password.");
            }

            //Check if user is exist or not
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            //Update password
            var changePassResult = await _userManager.ChangePasswordAsync(user, requestDTO.OldPassword, requestDTO.Password);

            if (!changePassResult.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Passwords not matched."
                });
            }

            return Ok(new
            {
                message = "Password is updated."
            });
        }

        [HttpGet("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword([FromRoute] string email)
        {
            // Check user with email exists
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { message = "No user with that email." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var feUrl = _configuration.GetValue<string>("Frontend:Default");
            string link = feUrl + $"reset-password?id={user.Id}&token={token}";
            string subject = "Reset Your Password";
            string content = $"Click the link to reset your password: {link}";
            _emailService.SendMail2(content, email, subject);

            return Ok(new
            {
                message = "Sent reset link to your email."
            });
        }

        [HttpPost("reset-password/{userId}")]
        public async Task<IActionResult> ResetPassword(string userId, [FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new
                {
                    message = "This user is no longer existed."
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.ResetToken, resetPasswordDTO.NewPassword);

            if(!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Cannot verify token."
                });
            }
            return Ok(new
            {
                message = "Password reset successfully."
            });
        }

        [HttpPost("confirm-account/{userId}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> ConfirmAccount([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            if (user.isConfirmed)
            {
                return BadRequest(new
                {
                    message = "This user has already been confirmed."
                });
            }

            user.isConfirmed = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "An error occured."
                });
            }

            return Ok(new
            {
                message = "Confirm successfully."
            });
        }
    }
}
