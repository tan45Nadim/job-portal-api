using FluentValidation;
using JobPortalAPI.API.DTOs.Job;

namespace JobPortalAPI.API.Validators;

public class JobCreateDtoValidator : AbstractValidator<JobCreateDto>
{
    public JobCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(150).WithMessage("Job title must not exceed 150 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Job description is required.");

        RuleFor(x => x.SalaryMin)
            .GreaterThan(0).WithMessage("Minimum salary must be a positive number.");

        RuleFor(x => x.SalaryMax)
            .GreaterThan(0).WithMessage("Maximum salary must be a positive number.");

        RuleFor(x => x)
            .Must(x => x.SalaryMax >= x.SalaryMin)
            .WithMessage(
                "SalaryMax must be greater than SalaryMin");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(x => x.EmploymentType)
            .NotEmpty().WithMessage("Employment type is required.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future.");
    }
}