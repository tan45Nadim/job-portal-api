using FluentValidation;
using JobPortalAPI.API.DTOs.Application;

namespace JobPortalAPI.API.Validators;

public class ApplicationCreateDtoValidator : AbstractValidator<ApplicationCreateDto>
{
    public ApplicationCreateDtoValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty().WithMessage("Job ID is required.");

        RuleFor(x => x.ResumeUrl)
            .NotEmpty().WithMessage("Resume URL is required.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Invalid resume URL format.");

        RuleFor(x => x.CoverLetter)
            .NotEmpty().WithMessage("Cover letter is required.")
            .MinimumLength(20).WithMessage("Cover letter must be at least 20 characters long.");
    }
}
