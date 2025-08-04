using Microsoft.AspNetCore.Mvc;
using ReportMicroservice.Application.DTOs;
using ReportMicroservice.Application.Interfaces;

namespace ReportMicroservice.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportDto dto)
        {
            var report = await _reportService.CreateReportAsync(dto);
            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report);
        }

        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportService.GetReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
                return NotFound();
            return Ok(report);
        }
    }
}
