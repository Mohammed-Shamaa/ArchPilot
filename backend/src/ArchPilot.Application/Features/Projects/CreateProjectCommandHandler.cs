using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using ArchPilot.Domain.Enums;
using MediatR;

namespace ArchPilot.Application.Features.Projects;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ApiResult<ProjectResponse>>
{
    private readonly IRepository<Project> _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateProjectCommandHandler(IRepository<Project> projectRepository, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<ProjectResponse>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<ProjectResponse>.Failure("User not authenticated");

        var project = new Project
        {
            Id = Guid.NewGuid(),
            UserId = currentUser.Id,
            ProjectName = request.ProjectName,
            Description = request.Description,
            Status = ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            LastAccessedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(project, cancellationToken);

        return ApiResult<ProjectResponse>.SuccessResult(new ProjectResponse
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            Status = project.Status.ToString(),
            CreatedAt = project.CreatedAt,
            LastAccessedAt = project.LastAccessedAt
        });
    }
}
