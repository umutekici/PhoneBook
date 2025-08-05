using MassTransit;
using ReportMicroservice.Domain.Entities;
using ReportMicroservice.Domain.Enums;
using ReportMicroservice.Domain.Interfaces.Repositories;
using Shared.Messages;

namespace ReportMicroservice.Infrastructure.Integration.Consumers
{
    public class LocationReportCompletedConsumer : IConsumer<ILocationReportResponse>
    {
        private readonly IReportRepository _reportRepository;

        public LocationReportCompletedConsumer(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task Consume(ConsumeContext<ILocationReportResponse> context)
        {
            var message = context.Message;

            var existingReport = await _reportRepository.GetByIdAsync(message.ReportId);

            if (existingReport != null)
            {
                existingReport.Location = message.Location;
                existingReport.PersonCount = message.PersonCount;
                existingReport.PhoneCount = message.PhoneCount;
                existingReport.RequestedDate = message.RequestedDate;
                existingReport.Status = ReportStatus.Completed;

                await _reportRepository.UpdateAsync(existingReport);
            }
            else
            {
                var newReport = new Report
                {
                    Id = message.ReportId,
                    Location = message.Location,
                    PersonCount = message.PersonCount,
                    PhoneCount = message.PhoneCount,
                    RequestedDate = message.RequestedDate,
                    Status = ReportStatus.Completed
                };

                await _reportRepository.CreateAsync(newReport);
            }
        }
    }
}
