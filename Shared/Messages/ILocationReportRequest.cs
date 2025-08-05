namespace Shared.Messages
{
    public interface ILocationReportRequest
    {
        Guid ReportId { get; }
        string Location { get; }
        DateTime RequestedDate { get; }
    }
}
