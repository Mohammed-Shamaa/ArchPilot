using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Entities;
using MediatR;

namespace ArchPilot.Application.Features.Projects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, ApiResult<List<ProjectResponse>>>
{
    private readonly IRepository<Project> _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectsQueryHandler(IRepository<Project> projectRepository, ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResult<List<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.GetCurrentUser();
        if (currentUser == null)
            return ApiResult<List<ProjectResponse>>.Failure("User not authenticated");

        var projects = await _projectRepository.FindAsync(p => p.UserId == currentUser.Id, cancellationToken);
        var response = projects.OrderByDescending(p => p.LastAccessedAt).Select(p => new ProjectResponse
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            Description = p.Description,
            Status = p.Status.ToString(),
            CreatedAt = p.CreatedAt,
            LastAccessedAt = p.LastAccessedAt
        }).ToList();

        return ApiResult<List<ProjectResponse>>.SuccessResult(response);
    }
}
