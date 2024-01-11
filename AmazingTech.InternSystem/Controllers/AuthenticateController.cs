using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using swp391_be.API.Models.Request.Authenticate;
using swp391_be.API.Models.Response.Jwt;
using swp391_be.API.Repositories.Tokens;
using swp391_be.API.Services.Token;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateController(UserManager<User> userManager,
            ITokenRepository tokenRepository,
            AppDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequestDTO registerUserRequestDTO)
        {
            await Task.Delay(1000);

            var existingUser = await _userManager.FindByNameAsync(registerUserRequestDTO.Username);
            if (existingUser != null)
            {
                return BadRequest(new ErrorResponse { Succeeded = false, Errors = "Username already exists." });
            }

            var identityUser = CreateUserFromRequest(registerUserRequestDTO);
            var identityResult = await _userManager.CreateAsync(identityUser, registerUserRequestDTO.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest(new ErrorResponse { Succeeded = false, Errors = identityResult.Errors });
            }

            var roleExists = await _roleManager.RoleExistsAsync(Roles.INTERN);

            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(Roles.INTERN));

                if (!roleResult.Succeeded)
                {
                    return BadRequest(new ErrorResponse { Succeeded = false, Errors = roleResult.Errors });
                }
            }

            identityResult = await _userManager.AddToRoleAsync(identityUser, Roles.INTERN);

            if (!identityResult.Succeeded)
            {
                return BadRequest(new ErrorResponse { Succeeded = false, Errors = identityResult.Errors });
            }

            await SaveUserToken(identityUser);

            return Ok(new { Succeeded = true, Message = "Registration successful." });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] SignInUserRequestDTO signInUserRequestDTO)
        {
            await Task.Delay(1000);

            var user = await _userManager.FindByNameAsync(signInUserRequestDTO.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, signInUserRequestDTO.Password))
            {
                return BadRequest(new ErrorResponse { Succeeded = false, Errors = "Invalid username or password." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                return BadRequest(new ErrorResponse { Succeeded = false, Errors = "This current user doesn't have a role." });
            }

            var identityUser = new User
            {
                UserName = user.UserName,
                Id = user.Id
            };
            var jwtToken = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

            if (string.IsNullOrEmpty(jwtToken))
            {
                return BadRequest(new ErrorResponse { Succeeded = false, Errors = "Failed to generate JWT token." });
            }

            return Ok(new JwtResponseDTo { Jwt = jwtToken, Role = roles[0], UserId = user.Id });
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
    }

    public class ErrorResponse
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
    }
}
