using ReportMicroservice.Domain.Entities;

namespace ReportMicroservice.Domain.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task CreateAsync(Report report);
        Task<List<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(Guid id);
        Task UpdateAsync(Report report);
    }
}
