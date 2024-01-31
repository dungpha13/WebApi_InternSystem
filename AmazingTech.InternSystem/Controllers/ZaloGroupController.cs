using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static AmazingTech.InternSystem.Data.Enum.Enums;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/group-zalos")]
    [ApiController]
    //[Authorize(Roles = "Admin,Hr,Mentorn")]
    public class ZaloGroupController : ControllerBase
    {
        private readonly INhomZaloService _nhomZaloService;
        public ZaloGroupController(INhomZaloService nhomZaloService, ILogger<ZaloGroupController> logger)
        {
            _nhomZaloService = nhomZaloService;
        }

        // Manage ZaloGroup
        [HttpGet("get")]
        public async Task<ActionResult<List<NhomZalo>>> GetAllZaloGroupsAsync()
        {
            try
            {
                var groups = await _nhomZaloService.GetAllZaloAsync();

                if (groups is List<NhomZalo> nhomZaloList)
                {
                    var formattedResponse = nhomZaloList.Select(nhomZalo => new
                    {
                        id = nhomZalo.Id,
                        tenNhom = nhomZalo.TenNhom,
                        linkNhom = nhomZalo.LinkNhom,
                        /*idMentor = nhomZalo.IdMentor,
                        mentorName = nhomZalo.Mentor?.UserName*/
                    }).ToList();

                    return Ok(formattedResponse);
                }

                return groups;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<NhomZaloDTO>> GetZaloGroupByIdAsync(string id)
        {
            try
            {
                var group = await _nhomZaloService.GetGroupByIdAsync(id);

                if (group == null)
                    return NotFound($"Zalo group with ID {id} not found.");

                var formattedResponse = new NhomZaloDTO
                {
                    TenNhom = group.TenNhom,
                    LinkNhom = group.LinkNhom,
                   /* IdMentor = group.IdMentor,
                    MentorName = group.Mentor?.UserName*/
                };

                return Ok(formattedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost("create")]
        public async Task<ActionResult> CreateZaloGroupAsync([FromBody] NhomZaloDTO zaloDTO)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.AddNewZaloAsync(user, zaloDTO);
                return Ok("Zalo group created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateZaloGroupAsync(string id, [FromBody] NhomZaloDTO zaloDTO)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.UpdateZaloAsync(id, user, zaloDTO);
                return Ok("Zalo group updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteZaloGroupAsync(string id)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.DeleteZaloAsync(id, user);
                return Ok("Zalo group deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Manage User in ZaloGroup methods
        [HttpGet("get/{nhomZaloId}/users")]
        public async Task<ActionResult<List<UserNhomZalo>>> GetUsersInZaloGroupAsync(string nhomZaloId)
        {
            try
            {
                var users = await _nhomZaloService.GetUsersInGroupAsync(nhomZaloId);
                if (users is List<UserNhomZalo> userNhomZaloList)
                {
                    var formattedResponse = userNhomZaloList.Select(userNhomZalo => new UserNhomZaloDTO
                    {
                        UserId = userNhomZalo.UserId,
                        UserName = userNhomZalo.User?.HoVaTen,
                        NhomZalo = userNhomZalo.NhomZalo?.TenNhom,
                        JoinedTime = userNhomZalo.JoinedTime,
                        LeftTime = userNhomZalo.LeftTime
                    }).ToList();

                    return Ok(formattedResponse);
                }

                return users;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("get-user-in-zalo-group/{nhomZaloId}/users/{userId}")]
        public async Task<ActionResult<UserNhomZalo>> GetUserInZaloGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                var user = await _nhomZaloService.GetUserInGroupAsync(nhomZaloId, userId);

                if (user == null)
                    return NotFound($"User with ID {userId} not found in Zalo group {nhomZaloId}.");

                var formattedResponse = new UserNhomZaloDTO
                {
                    UserId = user.UserId,
                    UserName = user.User?.HoVaTen,
                    NhomZalo = user.NhomZalo?.TenNhom,
                    JoinedTime = user.JoinedTime,
                    LeftTime = user.LeftTime
                };

                return Ok(formattedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("add-user-to-zalo/{nhomZaloId}/users")]
        public async Task<ActionResult> AddUserToZaloGroupAsync(string nhomZaloId, [FromBody] UserNhomZaloDTO userDTO)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.AddUserToGroupAsync(nhomZaloId, user, userDTO);
                return Ok($"User added to Zalo group {nhomZaloId} successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("update-user-in-zalo-group/{nhomZaloId}/users")]
        public async Task<ActionResult> UpdateUserInZaloGroupAsync(string nhomZaloId, [FromBody] UserNhomZaloDTO updatedUserDTO)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.UpdateUserInGroupAsync(nhomZaloId, user, updatedUserDTO);
                return Ok($"User in Zalo group {nhomZaloId} updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete-user-in-zalo-group/{nhomZaloId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromZaloGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.RemoveUserFromGroupAsync(nhomZaloId, user, userId);
                return Ok($"User removed from Zalo group {nhomZaloId} successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
