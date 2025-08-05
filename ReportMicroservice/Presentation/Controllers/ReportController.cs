using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ReportMicroservice.Application.DTOs;
using ReportMicroservice.Application.Interfaces;
using ReportMicroservice.Domain.Entities;
using ReportMicroservice.Domain.Enums;
using Shared.Messages;

namespace ReportMicroservice.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IRequestClient<ILocationReportRequest> _locationReportRequestClient;

        public ReportController(IReportService reportService, IRequestClient<ILocationReportRequest> locationReportRequestClient)
        {
            _reportService = reportService;
            _locationReportRequestClient = locationReportRequestClient;
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

        [HttpPost("location-report")]
        public async Task<IActionResult> RequestReport([FromBody] ReportLocationDto reportLocationDto)
        {
            var reportId = Guid.NewGuid();
            var requestedDate = DateTime.UtcNow;

            await _reportService.CreateReportAsync(new Report
            {
                Id = reportId,
                Location = reportLocationDto.Location,
                RequestedDate = requestedDate,
                Status = ReportStatus.Preparing
            });

            var response = await _locationReportRequestClient.GetResponse<ILocationReportResponse>(new
            {
                ReportId = reportId,
                Location = reportLocationDto.Location,
                RequestedDate = requestedDate
            });

            return Ok(response.Message);
        }
    }
}
