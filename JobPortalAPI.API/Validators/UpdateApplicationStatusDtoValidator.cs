using FluentValidation;
using JobPortalAPI.API.Constants;
using JobPortalAPI.API.DTOs.Application;

namespace JobPortalAPI.API.Validators;

public class UpdateApplicationStatusDtoValidator : AbstractValidator<UpdateApplicationStatusDto>
{
    public UpdateApplicationStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .Must(x =>
                x == ApplicationStatuses.Accepted ||
                x == ApplicationStatuses.Rejected)
            .WithMessage(
                "Status must be Accepted or Rejected");
    }
}
