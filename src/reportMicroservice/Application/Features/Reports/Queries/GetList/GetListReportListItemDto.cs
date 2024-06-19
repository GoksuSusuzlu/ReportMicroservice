using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Reports.Queries.GetList;

public class GetListReportListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid CounterId { get; set; }
    public DateTime RequestedDate { get; set; }
    public string StatusCode { get; set; }
}