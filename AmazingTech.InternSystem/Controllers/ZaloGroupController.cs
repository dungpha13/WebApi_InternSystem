using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO.NhomZalo;
using AmazingTech.InternSystem.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/group-zalos")]
    [ApiController]
    //[Authorize(Roles = "Admin,HR")]
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
                    return NotFound($"ZaloGroup with ID ({id}) not found.");

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
                return Ok("ZaloGroup created successfully");
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
                    return NotFound($"ZaloGroup with ID ({id}) not found.");
                }
                await _nhomZaloService.UpdateZaloAsync(id, user, zaloDTO);
                return Ok("ZaloGroup updated successfully");
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
                    return NotFound($"ZaloGroup with ID ({id}) not found.");
                }
                await _nhomZaloService.DeleteZaloAsync(id, user);
                return Ok("ZaloGroup deleted successfully");
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
                var nhomZalo = await _nhomZaloService.GetGroupByIdAsync(nhomZaloId);
                if (nhomZalo == null)
                {
                    return NotFound($"ZaloGroup with ID ({nhomZaloId}) not found.");
                }

                //var nhomZalo = _dbContext.UserNhomZalos.FirstOrDefault(x => x.IdNhomZalo == nhomZaloId && x.DeletedTime == null);
                //if (nhomZalo == null)
                //{
                //    return NotFound($"ZaloGroup with ID ({nhomZaloId}) not found.");
                //}

                var users = await _nhomZaloService.GetUsersInGroupAsync(nhomZaloId);
                if (users.Count == 0)
                {
                    return BadRequest($"ZaloGroup with ID ({nhomZaloId}) exists but does not have any members.");
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

        [HttpPost("add-user-to-zalo-group/{idNhomZaloChung}/{idNhomZaloRieng}")]
        public async Task<ActionResult> AddUserToZaloGroupAsync(string idNhomZaloChung, string idNhomZaloRieng, [FromBody] AddUserNhomZaloDTO addUserDTO)
        {
            try
            {
                var nhomZaloChung = await _nhomZaloService.GetGroupByIdAsync(idNhomZaloChung);
                if (nhomZaloChung == null)
                {
                    return NotFound($"ZaloGroup with ID ({idNhomZaloChung}) not found.");
                }

                var nhomZaloRieng = await _nhomZaloService.GetGroupByIdAsync(idNhomZaloRieng);
                if (nhomZaloRieng == null)
                {
                    return NotFound($"ZaloGroup with ID ({idNhomZaloRieng}) not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.AddUserToGroupAsync(idNhomZaloChung, idNhomZaloRieng, user, addUserDTO);
                return Ok($"User added to ZaloGroup '{nhomZaloChung.TenNhom}' and '{nhomZaloRieng.TenNhom}' successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                    return NotFound($"ZaloGroup with ID ({nhomZaloId}) not found.");
                }

                var users = await _nhomZaloService.GetUsersInGroupAsync(nhomZaloId);
                if (users.Count == 0)
                {
                    return BadRequest($"ZaloGroup with ID ({nhomZaloId}) exists but does not have any members.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _nhomZaloService.UpdateUserInGroupAsync(nhomZaloId, user, updatedUserDTO);
                return Ok($"User in ZaloGroup '{nhomZalo.TenNhom}' updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                    return NotFound($"ZaloGroup with ID ({nhomZaloId}) not found.");
                }

                var users = await _nhomZaloService.GetUsersInGroupAsync(nhomZaloId);
                if (users.Count == 0)
                {
                    return BadRequest($"ZaloGroup with ID ({nhomZaloId}) exists but does not have any members.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _nhomZaloService.RemoveUserFromGroupAsync(nhomZaloId, user, userId);
                return Ok($"User removed from ZaloGroup '{nhomZalo.TenNhom}' successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
