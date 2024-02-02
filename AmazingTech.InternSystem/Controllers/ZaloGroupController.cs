using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _dbContext;

        public ZaloGroupController(INhomZaloService nhomZaloService, AppDbContext dbContext)
        {
            _nhomZaloService = nhomZaloService;
            _dbContext = dbContext;
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
                var group = await _nhomZaloService.GetGroupByIdAsync(id);
                if (group == null)
                {
                    return NotFound($"Zalo group with ID {id} not found.");
                }
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

        [HttpGet("excel-export")]
        public async Task<ActionResult> ExportZaloGroupsToExcelAsync()
        {
            try
            {
                var zaloGroups = await _dbContext.NhomZalos
                    .ToListAsync();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var sheet1 = wb.AddWorksheet("Projects");

                    sheet1.Cell(1, 1).Value = "Group ID";
                    sheet1.Cell(1, 2).Value = "Group Name";
                    sheet1.Cell(1, 3).Value = "Link Group";

                    int row = 2;
                    foreach (var group in zaloGroups)
                    {
                        sheet1.Cell(row, 1).Value = group.Id;
                        sheet1.Cell(row, 2).Value = group.TenNhom;
                        sheet1.Cell(row, 3).Value = group.LinkNhom;

                        row++;
                    }

                    // Apply styling
                    sheet1.Columns(1, 2).Style.Font.FontColor = XLColor.Black;
                    sheet1.Column(3).Style.Font.FontColor = XLColor.Blue;

                    sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.AntiFlashWhite;
                    sheet1.Row(1).Style.Font.FontColor = XLColor.Black;
                    sheet1.Row(1).Style.Font.Bold = true;
                    sheet1.Row(1).Style.Font.Shadow = true;
                    sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                    sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                    sheet1.Row(1).Style.Font.Italic = true;

                    sheet1.Rows(2, 3).Style.Font.FontColor = XLColor.Black;

                    // Apply borders to all cells
                    var range = sheet1.RangeUsed();
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Autofit 
                    sheet1.Rows().AdjustToContents();
                    sheet1.Columns().AdjustToContents();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        wb.SaveAs(ms);
                        var fileName = "ZaloGroups.xlsx";
                        return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error exporting projects to Excel: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting projects to Excel.");
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
                        UserName = userNhomZalo.User.HoVaTen,
                        IsMentor = userNhomZalo.IsMentor,
                        IdNhomZalo = userNhomZalo.IdNhomZalo,
                        NhomZalo = userNhomZalo.NhomZalo.TenNhom,
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
                    UserName = user.User.HoVaTen,
                    IsMentor = user.IsMentor,
                    IdNhomZalo = user.IdNhomZalo,
                    NhomZalo = user.NhomZalo.TenNhom,
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
