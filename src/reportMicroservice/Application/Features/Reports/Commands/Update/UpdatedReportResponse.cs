using NArchitecture.Core.Application.Responses;

namespace Application.Features.Reports.Commands.Update;

public class UpdatedReportResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid CounterId { get; set; }
    public DateTime RequestedDate { get; set; }
    public string StatusCode { get; set; }
}