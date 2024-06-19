using Application.Services.RabbitMQClientService;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Reports.Queries.GetById;

public class GetByIdReportResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid CounterId { get; set; }
    public DateTime RequestedDate { get; set; }
    public string StatusCode { get; set; }
    public CounterDetailsDto CounterDetails { get; set; }
}