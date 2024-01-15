using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DuAnController : ControllerBase
    {
        private readonly IDuAnService _duAnService;
        private readonly AppDbContext _dbContext;

        public DuAnController(IDuAnService duAnService, AppDbContext dbContext)
        {
            _duAnService = duAnService;
            _dbContext = dbContext;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<DuAn>>> SearchProjectsAsync([FromQuery] DuAnFilterCriteria criteria)
        {
            try
            {
                var projects = await _duAnService.SearchProjectsAsync(criteria);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while searching for projects.");
            }
        }

        [HttpPost("clean-filters")]
        public async Task<ActionResult> CleanFiltersAsync()
        {
            try
            {
                await _duAnService.CleanFiltersAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while cleaning filters.");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateDuAnAsync([FromBody] CreateDuAnRequest request)
        {
            try
            {
                var createdDuAnId = await _duAnService.CreateDuAnAsync(request.CreatedDuAn, request.ViTriIds, request.CongNgheIds, request.LeaderUserIds);
                return CreatedAtAction(nameof(CreateDuAnAsync), new { id = createdDuAnId }, new { Id = createdDuAnId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the project.");
            }
        }

        [HttpPost("update/{id}")]
        public async Task<ActionResult> UpdateDuAnAsync(string id, [FromBody] UpdateDuAnRequest request)
        {
            try
            {
                await _duAnService.UpdateDuAnAsync(id, request.UpdatedDuAn, request.ViTriIds, request.CongNgheIds, request.LeaderUserIds);
                return Ok("Project updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the project.");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteDuAnAsync(string id)
        {
            try
            {
                await _duAnService.DeleteDuAnAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the project with ID {id}.");
            }
        }

        [HttpGet("get-for-edit/{id}")]
        public async Task<ActionResult<DuAn>> GetDuAnForEditAsync(string id)
        {
            try
            {
                var duAn = await _duAnService.GetDuAnForEditAsync(id);
                if (duAn == null)
                {
                    return NotFound($"Project with ID {id} not found.");
                }
                return Ok(duAn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting project details for editing with ID {id}.");
            }
        }

        [HttpGet("ExportExcel")]
        public async Task<ActionResult> ExportProjectsToExcelAsync()
        {
            try
            {
                var projects = await _dbContext.DuAns
                    .Include(d => d.ViTris)
                    .Include(d => d.CongNghes)
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
                        sheet1.Cell(row, 5).Value = project.Leader?.UserName;

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

                    sheet1.Rows(2, 3).Style.Font.FontColor = XLColor.AshGrey;

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
