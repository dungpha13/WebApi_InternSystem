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

        [Authorize(Roles = "Admin, HR")]
        [HttpPost("update-trang-thai-thuc-tap/{id}")]
        public async Task<IActionResult> UpdateTrangThaiThucTap([FromRoute] string id, [FromBody] string trangThaiThucTap)
        {
            return await _userService.UpdateTrangThaiThucTap(id, trangThaiThucTap);
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpGet]
        [Route("get/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            if (!id.Equals(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) && !Roles.ADMIN.Equals(HttpContext.User.FindFirstValue(ClaimTypes.Role)) && !Roles.HR.Equals(HttpContext.User.FindFirstValue(ClaimTypes.Role))) {
                return Forbid();
            }
            return await _userService.GetUserById(id);
        }


        [HttpPost("create")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDto)
        {
            return await _userService.CreateUser(createUserDto);
        }

        [HttpPut]
        [Authorize]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserRequestDTO, [FromRoute] string id)
        {
            if (!id.Equals(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Forbid();
            }
            return await _userService.UpdateUser(id, updateUserRequestDTO);
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
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            return await _userService.DeleteUser(id);
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
