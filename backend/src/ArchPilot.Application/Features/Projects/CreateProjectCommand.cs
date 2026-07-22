using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Projects;

public class CreateProjectCommand : IRequest<ApiResult<ProjectResponse>>
{
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastAccessedAt { get; set; }
}
