using MongoDB.Driver;
using ReportMicroservice.Domain.Entities;
using ReportMicroservice.Domain.Interfaces.Repositories;

namespace ReportMicroservice.Infrastructure.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoCollection<Report> _report;
        public ReportRepository(IMongoDatabase database)
        {
            _report = database.GetCollection<Report>("Reports");
        }
        public async Task CreateAsync(Report report) => await _report.InsertOneAsync(report);

        public async Task<List<Report>> GetAllAsync() => await _report.Find(_ => true).ToListAsync();

        public async Task<Report> GetByIdAsync(Guid id) =>
            await _report.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(Report report) =>
            await _report.ReplaceOneAsync(r => r.Id == report.Id, report);
    }
}
