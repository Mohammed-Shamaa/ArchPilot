using ArchPilot.Application.Common.Models;
using MediatR;

namespace ArchPilot.Application.Features.Documents;

public class GetDocumentsQuery : IRequest<ApiResult<List<DocumentResponse>>>
{
    public Guid ProjectId { get; set; }
}

public class DocumentResponse
{
    public Guid Id { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
