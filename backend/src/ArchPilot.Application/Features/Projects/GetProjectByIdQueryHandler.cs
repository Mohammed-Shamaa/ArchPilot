using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.Projects;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ApiResult<ProjectResponse>>
{
    private readonly IRepository<Project> _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectByIdQueryHandler(IRepository<Project> projectRepository, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<ProjectResponse>.Failure("User not authenticated");

        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null || project.UserId != currentUser.Id)
            return ApiResult<ProjectResponse>.Failure("Project not found");

        project.LastAccessedAt = DateTime.UtcNow;

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
