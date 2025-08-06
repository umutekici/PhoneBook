using Moq;
using ReportMicroservice.Application.Services;
using ReportMicroservice.Domain.Entities;
using ReportMicroservice.Domain.Interfaces.Repositories;

namespace ReportMicroservice.Tests.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IReportRepository> _reportRepositoryMock;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _reportRepositoryMock = new Mock<IReportRepository>();
            _reportService = new ReportService(_reportRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateReportAsync_CallsRepositoryCreateAndReturnsReport()
        {
            var report = new Report { Id = Guid.NewGuid(), Location = "Test" };

            _reportRepositoryMock
                .Setup(r => r.CreateAsync(report))
                .Returns(Task.CompletedTask);

            var result = await _reportService.CreateReportAsync(report);

            _reportRepositoryMock.Verify(r => r.CreateAsync(report), Times.Once);
            Assert.Equal(report, result);
        }

        [Fact]
        public async Task GetReportsAsync_ReturnsListOfReports()
        {
            var reports = new List<Report>
            {
                new Report { Id = Guid.NewGuid(), Location = "Test" },
                new Report { Id = Guid.NewGuid(), Location = "Test 2" }
            };

            _reportRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(reports);

            var result = await _reportService.GetReportsAsync();

            Assert.Equal(reports, result);
        }

        [Fact]
        public async Task GetReportByIdAsync_ReturnsReport()
        {
            var id = Guid.NewGuid();
            var report = new Report { Id = id, Location = "Test" };

            _reportRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(report);

            var result = await _reportService.GetReportByIdAsync(id);

            Assert.Equal(report, result);
        }

        [Fact]
        public async Task UpdateReportAsync_CallsRepositoryUpdate()
        {
            var report = new Report { Id = Guid.NewGuid(), Location = "Test" };

            _reportRepositoryMock
                .Setup(r => r.UpdateAsync(report))
                .Returns(Task.CompletedTask);

            await _reportService.UpdateReportAsync(report);

            _reportRepositoryMock.Verify(r => r.UpdateAsync(report), Times.Once);
        }
    }
}
