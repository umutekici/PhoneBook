using ReportMicroservice.Domain.Entities;

namespace ReportMicroservice.Application.Interfaces
{
    public interface IReportService
    {
        Task<Report> CreateReportAsync(Report dto);
        Task<List<Report>> GetReportsAsync();
        Task<Report> GetReportByIdAsync(Guid id);
        Task UpdateReportAsync(Report report);
    }

}
