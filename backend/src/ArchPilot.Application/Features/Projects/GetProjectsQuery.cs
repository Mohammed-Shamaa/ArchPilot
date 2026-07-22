using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Projects;

public class GetProjectsQuery : IRequest<ApiResult<List<ProjectResponse>>> { }
