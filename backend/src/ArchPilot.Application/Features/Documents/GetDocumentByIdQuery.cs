using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Documents;

public class GetDocumentByIdQuery : IRequest<ApiResult<DocumentResponse>>
{
    public Guid Id { get; set; }
}
