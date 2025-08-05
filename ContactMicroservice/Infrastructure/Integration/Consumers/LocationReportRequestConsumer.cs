using ContactMicroservice.Domain.Interfaces.Repositories;
using MassTransit;
using Shared.Messages;

namespace ContactMicroservice.Infrastructure.Integration.Consumers
{
    public class LocationReportRequestConsumer : IConsumer<ILocationReportRequest>
    {
        private readonly IPersonRepository _personRepository;

        public LocationReportRequestConsumer(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task Consume(ConsumeContext<ILocationReportRequest> context)
        {
            var message = context.Message;

            var location = message.Location;

            int personCount = await _personRepository.GetCountByLocationAsync(location);

            int phoneCount = await _personRepository.GetPhoneCountByLocationAsync(location);

            var reportResponse = new
            {
                ReportId = message.ReportId,
                Location = location,
                PersonCount = personCount,
                PhoneCount = phoneCount,
                RequestedDate = message.RequestedDate,
                ReportStatus = "Completed"
            };

            await context.RespondAsync<ILocationReportResponse>(reportResponse);

            await context.Publish<ILocationReportResponse>(reportResponse);
        }
    }
}
