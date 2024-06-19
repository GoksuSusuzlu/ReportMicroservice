using Application.Features.Reports.Constants;
using Application.Features.Reports.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Reports.Constants.ReportsOperationClaims;

namespace Application.Features.Reports.Commands.Update;

public class UpdateReportCommand : IRequest<UpdatedReportResponse>, ISecuredRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public Guid CounterId { get; set; }
    public DateTime RequestedDate { get; set; }
    public string StatusCode { get; set; }

    public string[] Roles => [Admin, Write, ReportsOperationClaims.Update];

    public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, UpdatedReportResponse>
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        private readonly ReportBusinessRules _reportBusinessRules;

        public UpdateReportCommandHandler(IMapper mapper, IReportRepository reportRepository,
                                         ReportBusinessRules reportBusinessRules)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _reportBusinessRules = reportBusinessRules;
        }

        public async Task<UpdatedReportResponse> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            Report? report = await _reportRepository.GetAsync(predicate: r => r.Id == request.Id, cancellationToken: cancellationToken);
            await _reportBusinessRules.ReportShouldExistWhenSelected(report);
            await _reportBusinessRules.ReportShouldHaveValidStatusCode(report.StatusCode);

            report = _mapper.Map(request, report);

            await _reportRepository.UpdateAsync(report!);

            UpdatedReportResponse response = _mapper.Map<UpdatedReportResponse>(report);
            return response;
        }
    }
}