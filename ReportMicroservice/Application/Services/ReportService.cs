using ReportMicroservice.Application.Interfaces;
using ReportMicroservice.Domain.Entities;
using ReportMicroservice.Domain.Interfaces.Repositories;

namespace ReportMicroservice.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<Report> CreateReportAsync(Report report)
        {
            await _reportRepository.CreateAsync(report);
            return report;
        }

        public Task<List<Report>> GetReportsAsync() => _reportRepository.GetAllAsync();

        public Task<Report> GetReportByIdAsync(Guid id) => _reportRepository.GetByIdAsync(id);

        public Task UpdateReportAsync(Report report) => _reportRepository.UpdateAsync(report);
    }

}
