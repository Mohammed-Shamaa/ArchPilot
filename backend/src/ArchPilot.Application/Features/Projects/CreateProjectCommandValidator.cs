using FluentValidation;

namespace ArchPilot.Application.Features.Projects;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectName)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(200).WithMessage("Project name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");
    }
}
