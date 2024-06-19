using FluentValidation;

namespace Application.Features.Reports.Commands.Update;

public class UpdateReportCommandValidator : AbstractValidator<UpdateReportCommand>
{
    public UpdateReportCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.CounterId).NotEmpty();
        RuleFor(c => c.RequestedDate).NotEmpty();
        RuleFor(c => c.StatusCode).NotEmpty();
    }
}