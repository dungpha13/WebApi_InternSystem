using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Request.User;
using AmazingTech.InternSystem.Services;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/du-ans")]
    [ApiController]
    //[Authorize(Roles = "Admin,Hr,Mentorn")]
    public class DuAnController : ControllerBase
    {
        private readonly IDuAnService _duAnService;
        private readonly AppDbContext _dbContext;

        public DuAnController(IDuAnService duAnService, AppDbContext dbContext)
        {
            _duAnService = duAnService;
            _dbContext = dbContext;
        }

        //Manage DuAn
        [HttpGet("get-all-projects")]
        public IActionResult GetAllDuAns()
        {
            var result = _duAnService.GetAllDuAns();

            if (result is OkObjectResult okResult)
            {
                var duAnList = okResult.Value as List<DuAn>;

                if (duAnList != null)
                {
                    var formattedResponse = new
                    {
                        value = duAnList.Select(duAn => new
                        {
                            id = duAn.Id,
                            ten = duAn.Ten,
                            leaderId = duAn.LeaderId,
                            leaderName = duAn.Leader.HoVaTen,
                            thoiGianBatDau = duAn.ThoiGianBatDau,
                            thoiGianKetThuc = duAn.ThoiGianKetThuc
                        })
                    };

                    return Ok(formattedResponse);
                }

            }
            return result;
        }

        [HttpGet("get-project/{id}")]
        public IActionResult GetDuAnById(string id)
        {
            var result = _duAnService.GetDuAnById(id);

            //if (result == null)
            //{
            //    return NotFound($"DuAn with ID {id} not found.");
            //}

            if (result is OkObjectResult okResult)
            {
                var duAn = okResult.Value as DuAn;

                if (duAn != null)
                {
                    var formattedResponse = new
                    {
                        id = duAn.Id,
                        ten = duAn.Ten,
                        leaderId = duAn.LeaderId,
                        leaderName = duAn.Leader.HoVaTen,
                        thoiGianBatDau = duAn.ThoiGianBatDau,
                        thoiGianKetThuc = duAn.ThoiGianKetThuc
                    };

                    return Ok(formattedResponse);
                }
            }

            //return result;
            return NotFound($"DuAn with ID {id} not found.");
            //return Ok(result);
        }

        [HttpGet("search-project")]
        public IActionResult SearchProject(string? ten, string? leaderName, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                //var duAns = _duAnService.SearchProject(ten, leaderName, startDate, endDate);
                //if (duAns is OkObjectResult okResult)
                //{
                //    var duAn = okResult.Value as DuAn;

                //    if (duAn != null)
                //    {
                //        var formattedResponse = new
                //        {
                //            ten = duAn.Ten,
                //            leaderId = duAn.LeaderId,
                //            leaderName = duAn.Leader.HoVaTen,
                //            thoiGianBatDau = duAn.ThoiGianBatDau,
                //            thoiGianKetThuc = duAn.ThoiGianKetThuc
                //        };

                //        return Ok(formattedResponse);
                //    }
                //}

                //return duAns;
                var duAns = _duAnService.SearchProject(ten, leaderName, startDate, endDate);

                if (duAns is OkObjectResult okResult)
                {
                    var duAnList = okResult.Value as List<DuAnModel>;

                    if (duAnList != null && duAnList.Count > 0)
                    {
                        return Ok(duAnList);
                    }
                    else
                    {
                        return NotFound("No projects found matching the search criteria.");
                    }
                }
                else
                {
                    return duAns;
                }
            }
            catch (Exception ex)
            {
                return BadRequest("We can't get the project.");
            }
        }

        [HttpPost("create-project")]
        public IActionResult CreateDuAn([FromBody] DuAnModel createDuAn)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = _duAnService.CreateDuAn(user, createDuAn);

                if (result is BadRequestObjectResult badRequestResult)
                {
                    return badRequestResult;
                }
                else if (result is OkResult)
                {
                    return Ok("DuAn created successfully");
                }
                else
                {
                    return BadRequest("Failed to create DuAn");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("update-project/{id}")]
        public IActionResult UpdateDuAn(string id, [FromBody] DuAnModel updatedDuAn)
        {
            try
            {
                var existingProject = _duAnService.GetDuAnById(id);
                if (existingProject == null)
                {
                    return BadRequest($"DuAn with ID {id} not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //_duAnService.UpdateDuAn(id, user, updatedDuAn);
                //return Ok("DuAn updated successfully");
                var result = _duAnService.UpdateDuAn(id, user, updatedDuAn);

                if (result is BadRequestObjectResult badRequestResult)
                {
                    return badRequestResult;
                }
                else if (result is OkResult)
                {
                    return Ok("DuAn updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update DuAn");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete-project/{id}")]
        public IActionResult DeleteDuAn(string id)
        {
            try
            {
                var existingProject = _duAnService.GetDuAnById(id);
                if (existingProject == null)
                {
                    return BadRequest($"DuAn with ID {id} not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                //_duAnService.DeleteDuAn(id, user);
                //return Ok("DuAn deleted successfully");
                var result = _duAnService.DeleteDuAn(id, user);

                if (result is BadRequestObjectResult badRequestResult)
                {
                    return badRequestResult;
                }
                else if (result is OkResult)
                {
                    return Ok("DuAn deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete DuAn");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("project-excel-export")]
        public async Task<ActionResult> ExportProjectsToExcelAsync()
        {
            try
            {
                var projects = await _dbContext.DuAns
                    //.Include(d => d.ViTris)
                    //.Include(d => d.CongNgheDuAns)
                    .Include(d => d.Leader)
                    .ToListAsync();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var sheet1 = wb.AddWorksheet("Projects");

                    sheet1.Cell(1, 1).Value = "Project ID";
                    sheet1.Cell(1, 2).Value = "Project Name";
                    sheet1.Cell(1, 3).Value = "Start Date";
                    sheet1.Cell(1, 4).Value = "End Date";
                    sheet1.Cell(1, 5).Value = "Leader";

                    int row = 2;
                    foreach (var project in projects)
                    {
                        sheet1.Cell(row, 1).Value = project.Id;
                        sheet1.Cell(row, 2).Value = project.Ten;
                        sheet1.Cell(row, 3).Value = project.ThoiGianBatDau?.ToShortDateString();
                        sheet1.Cell(row, 4).Value = project.ThoiGianKetThuc?.ToShortDateString();
                        sheet1.Cell(row, 5).Value = project.Leader?.HoVaTen;

                        row++;
                    }

                    // Apply styling
                    sheet1.Columns(1, 2).Style.Font.FontColor = XLColor.Black;
                    sheet1.Columns(3, 4).Style.Font.FontColor = XLColor.Blue;
                    sheet1.Column(5).Style.Font.FontColor = XLColor.Red;

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
                        var fileName = "Projects.xlsx";
                        return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting projects to Excel: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting projects to Excel.");
            }
        }

        //Manage UserDuAn
        [HttpGet("get-all-user-in-project/{duAnId}")]
        public IActionResult GetAllUserDuAns(string duAnId)
        {
            if (string.IsNullOrEmpty(duAnId))
            {
                return BadRequest("DuAnId is required");
            }

            var result = _duAnService.GetAllUsersInDuAn(duAnId);

            if (result == null)
            {
                return NotFound();
            }

            if (result is OkObjectResult okResult)
            {
                var userDuAnList = okResult.Value as List<UserDuAn>;

                if (userDuAnList != null)
                {
                    var formattedResponse = new
                    {
                        value = userDuAnList.Select(userDuAn => new
                        {
                            userId = userDuAn.UserId,
                            userName = userDuAn.User.HoVaTen,
                            idDuAn = userDuAn.IdDuAn,
                            tenDuAn = userDuAn.DuAn.Ten,
                            viTri = userDuAn.ViTri,
                        })
                    };

                    return Ok(formattedResponse);
                }

            }

            return result;
        }

        [HttpPost("add-user-to-project/{duAnId}")]
        public IActionResult AddUserToDuAn(string duAnId, [FromBody] UserDuAnModel addUserDuAn)
        {

            try
            {
                if (string.IsNullOrEmpty(duAnId))
                {
                    return BadRequest("DuAnId is required");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _duAnService.AddUserToDuAn(duAnId, user, addUserDuAn);
                return Ok($"User added to DuAn {duAnId} successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

            //return _duAnService.AddUserToDuAn(duAnId, HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), addUserDuAn);
        }

        [HttpPut("update-user-in-project/{duAnId}")]
        public IActionResult UpdateUserInDuAn(string duAnId, [FromBody] UserDuAnModel updateUserDuAn)
        {
            try
            {
                if (string.IsNullOrEmpty(duAnId))
                {
                    return BadRequest("DuAnId is required");
                }

                var existingProject = _duAnService.GetDuAnById(duAnId);
                if (existingProject == null)
                {
                    return NotFound($"DuAn with ID {duAnId} not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _duAnService.UpdateUserInDuAn(duAnId, user, updateUserDuAn);
                return Ok($"User added to DuAn {duAnId} updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

            //return _duAnService.UpdateUserInDuAn(duAnId, HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), updateUserDuAn);
        }

        [HttpDelete("delete-user-from-project/{duAnId}/{userId}")]
        public IActionResult DeleteUserFromDuAn(string userId, string duAnId)
        {
            try
            {
                if (string.IsNullOrEmpty(duAnId))
                {
                    return BadRequest("DuAnId is required");
                }

                var existingProject = _duAnService.GetDuAnById(duAnId);
                if (existingProject == null)
                {
                    return NotFound($"DuAn with ID {duAnId} not found.");
                }

                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                _duAnService.DeleteUserFromDuAn(duAnId, user, userId);
                return Ok($"User removed from DuAn {duAnId} successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

            //return _duAnService.DeleteUserFromDuAn(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), userId, duAnId);
        }
    }
}
