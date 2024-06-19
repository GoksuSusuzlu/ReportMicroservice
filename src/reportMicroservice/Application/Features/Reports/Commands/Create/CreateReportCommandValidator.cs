using FluentValidation;

namespace Application.Features.Reports.Commands.Create;

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(c => c.CounterId).NotEmpty();
        RuleFor(c => c.RequestedDate).NotEmpty();
        RuleFor(c => c.StatusCode).NotEmpty();
    }
}