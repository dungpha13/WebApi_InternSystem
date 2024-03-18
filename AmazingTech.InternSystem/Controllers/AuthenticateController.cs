using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
using DocumentFormat.OpenXml.Office2013.Excel;
using System.Net;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticateController(UserManager<User> userManager,
            AppDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            IUserService userService,
            IEmailService emailService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost]
        [Route("confirm-user/{userId}")]
        [Authorize(Roles = "Admin")] //only admins can access 
        public async Task<IActionResult> ConfirmUserRegistration(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            if (user.isConfirmed)
            {
                return BadRequest(new { message = "User already confirmed." });
            }

            user.isConfirmed = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to confirm user." });
            }

           

            return Ok(new { message = "User confirmed successfully." });
        }
        [HttpPost]
        [Route("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRoleDTO registerAdminDTO)
        {
            return await _userService.RegisterUserForRole(registerAdminDTO, Roles.ADMIN);
        }

        [HttpPost]
        [Route("register/hr")]
        public async Task<IActionResult> RegisterHR([FromBody] RegisterRoleDTO registerHRDTO)
        {
            return await _userService.RegisterUserForRole(registerHRDTO, Roles.HR);
        }

        [HttpPost]
        [Route("register/mentor")]
        public async Task<IActionResult> RegisterMentor([FromBody] RegisterRoleDTO registerMentorDTO)
        {
            return await _userService.RegisterUserForRole(registerMentorDTO, Roles.MENTOR);
        }

        [HttpPost]
        [Route("register/intern")]
        public async Task<IActionResult> RegisterIntern([FromBody] RegisterInternDTO registerUserRequestDTO)
        {
            return await _userService.RegisterIntern(registerUserRequestDTO);
        }

        [HttpPost]
        [Route("register/school")]
        public async Task<IActionResult> RegisterSchool([FromBody] RegisterSchoolDTO registerUserRequestDTO)
        {
            return await _userService.RegisterSchool(registerUserRequestDTO);
        }




        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] SignInUserRequestDTO signInUserRequestDTO)
        {
            return await _userService.Login(signInUserRequestDTO);
        }

        [HttpGet]
        [Route("email-confirmation")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string id, [FromQuery] string token)
        {
            return await _userService.ConfirmEmail(id, token);
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public IActionResult Logout(/*[FromHeader(Name = "Authorization")] string authorizationHeader*/)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization.ToString();

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
        [Route("login-with-google/intern")]
        [AllowAnonymous]
        public IActionResult LoginWithGoogleIntern()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "api/auth/redirect-google?role=Intern",
                Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("login-with-google/school")]
        [AllowAnonymous]
        public IActionResult LoginWithGoogleSchool()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "api/auth/redirect-google?role=School",
                Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("redirect-google")] // Redirect
        public async Task<IActionResult> GoogleLoginRedirect([FromQuery] string role)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                // Handle failed authentication
                return BadRequest("Failed to authenticate");
            }

            // Retrieve user information from claims
            //var userId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            //var userName = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            //var userPhone = result.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;

            // Next
            return await _userService.GoogleLoginRedirect(userEmail, role);
        }

        [HttpPost("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO requestDTO, [FromRoute] string id)
        {
            //Check user is changing own password
            string uid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (uid == null || role == null)
            {
                return BadRequest("Can't verify token");
            }

            if (!uid.Equals(id) && !role.Equals("Admin"))
            {
                return BadRequest(new
                {
                    message = "You don't have permission to change this user password."
                });
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
                    message = "Wrong old password."
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
            var feUrl = _configuration.GetValue<string>("Url:Frontend");
            string link = feUrl + $"reset-password?id={user.Id}&token={token}";
            string subject = "Reset Your Password";
            string content = $"Click the link to reset your password: {link}";
            _emailService.SendMail2(content, email, subject);

            return Ok(new
            {
                message = "Sent reset link to your email.",
                //token,
                //encodedToken = WebUtility.UrlEncode(token),
                //decodedToken = WebUtility.UrlDecode(token)
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

            var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(resetPasswordDTO.ResetToken), resetPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                var result2 = await _userManager.ResetPasswordAsync(user, WebUtility.UrlEncode(resetPasswordDTO.ResetToken), resetPasswordDTO.NewPassword);

                if (!result2.Succeeded)
                {
                    var result3 = await _userManager.ResetPasswordAsync(user, (resetPasswordDTO.ResetToken), resetPasswordDTO.NewPassword);

                    if (!result3.Succeeded)
                    {
                        return BadRequest(new
                        {
                            message = "Cannot verify token."
                        });
                    }
                }
            }
            return Ok(new
            {
                message = "Password reset successfully."
            });


        }

    }
}
