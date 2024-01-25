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

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly INameService _nameService;
        private readonly AppDbContext _dBUtils;


        public UserController(UserManager<User> _userManager, IMapper _mapper, INameService _nameService, AppDbContext _dBUtils)
        {
            this._userManager = _userManager;
            this._mapper = _mapper;
            this._nameService = _nameService;
            this._dBUtils = _dBUtils;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUser()
        {
            Thread.Sleep(700);
            var userDomainList = await _userManager.Users.ToListAsync();
            var result = new List<ProfileResponseDTO>();
            foreach (var User in userDomainList)
            {
                var roles = await _userManager.GetRolesAsync(User);
                var ProfileResponseDTO = _mapper.Map<ProfileResponseDTO>(User);
                ProfileResponseDTO.Roles = roles.ToList();
                result.Add(ProfileResponseDTO);
            }
            return Ok(result);
        }


        [HttpGet]
        [Route("get/{id:Guid}")]
        //[Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var userDomainModel = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

            if (userDomainModel is null)
            {
                return BadRequest(new
                {
                    succeeded = false,
                    errors = "The given user doesn't exist in our system."
                });
            }

            return Ok(_mapper.Map<ProfileResponseDTO>(userDomainModel));
        }


        [HttpPost("create")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            if (!(createUserRequestDTO.Roles == Roles.ADMIN
                || createUserRequestDTO.Roles == Roles.MENTOR
                || createUserRequestDTO.Roles == Roles.INTERN
                || createUserRequestDTO.Roles == Roles.SCHOOL))
            {
                return BadRequest(new
                {
                    succeeded = false,
                    errors = "The input role doesn't exist in our system"
                });
            }

            var identityUser = new User
            {
                UserName = createUserRequestDTO.Username,
                HoVaTen = createUserRequestDTO.HoVaTen,
                PhoneNumber = createUserRequestDTO.PhoneNumber,
                EmailConfirmed = true,
            };

            var identityResult = await _userManager.CreateAsync(identityUser, createUserRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddToRolesAsync(identityUser, new[] { createUserRequestDTO.Roles });

                if (identityResult.Succeeded)
                {
                    return Ok(identityResult);
                }
            }
            return BadRequest(identityResult);
        }

        //[HttpPost]
        //[Route("Update/{id:Guid}")]
        //public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO updateUserRequestDTO, [FromRoute] Guid id)
        //{
        //    //If all value is null then nothing to update
        //    if (string.IsNullOrEmpty(updateUserRequestDTO.HoVaTen)
                
        //        && string.IsNullOrEmpty(updateUserRequestDTO.PhoneNumber)
        //        && string.IsNullOrEmpty(updateUserRequestDTO.Password)
        //        && updateUserRequestDTO.Gender != null
        //        && string.IsNullOrEmpty(updateUserRequestDTO.Bio))
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = "There is nothing to update."
        //        });
        //    }

        //    //Check if user is exist or not
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
        //    var errors = new List<string>();
        //    if (user == null)
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = "This user doesn't exist, so we can't update."
        //        });
        //    }

        //    //Update First Name - Last Name - Phone Number
        //    //If null then will not update the value
        //    try
        //    {
        //        int status = await _userManager.Users.AsQueryable()
        //         .Where(u => u.Id == id.ToString())
        //         .ExecuteUpdateAsync(property =>
        //             property.SetProperty(e => e.HoVaTen, e => (e.HoVaTen != updateUserRequestDTO.HoVaTen && updateUserRequestDTO.HoVaTen != null)
        //                         ? updateUserRequestDTO.HoVaTen
        //                         : e.HoVaTen)

        //                     .SetProperty(e => e.PhoneNumber, e => (updateUserRequestDTO.PhoneNumber != null && user.PhoneNumber != updateUserRequestDTO.PhoneNumber)
        //                         ? updateUserRequestDTO.PhoneNumber
        //                         : e.PhoneNumber));
                            
                            
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = e.ToString()
        //        });
        //    }

        //    return Ok(new
        //    {
        //        succeeded = true
        //    });
        //}

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


        //[HttpDelete]
        //[Route("delete/{email}")]
        //public async Task<IActionResult> RemoveUser([FromRoute] string username)
        //{
        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = new[] { "username is null or empty unable to delete" }
        //        });
        //    }

        //    var existingUser = await _userManager.FindByNameAsync(username);

        //    if (existingUser == null)
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = new[] { "user doesn't exist" }
        //        });
        //    }
        //    var result = await _userManager.DeleteAsync(existingUser);
        //    if (result.Succeeded)
        //    {
        //        return Ok(new
        //        {
        //            succeeded = true,
        //            message = "User deleted successfully"
        //        });
        //    }
        //    else
        //    {
        //        return BadRequest(new
        //        {
        //            succeeded = false,
        //            errors = result.Errors.Select(error => error.Description)
        //        });
        //    }
        //}


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



        //[HttpGet]
        //[Route("avatar/{UserId}")]
        //public async Task<IActionResult> GetUserAvatar([FromRoute] string UserId)
        //{
        //    var existingUser = _userManager.FindByIdAsync(UserId);

        //    var imgUrl = existingUser.Result.ImageUrl;

        //    if (existingUser.Result == null) return Ok(null);


        //    return Ok(new
        //    {
        //        imageUrl = imgUrl
        //    });
        //}



    }
}
