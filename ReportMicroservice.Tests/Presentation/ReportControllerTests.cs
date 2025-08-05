using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReportMicroservice.Application.Interfaces;
using ReportMicroservice.Domain.Entities;
using ReportMicroservice.Presentation.Controllers;
using Shared.Messages;

namespace ReportMicroservice.Tests.Presentation
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportService> _reportServiceMock;
        private readonly Mock<IRequestClient<ILocationReportRequest>> _requestClientMock;
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _reportServiceMock = new Mock<IReportService>();
            _requestClientMock = new Mock<IRequestClient<ILocationReportRequest>>();
            _controller = new ReportController(_reportServiceMock.Object, _requestClientMock.Object);
        }

        [Fact]
        public async Task GetReports_ReturnsOkWithReports()
        {
            var reports = new List<Report>
            {
                new Report { Id = Guid.NewGuid(), Location = "Location1" },
                new Report { Id = Guid.NewGuid(), Location = "Location2" }
            };
            _reportServiceMock.Setup(s => s.GetReportsAsync()).ReturnsAsync(reports);

            var result = await _controller.GetReports();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedReports = Assert.IsAssignableFrom<IEnumerable<Report>>(okResult.Value);
            Assert.Equal(2, returnedReports.Count());
        }

        [Fact]
        public async Task GetReportById_ReturnsOk_WhenReportExists()
        {
            var reportId = Guid.NewGuid();
            var report = new Report { Id = reportId, Location = "TestLocation" };
            _reportServiceMock.Setup(s => s.GetReportByIdAsync(reportId)).ReturnsAsync(report);

            var result = await _controller.GetReportById(reportId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedReport = Assert.IsType<Report>(okResult.Value);
            Assert.Equal(reportId, returnedReport.Id);
        }

        [Fact]
        public async Task GetReportById_ReturnsNotFound_WhenReportDoesNotExist()
        {
            var reportId = Guid.NewGuid();
            _reportServiceMock.Setup(s => s.GetReportByIdAsync(reportId)).ReturnsAsync((Report)null);

            var result = await _controller.GetReportById(reportId);

            Assert.IsType<NotFoundResult>(result);
        }

    }
}
