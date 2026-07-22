using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Projects;

public class GetProjectByIdQuery : IRequest<ApiResult<ProjectResponse>>
{
    public Guid Id { get; set; }
}
