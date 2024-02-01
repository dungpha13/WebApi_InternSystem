using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Enums;
using AmazingTech.InternSystem.Models.Response.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swp391_be.API.Models.Request.User;
using AmazingTech.InternSystem.Services.Name;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using AmazingTech.InternSystem.Models.Request.Authenticate;
using AmazingTech.InternSystem.Services;
using System.Security.Claims;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Models.Request.User;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly INameService _nameService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _dBUtils;
        private readonly IInternInfoRepo _internInfoRepo;

        public UserController(UserManager<User> _userManager, IMapper _mapper, INameService _nameService, AppDbContext _dBUtils, IHttpContextAccessor httpContextAccessor, IInternInfoRepo internInfoRepo, IUserService userService)
        {
            this._userManager = _userManager;
            this._mapper = _mapper;
            this._nameService = _nameService;
            this._dBUtils = _dBUtils;
            _contextAccessor = httpContextAccessor;
            _internInfoRepo = internInfoRepo;
            _userService = userService;
        }

        [HttpPost("update-trang-thai-thuc-tap/{id}")]
        public async Task<IActionResult> UpdateTrangThaiThucTap([FromRoute] string id, [FromBody] string trangThaiThucTap)
        {
            return await _userService.UpdateTrangThaiThucTap(id, trangThaiThucTap);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpGet]
        [Route("get/{id}")]
        //[Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            return await _userService.GetUserById(id);
        }


        [HttpPost("create")]
        //[Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDto)
        {
            return await _userService.CreateUser(createUserDto);
        }

        [HttpPut]
        [Route("update/{id:Guid}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO updateUserRequestDTO, [FromRoute] Guid id, [FromHeader(Name = "Authentication")] string authenHeader)
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
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
            if (user == null)
            {
                return NotFound(new
                {
                    succeeded = false,
                    errors = "User not found."
                });
            }

            //If null then will not update the value
            user.HoVaTen = updateUserRequestDTO.HoVaTen ?? user.HoVaTen;
            user.PhoneNumber = updateUserRequestDTO.PhoneNumber ?? user.PhoneNumber;
            user.Email = updateUserRequestDTO.EmailAddress ?? user.Email;
            user.NormalizedEmail = updateUserRequestDTO.EmailAddress?.ToUpper() ?? user.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    succeeded = false,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                succeeded = true
            });
        }

        //[HttpPost]
        //[Route("update/role/{email}/{role}")]
        //[Authorize(Roles = Roles.ADMIN)]
        //public async Task<IActionResult> UpdateUserRole([FromRoute] string role, [FromRoute] string email)
        //{
        //    var existingUser = await _userManager.FindByEmailAsync(email);
        //    if (existingUser == null)
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = new[] { "user doesn't exist" }
        //        });
        //    }
        //    var existingRole = await _userManager.GetRolesAsync(existingUser);


        //    await _userManager.RemoveFromRolesAsync(existingUser, existingRole);
        //    await _userManager.AddToRoleAsync(existingUser, role);
        //    try
        //    {
        //        var result = await _userManager.UpdateAsync(existingUser);
        //        return Ok(result);

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = new[] { "failed to update" }
        //        });
        //    }
        //}


        [HttpDelete]
        [Route("delete/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUser([FromRoute] string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                return BadRequest(new
                {
                    message = "User khong ton tai."
                });
            }

            var currentUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser.Equals(userId))
            {
                return BadRequest(new
                {
                    message = "Ban khong the xoa chinh minh."
                });
            }

            try
            {
                var result = await _userManager.DeleteAsync(existingUser);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        message = "Da delete user."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "Co loi xay ra."
                    });
                }
            }
            catch (Exception ex)
            {
                existingUser.DeletedTime = DateTime.Now;
                existingUser.UserName += "_";
                existingUser.Email += "_";
                while (true)
                {
                    var result = await _userManager.UpdateAsync(existingUser);

                    if (result.Succeeded)
                    {
                        break;
                    }
                    else
                    {
                        existingUser.UserName += "_";
                        existingUser.Email += "_";
                    }
                }
            }

            // phai xoa interninfo
            var roles = await _userManager.GetRolesAsync(existingUser);

            if (roles.Contains(Roles.INTERN))
            {
                var internInfos = await _internInfoRepo.GetAllInternsInfoAsync();

                if (internInfos.Count > 0)
                {
                    foreach (var internInfo in internInfos)
                    {
                        if (existingUser.Id.Equals(internInfo.Id))
                        {
                            await _internInfoRepo.DeleteInternInfoAsync(internInfo);
                            break;
                        }
                    }
                }
            }

            return Ok(new
            {
                message = "Da delete user."
            });
        }


        //[HttpPost]
        //[Route("update/avatar")]
        //public async Task<IActionResult> UploadUserImage([FromBody] UploadImageRequestDTO uploadImageRequest)
        //{
        //    var result = await _userManager.Users
        //                    .Where(x => x.Id == uploadImageRequest.UserId)
        //                    .ExecuteUpdateAsync(property => property.SetProperty(e => e.ImageUrl, uploadImageRequest.ImgUrl));
        //    if (result != 1)
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = new[] { "Failed to update" }
        //        });
        //    }

        //    return Ok(result);
        //}
    }
}
