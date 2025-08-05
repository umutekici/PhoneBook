namespace Shared.Messages
{
    public interface ILocationReportResponse
    {
        Guid ReportId { get; }
        string Location { get; }
        int PersonCount { get; }
        int PhoneCount { get; }
        DateTime RequestedDate { get; }
        string ReportStatus { get; }
    }
}
