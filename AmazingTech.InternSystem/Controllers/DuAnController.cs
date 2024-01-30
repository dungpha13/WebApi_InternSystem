using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Services;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/du-ans")]
    [ApiController]
    //[Authorize(Roles = "Admin,Hr,Mentorn")]
    public class DuAnController : ControllerBase
    {
        private readonly IDuAnService _duAnService;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public DuAnController(IDuAnService duAnService, AppDbContext dbContext, IMapper mapper)
        {
            _duAnService = duAnService;
            _dbContext = dbContext;
            _mapper = mapper;

        }

        [HttpGet("get")]
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

        [HttpGet("get/{id}")]
        public IActionResult GetDuAnById(string id)
        {
            var result = _duAnService.GetDuAnById(id);

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

            return result;
            //return Ok(result);
        }

        [HttpGet("search")]
        public IActionResult SearchProject(string? ten, string? leaderName)
        {
            try
            {
                var duAns = _duAnService.SearchProject(ten, leaderName);
                if (duAns is OkObjectResult okResult)
                {
                    var duAn = okResult.Value as DuAn;

                    if (duAn != null)
                    {
                        var formattedResponse = new
                        {
                            ten = duAn.Ten,
                            leaderId = duAn.LeaderId,
                            leaderName = duAn.Leader.HoVaTen,
                            thoiGianBatDau = duAn.ThoiGianBatDau,
                            thoiGianKetThuc = duAn.ThoiGianKetThuc
                        };

                        return Ok(formattedResponse);
                    }
                }
                return duAns;
            }
            catch (Exception ex)
            {
                return BadRequest("We can't get the project.");
            }
        }

        [HttpPost("create")]
        public IActionResult CreateDuAn([FromBody] DuAnModel createDuAn)
        {
            try
            {
                var result = _duAnService.CreateDuAn(createDuAn);

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
                    return StatusCode(500, "Internal Server Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateDuAn(string id, [FromBody] DuAnModel updatedDuAn)
        {
            try
            {
                _duAnService.UpdateDuAn(id, updatedDuAn);
                return Ok("DuAn updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDuAn(string id)
        {
            try
            {
                _duAnService.DeleteDuAn(id);
                return Ok("DuAn deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("excel-export")]
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
                    sheet1.Column(1).Style.Font.FontColor = XLColor.Red;
                    sheet1.Columns(2, 4).Style.Font.FontColor = XLColor.Blue;

                    sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
                    sheet1.Row(1).Style.Font.FontColor = XLColor.White;
                    sheet1.Row(1).Style.Font.Bold = true;
                    sheet1.Row(1).Style.Font.Shadow = true;
                    sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                    sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                    sheet1.Row(1).Style.Font.Italic = true;

                    sheet1.Rows(2, 3).Style.Font.FontColor = XLColor.Black;

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
                // Log the exception
                Console.WriteLine($"Error exporting projects to Excel: {ex.Message}");
                return StatusCode(500, "An error occurred while exporting projects to Excel.");
            }
        }
    }
}
