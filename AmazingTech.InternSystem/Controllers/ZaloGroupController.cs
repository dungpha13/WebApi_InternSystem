﻿using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Models.DTO.NhomZalo;
using AmazingTech.InternSystem.Services;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        //[HttpGet("get/{id}")]
        //public async Task<ActionResult<NhomZaloDTO>> GetZaloGroupByIdAsync(string id)
        //{
        //    try
        //    {
        //        var group = await _nhomZaloService.GetGroupByIdAsync(id);

        //        if (group == null)
        //            return NotFound($"Zalo group with ID {id} not found.");

        //        var formattedResponse = new NhomZaloDTO
        //        {
        //            TenNhom = group.TenNhom,
        //            LinkNhom = group.LinkNhom,
        //        };

        //        return Ok(formattedResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}


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
                    return NotFound($"Zalo group with ID ({id}) not found.");
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
                var group = await _nhomZaloService.GetGroupByIdAsync(id);
                if (group == null)
                {
                    return NotFound($"Zalo group with ID ({id}) not found.");
                }
                await _nhomZaloService.DeleteZaloAsync(id, user);
                return Ok("Zalo group deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("zalo-groups-excel-export")]
        public async Task<ActionResult> ExportZaloGroupsToExcelAsync()
        {
            try
            {
                var zaloGroups = await _dbContext.NhomZalos
                    .Where(zl => zl.DeletedTime == null)
                    .OrderByDescending(zl => zl.CreatedTime)
                    .ToListAsync();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var sheet1 = wb.AddWorksheet("ZaloGroups");

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

                    sheet1.Column(2).Style.Font.Bold = true;

                    sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.BlueGray;
                    sheet1.Row(1).Style.Font.FontColor = XLColor.Black;
                    sheet1.Row(1).Style.Font.Bold = true;
                    sheet1.Row(1).Style.Font.Shadow = true;
                    sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                    //sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                    sheet1.Row(1).Style.Font.Italic = true;

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
                Console.WriteLine($"Error exporting ZaloGroups to Excel: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting ZaloGroups to Excel.");
            }
        }

        // Manage User in ZaloGroup methods
        [HttpGet("get-all-users-in-zalo-group/{nhomZaloId}")]
        public async Task<ActionResult<List<UserNhomZalo>>> GetUsersInZaloGroupAsync(string nhomZaloId)
        {
            try
            {
                var users = await _nhomZaloService.GetUsersInGroupAsync(nhomZaloId);

                if (users == null)
                {
                    return NotFound();
                }

                //var nhomZalo = await _nhomZaloService.GetGroupByIdAsync(nhomZaloId);

                //if (nhomZalo == null)
                //{
                //    return NotFound($"Zalo group with ID ({nhomZaloId}) not found.");
                //}

                var nhomZalo = _dbContext.UserNhomZalos.FirstOrDefault(x => x.IdNhomZalo == nhomZaloId && x.DeletedTime == null);
                if (nhomZalo == null)
                {
                    return NotFound($"Zalo group with ID ({nhomZaloId}) not found.");
                }

                if (users is List<UserNhomZalo> userNhomZaloList)
                {
                    var formattedResponse = userNhomZaloList.Select(userNhomZalo => new UserNhomZaloDTO
                    {
                        UserId = userNhomZalo.UserId,
                        UserName = userNhomZalo.User.HoVaTen,
                        NhomZalo = userNhomZalo.NhomZalo.TenNhom,
                        IsMentor = userNhomZalo.IsMentor,
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

        //[HttpGet("get-user-in-zalo-group/{nhomZaloId}/users/{userId}")]
        //public async Task<ActionResult<UserNhomZalo>> GetUserInZaloGroupAsync(string nhomZaloId, string userId)
        //{
        //    try
        //    {
        //        var user = await _nhomZaloService.GetUserInGroupAsync(nhomZaloId, userId);

        //        if (user == null)
        //            return NotFound($"User with ID {userId} not found in Zalo group {nhomZaloId}.");

        //        var formattedResponse = new UpdateUserNhomZaloDTO
        //        {
        //            UserId = user.UserId,
        //            UserName = user.User.HoVaTen,
        //            IsMentor = user.IsMentor,
        //            //IdNhomZalo = user.IdNhomZalo,
        //            //NhomZalo = user.NhomZalo.TenNhom,
        //            JoinedTime = user.JoinedTime,
        //            LeftTime = user.LeftTime
        //        };

        //        return Ok(formattedResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        [HttpPost("add-user-to-zalo-group/{nhomZaloId}")]
        public async Task<ActionResult> AddUserToZaloGroupAsync(string nhomZaloId, [FromBody] AddUserNhomZaloDTO addUserDTO)
        {
            try
            {
                //var nhomZalo = await _nhomZaloService.GetGroupByIdAsync(nhomZaloId);
                //if (nhomZalo == null)
                //{
                //    return NotFound($"Zalo group with ID ({nhomZaloId}) not found.");
                //}

                var nhomZalo = _dbContext.NhomZalos.Find(nhomZaloId);
                if (nhomZalo == null)
                {
                    return NotFound($"GroupZalo with ID ({nhomZaloId}) not found.");
                }

                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == addUserDTO.UserId && u.DeletedTime == null);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {addUserDTO.UserId} not found.");
                }

                var userZaloGroup = _dbContext.UserNhomZalos.FirstOrDefault(x => x.UserId == addUserDTO.UserId
                                                                              && x.IdNhomZalo == nhomZaloId
                                                                              && x.DeletedTime == null);
                if (userZaloGroup == null)
                {
                    return NotFound($"UserDuAn with ID ({addUserDTO.UserId}) in GroupZalo not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _nhomZaloService.AddUserToGroupAsync(nhomZaloId, user, addUserDTO);
                if (result == null)
                {
                    return Ok($"User added to Zalo group '{nhomZalo.TenNhom}' successfully");
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("update-user-in-zalo-group/{nhomZaloId}")]
        public async Task<ActionResult> UpdateUserInZaloGroupAsync(string nhomZaloId, [FromBody] UpdateUserNhomZaloDTO updatedUserDTO)
        {
            try
            {
                var nhomZalo = await _nhomZaloService.GetGroupByIdAsync(nhomZaloId);

                if (nhomZalo == null)
                {
                    return NotFound($"Zalo group with ID ({nhomZaloId}) not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _nhomZaloService.UpdateUserInGroupAsync(nhomZaloId, user, updatedUserDTO);
                if (result == null)
                {
                    return Ok($"User in Zalo group '{nhomZalo.TenNhom}' updated successfully");
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete-user-from-zalo-group/{nhomZaloId}/{userId}")]
        public async Task<ActionResult> RemoveUserFromZaloGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                var nhomZalo = await _nhomZaloService.GetGroupByIdAsync(nhomZaloId);

                if (nhomZalo == null)
                {
                    return NotFound($"Zalo group with ID ({nhomZaloId}) not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _nhomZaloService.RemoveUserFromGroupAsync(nhomZaloId, user, userId);
                if (result == null)
                {
                    return Ok($"User removed from Zalo group '{nhomZalo.TenNhom}' successfully");
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
